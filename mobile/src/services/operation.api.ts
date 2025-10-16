import { Alert } from 'react-native';
import {Action, StatusHttp} from "../enums/enums.tsx";
import * as I from "../interfaces/interfaces.tsx";
import {del, get, post, put} from "./api.ts";
import {validateLogin} from "./helper.api.ts";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Operação cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Operação atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Operação excluída com sucesso.");
    }

    return true;
};

export const getOperations = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Operation?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postOperation = async (data: I.Operation): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Operation', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putOperation = async (data: I.Operation): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Operation`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteOperation = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Operation/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    validateResponse(Action.Delete, response);

    return response;
};