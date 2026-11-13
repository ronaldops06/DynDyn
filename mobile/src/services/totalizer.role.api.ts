import {Alert} from "react-native";
import * as I from "../interfaces/interfaces";
import {del, getPaginated, post, put} from "./api";
import {Action, StatusHttp} from "../enums/enums";
import {validateLogin} from "./helper.api";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Papel de totalizador cadastrado com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Papel de totalizador atualizado com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Papel de totalizador excluído com sucesso.");
    }

    return true;
};

export const getTotalizersRoles = async (params: string) => {
    let response = {} as I.Response;
    response = await getPaginated(`TotalizerRole?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postTotalizerRole = async (data: I.TotalizerRole): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('TotalizerRole', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putTotalizerRole = async (data: I.TotalizerRole): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`TotalizerRole`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteTotalizerRole = async (id: number) : Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`TotalizerRole/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    validateResponse(Action.Delete, response);

    return response;
};