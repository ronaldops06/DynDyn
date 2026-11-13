import * as I from "../interfaces/interfaces";
import {Alert} from "react-native";
import {
    postCleanupUserAccount,
    postLoginPasswordRecovery,
    postPasswordRecoveryValidate,
    postPasswordRecreation,
    postPasswordUser
} from "../services/user.api";
import {getUserLoginEncrypt, setUserInStorage} from "../utils";
import SecureStorage from "../secureStorage";
import {deleteAllBalances} from "../repository/balance.repository";
import {deleteAllPortfolios} from "../repository/portfolio.repository";
import {deleteAllOperations} from "../repository/operation.repository";
import {deleteAllCategories} from "../repository/category.repository";
import {deleteAllSynchronizations} from "../repository/synchronization.repository";
import {deleteAllTransactions} from "../repository/transaction.repository";
import {updateTokenCloudMessaging} from "./firebase.controller";

export const alterPasswordUser = async (changePasswordUser: I.ChangePasswordUser): Promise<I.Response> => {
    let response = await postPasswordUser(changePasswordUser);

    if (response && !response.isLogged)
        return response;
    
    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível alterar a senha.");
    } else if (response.data !== null){
        response.data.Password = changePasswordUser.NewPassword;
        await SecureStorage.remove("user_session");
        await setUserInStorage(response.data);
        await updateTokenCloudMessaging();
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

export const executeCleanupUserAccount = async (): Promise<I.Response> => {
    let response = await postCleanupUserAccount();
    
    if (!response.isConnected) {
        Alert.alert("Atenção!", "Sem conexão com a internet, não foi possível excluir sua conta.");
    } else if (response.data !== null) {
        let login = await getUserLoginEncrypt();
        await deleteAllBalances(login);
        await deleteAllTransactions(login);
        await deleteAllPortfolios(login);
        await deleteAllOperations(login);
        await deleteAllCategories(login);
        await deleteAllSynchronizations(login);
        
        await SecureStorage.remove("user_session");
    }

    return response;
}