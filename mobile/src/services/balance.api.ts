import {Action, StatusHttp} from "../enums/enums.tsx";
import * as I from "../interfaces/interfaces.tsx";
import {Alert} from "react-native";
import {del, get, getPaginated, post, put} from "./api.ts";
import {validateLogin} from "./helper.api.ts";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Saldo cadastrado com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Saldo atualizado com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Saldo excluído com sucesso.");
    }

    return true;
};

export const getBalances = async (params: string) => {
    let response = {} as I.Response;
    response = await getPaginated(`Balance?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postBalance = async (data: I.Balance): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Balance', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putBalance = async (data: I.Balance): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Balance`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteBalance = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Balance/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    validateResponse(Action.Delete, response);

    return response;
};