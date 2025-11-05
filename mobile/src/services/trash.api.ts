import * as I from "../interfaces/interfaces.tsx";
import {get} from "./api.ts";
import {validateLogin} from "./helper.api.ts";
import {Action} from "../enums/enums.tsx";
import {Alert} from "react-native";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }
    
    return true;
};

export const getTrashs = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Trash?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Get, response)) return null;

    return response;
};