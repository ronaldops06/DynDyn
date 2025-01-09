import {Action, StatusHttp} from "../enums/enums.tsx";
import * as I from "../interfaces/interfaces.tsx";
import {Alert} from "react-native";
import {del, get, post, put} from "./api.ts";
import {validateLogin} from "./helper.api.ts";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Conta cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Conta atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Conta excluída com sucesso.");
    }

    return true;
};

export const getAccounts = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Account?${params}`);
    
    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postAccount = async (data: I.Account): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Account', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putAccount = async (data: I.Account): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Account`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteAccount = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Account/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    validateResponse(Action.Delete, response);

    return response;
};