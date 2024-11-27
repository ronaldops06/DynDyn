import { Alert } from 'react-native';
import {Action, StatusHttp} from "../enums/enums.tsx";
import * as I from "../interfaces/interfaces.tsx";
import {get} from "./api.ts";

export const validateResponse = (action: Action, response: I.Response, navigation: any) => {
    if (navigation !== null && response.status == StatusHttp.Unauthorized) {
        navigation.navigate("SignIn");
        return false;
    }

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

export const getOperations = async (params: string, navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Operation?${params}`);

    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};