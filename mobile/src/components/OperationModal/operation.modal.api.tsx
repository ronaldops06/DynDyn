import { get } from '../../services/api';
import { Action } from '../../enums/enums';
import * as I from '../../interfaces/interfaces';
import { Alert } from 'react-native';

export const validateResponse = (action: Action, response: I.Response) => {
    
    if (!response.success){
        Alert.alert("Erro!", response.error);
        return false;
    }

    return true;
};

export const getOperations = async(params: string) =>{
    let response = {} as I.Response;
    response = await get(`Operacao?${params}`);
    
    if (!validateResponse(Action.Get, response)) return null;

    return response;
};