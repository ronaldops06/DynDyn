import { Alert } from 'react-native';
import { Action, StatusHttp } from '../enums/enums';
import * as I from '../interfaces/interfaces';
import { del, get, post, put } from './api';
import {validateLogin} from "./helper.api.ts";

export const validateResponse = (action: Action, response: I.Response) => {
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

export const getTransactions = async (params: string) => {
    let response = {} as I.Response;
    response = await get(`Transaction?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const getTotalsTransactions = async (params: string) => {

    let response = {} as I.Response;
    response = await get(`Transaction/Totais?${params}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Get, response)) return null;

    return response;
};

export const postTransaction = async (data: I.Transaction): Promise<I.Response> => {
    let response = {} as I.Response;

    response = await post('Transaction', data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Post, response)){
        response.data = null;
    }

    return response;
};

export const putTransaction = async (data: I.Transaction): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await put(`Transaction`, data);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    if (!validateResponse(Action.Put, response)){
        response.data = null;
    }

    return response;
};

export const deleteTransaction = async (id: number): Promise<I.Response> => {
    let response = {} as I.Response;
    response = await del(`Transaction/${id}`);

    response = validateLogin(response);
    if (!response.isLogged)
        return response;
    
    validateResponse(Action.Delete, response);
    
    return response;
};