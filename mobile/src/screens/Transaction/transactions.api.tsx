import React from 'react';
import { get, post, put, del } from '../../services/api';
import { TypesCategory, StatusHttp, Action } from '../../enums/enums';
import * as I from '../../interfaces/interfaces';
import { Alert } from 'react-native';

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
            Alert.alert("Sucesso!", "Transação cadastrada com sucesso.");
        else if (action == Action.Put)
            Alert.alert("Sucesso!", "Transação atualizada com sucesso.");
        else if (action == Action.Delete)
            Alert.alert("Sucesso!", "Transação excluída com sucesso.");

        navigation.goBack();
    }

    return true;
};

export const getCategories = async (navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Categoria?Tipo=${TypesCategory.Operation}`);
    
    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const getAccounts = async (navigation: any) => {
    let response = {} as I.Response;
    response = await get(`Conta`);
    
    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const getTransactions = async(params: string, navigation: any) =>{
    let response = {} as I.Response;
    response = await get(`Movimento?${params}`);
    
    if (!validateResponse(Action.Get, response, navigation)) return null;

    return response;
};

export const getTotalsTransactions = async(params: string, navigation: any) =>{
    
    let response = {} as I.Response;
    response = await get(`Movimento/Totais?${params}`);
    
    if (!validateResponse(Action.Get, response, navigation)) return null;
    
    return response;
};

export const postTransaction = async(data: I.Transaction, navigation: any) => {
    let response = {} as I.Response;
    response = await post('Movimento', data);
    
    if (!validateResponse(Action.Post, response, navigation)) return null;

    return response;
};

export const putTransaction = async(data: I.Transaction, navigation: any) => {
    let response = {} as I.Response;
    response = await put(`Movimento/${data.id}`, data);
    
    if (!validateResponse(Action.Put, response, navigation)) return null;

    return response;
};

export const deleteTransaction = async(id: number, navigation: any) => {
    let response = {} as I.Response;
    response = await del(`Movimento/${id}`);
    
    validateResponse(Action.Delete, response, navigation);
};