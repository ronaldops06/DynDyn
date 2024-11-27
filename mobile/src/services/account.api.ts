import {Action, StatusHttp} from "../enums/enums.tsx";
import * as I from "../interfaces/interfaces.tsx";
import {Alert} from "react-native";
import {get} from "./api.ts";

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
            Alert.alert("Sucesso!", "Conta cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Conta atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Conta excluída com sucesso.");
    }

    return true;
};

export const getAccounts = async (params: string, navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Account?${params}`);

    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};