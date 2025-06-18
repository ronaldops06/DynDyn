import {post, postTransientUser, postValidateUser} from '../../services/api';
import * as I from '../../interfaces/interfaces';
import { Alert } from 'react-native';
import { StatusHttp, Action } from '../../enums/enums';
import {VerificationUser} from "../../interfaces/interfaces";

export const validateResponse = (action: Action, response: I.Response, navigation: any) => {
    if (response.status == StatusHttp.Unauthorized) {
        navigation.navigate("SignIn");
        return false;
    }
    
    if (!response.success){
        Alert.alert("Erro!", response.error);
        return false;
    }

    if (response.status == StatusHttp.Created && action != Action.Get){
        if (action == Action.Post)
            Alert.alert("Sucesso!", "Usuário cadastrado com sucesso.");
    }

    return true;
};

export const postUser = async(data: I.User) => {
    return await postTransientUser('TransientUser', data);
};

export const userValidate = async(data: I.VerificationUser) => {
    return await postValidateUser('TransientUser/UserValidate', data);
};