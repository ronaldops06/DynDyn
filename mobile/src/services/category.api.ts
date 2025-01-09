import * as I from "../interfaces/interfaces.tsx";
import {del, get, post, put} from "./api.ts";
import {Action, StatusHttp} from "../enums/enums.tsx";
import {Alert} from "react-native";
import {validateLogin} from "./helper.api.ts";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Categoria cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Categoria atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Categoria excluída com sucesso.");
    }

    return true;
};

export const getCategories = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Category?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postCategory = async (data: I.Category): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Category', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putCategory = async (data: I.Category): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Category`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteCategory = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Category/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    validateResponse(Action.Delete, response);
    
    return response;
};