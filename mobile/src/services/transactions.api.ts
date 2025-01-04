import { Alert } from 'react-native';
import { Action, StatusHttp } from '../enums/enums';
import * as I from '../interfaces/interfaces';
import { del, get, post, put } from './api';

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
            Alert.alert("Sucesso!", "Transação cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Transação atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Transação excluída com sucesso.");
    }

    return true;
};

export const getTransactions = async (params: string, navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Transaction?${params}`);

    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const getTotalsTransactions = async (params: string, navigation: any) => {

    let response = {} as I.Response;
    response = await get(`Transaction/Totais?${params}`);

    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const postTransaction = async (data: I.Transaction, navigation: any): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Transaction', data);

    if (!validateResponse(Action.Post, response, navigation)){
        response.data = null;
    }

    return response;
};

export const putTransaction = async (data: I.Transaction, navigation: any): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Transaction`, data);

    if (!validateResponse(Action.Put, response, navigation)){
        response.data = null;
    }

    return response;
};

export const deleteTransaction = async (id: number, navigation: any) => {
    let response = {} as I.Response;
    response = await del(`Transaction/${id}`);

    validateResponse(Action.Delete, response, navigation);
};