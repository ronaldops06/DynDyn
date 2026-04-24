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
            Alert.alert("Sucesso!", "Papel de operação cadastrado com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Papel de operação atualizado com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Papel de operação excluído com sucesso.");
    }

    return true;
};

export const getOperationsRoles = async (params: string) => {
    let response = {} as I.Response;
    response = await getPaginated(`OperationRole?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postOperationRole = async (data: I.OperationRole): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('OperationRole', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putOperationRole = async (data: I.OperationRole): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`OperationRole`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteOperationRole = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`OperationRole/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    validateResponse(Action.Delete, response);
    
    return response;
};