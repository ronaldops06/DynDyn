import NetInfo from '@react-native-community/netinfo';
import axios, {AxiosInstance} from 'axios';
import { Alert } from 'react-native';

import * as I from '../interfaces/interfaces';
import EncryptedStorage from "react-native-encrypted-storage";
import {constants} from "../constants";

const configUrl = 'https://sagemoney.com.br/config/config.json';

const getToken = async (): Promise<string> => {
    const session = await EncryptedStorage.getItem("user_session");
    
    if (session) 
        return JSON.parse(session).AccessToken;
    
    return '';
}

const isInternetConnected = async (): Promise<boolean> => {
    const state = await NetInfo.fetch();
    return state.isConnected ?? false;
};

const formatErrors = (error: any): string => {
    if (error?.response?.data?.errors?.Value) {
        return error?.response?.data?.errors?.Value.join();
    } else if (error?.response?.data?.errors) {
        return Object.entries(error?.response?.data?.errors)
            .map(([field, messages]) => `${field}: ${messages.join('; ')}`)
            .join(". ");
    } else if (error?.response?.data) {
        return error?.response?.data;
    } else if (error.response) {
        return `Erro ao realizar a comunicação com o servidor. Status: ${error.response.status}`;
    } else if (error.request) {
        return 'O servidor está indisponível no momento, tente novamente mais tarde.'
    }
    
    return "Erro não identificado ao realizar a comunicação com o servidor.";
}

export const fetchApiUrl = async (): Promise<string | null> => {
    try {
        //Se estiver executando em modo debug (localmente), usa o endereço local da API.
        if (__DEV__)
            return "http://192.168.0.8:5000";
        
        const response = await fetch(configUrl);
        if (!response.ok) throw new Error('Erro ao buscar configuração');

        const data = await response.json();
        
        return data.api_url;
    } catch (error) {
        console.error('Erro ao buscar API URL:', error);
        return null;
    }
}

const getApiInstance = async () => {
    const apiUrl = await EncryptedStorage.getItem("API_URL");

    return axios.create({
        baseURL: `${apiUrl}/api/v1/`,
    });
}

export const getPaginated = async (path: string): Promise<I.Response> => {
    let pathWithPages = `${path}&PageNumber=1&PageSize=${constants.pageSizeRequest}`;
    let responseAllPages = await get(pathWithPages);
    
    if (!responseAllPages.success || !responseAllPages.isConnected) {
        return responseAllPages;
    } else if (responseAllPages.totalPages > 1){
        for (let i = 2; i <= responseAllPages.totalPages; i++) {
            pathWithPages = `${path}&PageNumber=${i}&PageSize=${constants.pageSizeRequest}`;
            
            let response = await get(pathWithPages);
            responseAllPages.data.push(...response.data);
        }
    }
    
    return responseAllPages;
}

export const get = async (path: string): Promise<I.Response> => {
    let token = await getToken();

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }
    
    let api = await getApiInstance();
    
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
        console.log('erro', error?.response?.data);
        responseRequest.error = error?.response?.data?.errors?.Value.join() ?? error?.response?.data;
        responseRequest.status = error?.response?.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const post = async (path: string, data?: any) => {
    let token = await getToken();

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }
    
    let api = await getApiInstance();
    await api.post(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.status;
        responseRequest.success = true;
    }).catch((error) => {
        responseRequest.error = formatErrors(error);
        responseRequest.status = error?.response?.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const postParamQuery = async (path: string): Promise<I.Response> => {
    let dataResponse: I.Response = {} as I.Response;

    dataResponse.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        dataResponse.isConnected = false;
        dataResponse.success = true;
        return dataResponse;
    }

    let token = await getToken();
    let api = await getApiInstance();
    await api.post(path, null, {
        headers: {'Authorization': 'Bearer ' + token ?? ""}
    }).then(response => {
        dataResponse.data = response.data;
        dataResponse.success = true;
        dataResponse.status = response.data.status;
    }).catch((error) => {
        dataResponse.error = formatErrors(error);
        dataResponse.status = error.response.status;
        dataResponse.success = false;
    });

    return dataResponse;
};

export const put = async (path: string, data: any) => {
    let token = await getToken();
    
    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }

    let api = await getApiInstance();
    await api.put(path, data, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.status;
        responseRequest.success = true;
    }).catch((error) => {
        responseRequest.error = formatErrors(error);        
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });
    
    return responseRequest;
};

