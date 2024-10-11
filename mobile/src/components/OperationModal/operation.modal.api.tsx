import { Alert } from 'react-native';
import { Action } from '../../enums/enums';
import * as I from '../../interfaces/interfaces';
import { get } from '../../services/api';

export const validateResponse = (action: Action, response: I.Response) => {

    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    return true;
};

export const getOperations = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Operation?${params}`);

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};