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

export const getPortfolios = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Portfolio?${params}`);
    
    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postPortfolio = async (data: I.Portfolio): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Portfolio', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putPortfolio = async (data: I.Portfolio): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Portfolio`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deletePortfolio = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Portfolio/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    validateResponse(Action.Delete, response);

    return response;
};