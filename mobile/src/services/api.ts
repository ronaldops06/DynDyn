import axios from 'axios';
import AsyncStorage from '@react-native-community/async-storage';
import { Alert } from 'react-native';

import * as I from '../interfaces/interfaces';

const api = axios.create({
    baseURL: "http://192.168.18.122:15866/api/v1/"
});

export const get = async (path: string) => {
    const token = await AsyncStorage.getItem('token');
    let responseRequest = {} as I.Response;

    await api.get(path, {
        headers: { 'Authorization': 'Bearer ' + token ?? ""}
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.data.status;
        responseRequest.success = true;

        var pagination = response.headers.pagination;
        if (pagination){
            var totalPages = pagination.split('totalPages":')[1].replace('}','');
            responseRequest.totalPages = Number(totalPages);
        }
    }).catch((error) => {
        responseRequest.error = error.response.data;
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const post = async (path: string, data: any) => {
    const token = await AsyncStorage.getItem('token');
    let responseRequest = {} as I.Response;

    await api.post(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? ""}
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.data.status;
        responseRequest.success = true;
    }).catch((error) => {
        responseRequest.error = error.response.data;
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const put = async (path: string, data: any) => {
    const token = await AsyncStorage.getItem('token');
    let responseRequest = {} as I.Response;

    await api.put(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? ""}
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.status;
        responseRequest.success = true;
    }).catch((error) => {
        responseRequest.error = error.response.data;
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const del = async (path: string) => {
    const token = await AsyncStorage.getItem('token');
    let responseRequest = {} as I.Response;

    await api.delete(path,{
        headers: { 'Authorization': 'Bearer ' + token ?? ""}
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.data.status;
        responseRequest.success = true;
    }).catch((error) => {
        responseRequest.error = error.response.data;
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const getLogin = async (path: string, navigation: any) => {
    const token = await AsyncStorage.getItem('token');

    if (token) {
        api.get(path, {
            headers: { 'Authorization': token }
        }).then(response => {
            navigation.reset({
                routes:[{name:'MainTab'}]
            });
        }).catch((error) => {
            if (error.response.status != 401) {
                Alert.alert(error.response.data);
            }
            navigation.navigate('SignIn');
        });
    }
}

export const postLogin = async (path: string, data: I.Login) => {
    let dataResponse = null;

    await api.post(path, data
    ).then(response => {
        dataResponse = response.data;
    }).catch((error) => {
        Alert.alert(error.response.data);
    }); 

    return dataResponse;
};
