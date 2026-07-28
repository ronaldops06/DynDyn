import * as I from "../interfaces/interfaces.tsx";
import {del, getPaginated, post, put} from "./api.ts";
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
            Alert.alert("Sucesso!", "Atributo cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Atributo atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Atributo excluída com sucesso.");
    }

    return true;
};

export const getAttributes = async (params: string) => {
    let response = {} as I.Response;
    response = await getPaginated(`Attribute?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postAttribute = async (data: I.Attribute): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Attribute', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putAttribute = async (data: I.Attribute): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Attribute`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteAttribute = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Attribute/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    validateResponse(Action.Delete, response);
    
    return response;
};