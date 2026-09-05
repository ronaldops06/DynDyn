import * as I from "./interfaces/interfaces";
import sha256 from 'crypto-js/sha256';
import {constants} from "./constants";
import Moment from "moment/moment";
import SecureStorage from "./secureStorage";
import NetInfo from '@react-native-community/netinfo';

export function isEndScroll(event: any) {
    let mHeight = event.nativeEvent.layoutMeasurement.height;
    let cSize = event.nativeEvent.contentSize.height;
    let Y = event.nativeEvent.contentOffset.y;

    return (Math.ceil(mHeight + Y) >= cSize);
}

export const validateLogin = (response: I.Response, navigation: any) => {
    if (!response.isLogged)
        navigation.navigate("SignIn");
}

export const validateSuccess = (response: I.Response, navigation: any, screem: string, reference: string | null = null) => {
    if (response.success) {
        if (screem) {
            navigation.popTo(screem, {
                actionNavigation: constants.actionNavigation.reload,
                referenceId: response.data?.Id,
                reference: reference
            });
        } else {
            navigation.goBack();
        }
    }
}

export const setUserInStorage = async (userStorage: I.User) => {
    await SecureStorage.set(
        "user_session",
        JSON.stringify(userStorage)
    );
};

export const getUserByStorage = async (): Promise<I.User | null> => {
    const session = await SecureStorage.get("user_session");

    if (session) {
        let userStorage = JSON.parse(session);

        if (userStorage !== null)
            return userStorage;
    }

    return null;
}

export const getUserLoginEncrypt = async (): Promise<string> => {
    let user = await getUserByStorage();

    if (user)
        return encrypt(user.Login);
}

export const getCurrentStack = (navigation: any): string => {
    const parent = navigation.getParent();
    return parent?.getState().routes[parent.getState().index].name;
}

export const encrypt = async (value: string): Promise<string> => {
    return sha256(value).toString();
}

export const getDate = (): Date => new Date(Moment().utc(true).format('YYYY-MM-DD HH:mm:ss'));

export const toLocalDate = (date): Date => Moment.parseZone(date, 'DD/MM/YYYY HH:mm:ss').toDate();

export const isInternetConnected = async (): Promise<boolean> => {
    const state = await NetInfo.fetch();
    return state.isConnected ?? false;
};

export const getDescriptionStatus = (statusId: number) => {
    return Object.values(constants.status).find(x => x.Id === statusId).Name;
}

export type Operator = '>' | '>=' | '<' | '<=' | '=' | '!=';

const operators: Record<Operator, (a: number, b: number) => boolean> = {
    '>': (a, b) => a > b,
    '>=': (a, b) => a >= b,
    '<': (a, b) => a < b,
    '<=': (a, b) => a <= b,
    '=': (a, b) => a === b,
    '!=': (a, b) => a !== b,
};

export const filterDynamic = <T>(
    data: T[],
    getter: (item: T) => number | undefined,
    operator: Operator,
    value: number
): T[] => {
    if (!operator || (value == undefined)) return data;

    return data.filter(item =>
        operators[operator](getter(item), value)
    );
};

export const hasAnyFilter = (filter: Record<string, any>) =>
    Object.entries(filter).some(item => {
        if (typeof item[1] === 'string') {
            return item[1].trim().length > 0;
        } else if (typeof item[1] === 'number') {
            if (item[0].slice(-2) === 'Id')
                return item[1] > 0;
            else
                return item[1] > -1;
        } else if (typeof item[1] === 'boolean') {
            return Boolean(item[1]);
        } else if (item[0] === 'ValueFilter') {
            return (item[1]?.Operator && (item[1]?.Value != undefined))
        }
    });