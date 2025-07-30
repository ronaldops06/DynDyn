import * as I from "../interfaces/interfaces.tsx";
import {Alert} from "react-native";
import {
    postLoginPasswordRecovery,
    postPasswordRecoveryValidate,
    postPasswordRecreation,
    postPasswordUser
} from "../services/user.api.ts";
import { setUserInStorage } from "../utils.ts";
import EncryptedStorage from "react-native-encrypted-storage";
import {postTransientUserParamQuery} from "../services/api.ts";

export const alterPasswordUser = async (changePasswordUser: I.ChangePasswordUser): Promise<I.Response> => {
    let response = await postPasswordUser(changePasswordUser);

    if (response && !response.isLogged)
        return response;
    
    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível alterar a senha.");
    } else if (response.data !== null){
        response.data.Password = changePasswordUser.NewPassword;
        await EncryptedStorage.removeItem("user_session");
        await setUserInStorage(response.data);
    }

    return response;
}

export const executeLoginPasswordRecovery = async (login: string): Promise<I.Response> => {
    let response = await postLoginPasswordRecovery(login);
    
    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível recuperar sua conta.");
    }

    return response;
}

export const executePasswordRecoveryValidate = async (data: I.VerificationUser): Promise<I.Response> => {
    let response = await postPasswordRecoveryValidate(data);

    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível recuperar sua conta.");
    }

    return response;
}

export const executePasswordRecreation = async (data: I.PasswordRecreation): Promise<I.Response> => {
    let response = await postPasswordRecreation(data);

    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível recuperar sua conta.");
    }

    return response;
}