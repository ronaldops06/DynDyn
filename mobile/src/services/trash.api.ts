import {Alert} from "react-native";
import * as I from "../interfaces/interfaces";
import {getPaginated} from "./api";
import {validateLogin} from "./helper.api";
import {Action} from "../enums/enums";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }
    
    return true;
};

export const getTrashs = async (params: string) => {
    let response = {} as I.Response;
    response = await getPaginated(`Trash?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};