export const del = async (path: string) => {
    let token = await getToken();

    let responseRequest = {} as I.Response;
    responseRequest.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        responseRequest.isConnected = false;
        responseRequest.success = true;
        return responseRequest;
    }

    let api = await getApiInstance();
    await api.delete(path, {
        headers: { 'Authorization': 'Bearer ' + token ?? "" }
    }).then(response => {
        responseRequest.data = response.data;
        responseRequest.status = response.data.status;
        responseRequest.success = true;
    }).catch((error) => {
        console.log('erro', error?.response?.data);
        responseRequest.error = error?.response?.data?.errors?.Value.join();
        responseRequest.status = error.response.status;
        responseRequest.success = false;
    });

    return responseRequest;
};

export const getLogin = async (path: string, navigation: any) => {
    let token = await getToken();

    if (token) {
        let api = await getApiInstance();
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

//Anonymous
export const postTransientUser = async (path: string, data: any): Promise<I.Response> => {
    let dataResponse: I.Response = {} as I.Response;

    let api = await getApiInstance();
    await api.post(path, data
    ).then(response => {
        dataResponse.success = true;
        dataResponse.status = response.data.status;
    }).catch((error) => {
        console.log('erro', error?.response?.data);
        dataResponse.error = error?.response?.data?.errors?.Value.join();
        dataResponse.status = error.response.status;
        dataResponse.success = false;
    });

    return dataResponse;
};

export const postPasswordRecovery = async (path: string): Promise<I.Response> => {
    let dataResponse: I.Response = {} as I.Response;

    dataResponse.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        dataResponse.isConnected = false;
        dataResponse.success = true;
        return dataResponse;
    }
    
    let api = await getApiInstance();
    await api.post(path
    ).then(response => {
        dataResponse.data = response.data;
        dataResponse.success = true;
        dataResponse.status = response.data.status;
    }).catch((error) => {
        console.log('erro', error);
        dataResponse.error = error;
        dataResponse.status = error.response.status;
        dataResponse.success = false;
    });

    return dataResponse;
};

export const postValidateUser = async (path: string, data: any): Promise<I.Response> => {
    let dataResponse: I.Response = {} as I.Response;

    dataResponse.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        dataResponse.isConnected = false;
        dataResponse.success = true;
        return dataResponse;
    }
    
    let api = await getApiInstance();
    await api.post(path, data
    ).then(response => {
        dataResponse.data = response?.data;
        dataResponse.success = true;
        dataResponse.status = response.data.status;
    }).catch((error) => {
        dataResponse.error = error?.response?.data;
        dataResponse.status = error.response.status;
        dataResponse.success = false;
    });

    return dataResponse;
};

export const postLogin = async (path: string, data: I.Login): Promise<I.Response> => {
    let dataResponse: I.Response = {} as I.Response;

    dataResponse.isConnected = true;

    let isConnected = await isInternetConnected();
    if (!isConnected) {
        dataResponse.isConnected = false;
        dataResponse.success = true;
        return dataResponse;
    }

    let api = await getApiInstance();
    await api.post(path, data
    ).then(response => {
        dataResponse.data = response?.data;
        dataResponse.success = true;
        dataResponse.status = response.data.status;
    }).catch((error) => {
        dataResponse.error = formatErrors(error);
        dataResponse.status = error?.response?.status;
        dataResponse.success = false;
    });

    return dataResponse;
};
