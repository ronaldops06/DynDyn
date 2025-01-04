import * as I from "../interfaces/interfaces.tsx";
import {del, get, post, put} from "./api.ts";
import {Action, StatusHttp} from "../enums/enums.tsx";
import {Alert} from "react-native";

export const validateResponse = (action: Action, response: I.Response, navigation: any) => {
    if (response.status == StatusHttp.Unauthorized) {
        navigation.navigate("SignIn");
        return false;
    }

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

export const getCategories = async (params: string, navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Category?${params}`);

    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const postCategory = async (data: I.Category, navigation: any): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Category', data);

    if (!validateResponse(Action.Post, response, navigation)){
        response.data = null;
    }

    return response;
};

export const putCategory = async (data: I.Category, navigation: any): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Category`, data);

    if (!validateResponse(Action.Put, response, navigation)){
        response.data = null;
    }

    return response;
};

export const deleteCategory = async (id: number, navigation: any) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Category/${id}`);

    validateResponse(Action.Delete, response, navigation);
    
    return response;
};