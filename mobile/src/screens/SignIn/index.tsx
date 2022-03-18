import React, { useState, useEffect } from 'react';
import { SafeAreaView, ScrollView, View, TouchableOpacity, Text, Image, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/core';
import {StackNavigationProp} from '@react-navigation/stack';
import AsyncStorage from '@react-native-community/async-storage';

import { RootStackParamList } from '../RootStackPrams';
import TextInput from '../../components/CustomTextInput';
import { login } from './signin.api';
import * as I from '../../interfaces/interfaces';

import { style } from '../../styles/styles';
import { signInStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'SignIn'>;

const SignIn = () => {
    const navigation = useNavigation<homeScreenProp>();
    
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [user, setUser] = useState<I.User>();

    useEffect(() => {
        
        if (user != null){
            setToken();
            navigation.reset({
                routes:[{name:'MainTab'}]
            });    
        }
    }, [user]);

    const setToken = async () => {
        await AsyncStorage.setItem('token', user != null ? user.token : "");
    };
    
    const handleSignClick = async () => {
        let loginDTO = {} as I.Login;
        loginDTO.login = valueEmail;
        loginDTO.password = valuePassword;

        setUser(await login(loginDTO) ?? {} as I.User);
        /*
        await api.post('User/Auth', {
            login: valueEmail,
            password: valuePassword
        }).then(response => {
            setUser(state => (response.data));
        }).catch((error) => {
            Alert.alert(error.response.data);
        }); 
        */
    };

    const handleRegisterClick = () => {
        navigation.navigate("SignUp");
    };

    return(
        <SafeAreaView style={[style.container,style.containerCadastro]}>
            <Image 
                style={style.viewHeaderCadastro} 
                source={require('../../assets/header.jpg')}
            />
            <View style={signInStyle.viewBodyCadastro}>
                <View style={signInStyle.areaFields}>
                    <TextInput 
                        text={"Email"}
                        value={valueEmail}
                        setValue={setValueEmail}
                    />
                    <TextInput 
                        text={"Senha"}
                        value={valuePassword}
                        setValue={setValuePassword}
                        secureTextEntry={true}
                    />
                    <TouchableOpacity
                        style={signInStyle.button}
                        onPress={handleSignClick}
                    >
                        <Text style={signInStyle.buttonText}>Login</Text>
                    </TouchableOpacity>
                    <Text style={signInStyle.registerText}>
                        Não possui uma conta?&nbsp;
                        <Text
                            style={signInStyle.registerTextLink}
                            onPress={handleRegisterClick}
                        >
                            Cadastrar-se.
                        </Text>
                    </Text>
                </View>
            </View>
        </SafeAreaView>
    );
}

export default SignIn;