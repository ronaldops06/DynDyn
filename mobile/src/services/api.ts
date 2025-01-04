import NetInfo from '@react-native-community/netinfo';
import axios from 'axios';
import { Alert } from 'react-native';
import { MMKV } from 'react-native-mmkv';

import * as I from '../interfaces/interfaces';

const api = axios.create({
    baseURL: "http://192.168.0.11:5000/api/v1/"
});

const isInternetConnected = async (): Promise<boolean> => {
    const state = await NetInfo.fetch();
    return state.isConnected ?? false;
  };

export const get = async (path: string) => {

    const storage = new MMKV();
    const token = await storage.getString('token');

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }

    await api.get(path, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.data.status;
        responseRequest.success = true;

        var pagination = response.headers.pagination;
        if (pagination) {
            var totalPages = pagination.split('totalPages":')[1].replace('}', '');
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
    const storage = new MMKV();
    const token = storage.getString('token');

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }
    
    await api.post(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.status;
        responseRequest.success = true;
    }).catch((error) => {
        console.log(error?.response?.data);
        responseRequest.error = error.response.data;
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const put = async (path: string, data: any) => {
    const storage = new MMKV();
    const token = await storage.getString('token');

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }

    await api.put(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
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
    const storage = new MMKV();
    const token = await storage.getString('token');

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }

    await api.delete(path, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
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
    const storage = new MMKV();
    const token = storage.getString('token');

    if (token) {
        api.get(path, {
            headers: { 'Authorization': 'Bearer ' + token }
        }).then(response => {
            navigation.reset({
                routes: [{ name: 'MainTab' }]
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
