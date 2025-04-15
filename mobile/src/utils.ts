import * as I from "./interfaces/interfaces.tsx";
import sha256 from 'crypto-js/sha256';
import {constants} from "./constants";
import Moment from "moment/moment";

export function isEndScroll(event: any) {
    let mHeight = event.nativeEvent.layoutMeasurement.height;
    let cSize = event.nativeEvent.contentSize.height;
    let Y = event.nativeEvent.contentOffset.y;

    if (Math.ceil(mHeight + Y) >= cSize) return true;
    return false;
}

export const validateLogin = (response: I.Response, navigation: any) => {
    if (!response.isLogged)
        navigation.navigate("SignIn");
}

export const validateSuccess = (response: I.Response, navigation: any, route: any) => {
    if (response.success) {
        if (route?.params?.onGoBack)
            route.params.onGoBack(constants.actionNavigation.reload);
        
        navigation.goBack();
    }
}

export const encrypt = async (value: string): Promise<string> => {
    return sha256(value).toString();
}

export const getDate = (): Date => new Date(Moment().utc(true).format('YYYY-MM-DD HH:mm:ss'));

export const toLocalDate = (date): Date => Moment.parseZone(date, 'DD/MM/YYYY HH:mm:ss').toDate();