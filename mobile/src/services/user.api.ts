import * as I from "../interfaces/interfaces.tsx";
import {del, postPasswordRecovery, post, postParamQuery, postValidateUser} from "./api.ts";
import {validateLogin} from "./helper.api.ts";
import {Action, StatusHttp} from "../enums/enums.tsx";
import {Alert} from "react-native";
import {PasswordRecreation} from "../interfaces/interfaces.tsx";

export const validateResponse = (action: Action, response: I.Response) => {
    if (!response.success) {
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get) {
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Usuário alterado com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Usuário atualizado com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Usuário excluído com sucesso.");
    }

    return true;
};

export const postPasswordUser = async (data: I.ChangePasswordUser): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await post(`Login/ChangePassword`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;

    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const postLoginPasswordRecovery = async (login: string): Promise<I.Response> => {
    let response = await postPasswordRecovery(`TransientUser/LoginPasswordRecovery?login=${login}`);
    if (response.success) {
        Alert.alert("Info!", response.data);
    } else {
        Alert.alert("Erro!", response.error);
    }
    
    return response;
};

export const postCleanupUserAccount = async (): Promise<I.Response> => {
    let response = await postParamQuery(`Maintenance/Cleanup`);
    if (response.success) {
        Alert.alert("Info!", response.data);
    } else {
        Alert.alert("Erro!", response.error);
    }

    return response;
}

export const postPasswordRecoveryValidate = async (data: I.VerificationUser): Promise<I.Response> => {
    return await postValidateUser(`TransientUser/PasswordRecoveryValidate`, data);
};

export const postPasswordRecreation = async (data: I.PasswordRecreation): Promise<I.Response> => {
    return await postValidateUser(`TransientUser/PasswordRecreation`, data);
};

export const postDevice = async (data: I.Device): Promise<I.Response> => {
    let response = await post('Device/RegisterDevice', data);
    
    if (!response.isLogged || !response.isConnected)
        return response;
    
    if (!response.success) {
        Alert.alert("Erro!", response.error);
    }
}