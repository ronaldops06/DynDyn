import React, { useState } from 'react';
import { SafeAreaView, View, TouchableOpacity, Text, Image, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/core';
import {StackNavigationProp} from '@react-navigation/stack';
import AsyncStorage from '@react-native-community/async-storage';

import { RootStackParamList } from '../RootStackPrams';
import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import { postUser } from './signup.api';

import { style } from '../../styles/styles';
import { signUpStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'SignUp'>;

interface User {
    id: number,
    login: string,
    name: string,
    token: string
};

const SignUp = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [valueName, setValueName] = useState("");
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [valuePasswordConfirm, setValuePasswordConfirm] = useState("");
    const [user, setUser] = useState<User>({id: 0, name: "", login: "", token: ""});

    const handleRegisterClick = async () => {
        if (valuePassword != valuePasswordConfirm){
            Alert.alert("Senha de confirmação não corresponde com a senha.");
        } else {
            let userDTO = {} as I.User;
            userDTO.name = valueName;
            userDTO.login = valueEmail;
            userDTO.password = valuePasswordConfirm;

            const response = await postUser(userDTO, navigation);
            setUser(response?.data);
            
            if (user.token){
                await AsyncStorage.setItem('token', user.token);
                navigation.reset({
                    routes:[{name:'MainTab'}]
                })
            }
        }
    };

    const handleLoginClick = () => {
        navigation.navigate("SignIn");
    };

    return(
        <SafeAreaView style={[style.container,style.containerCadastro]}>
            <Image 
                style={style.viewHeaderCadastro} 
                source={require('../../assets/header.jpg')}
            />
            <View style={signUpStyle.viewBodyCadastro}>
                <View style={signUpStyle.areaFields}>
                    <TextInput
                        text={"Nome"}
                        value={valueName}
                        setValue={setValueName}
                    />
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
                    <TextInput 
                        text={"Repetir a senha"}
                        value={valuePasswordConfirm}
                        setValue={setValuePasswordConfirm}
                        secureTextEntry={true}
                    />
                    <TouchableOpacity
                        style={signUpStyle.button}
                        onPress={handleRegisterClick}>
                        <Text style={signUpStyle.buttonText}>Salvar</Text>
                    </TouchableOpacity>
                    <Text style={signUpStyle.loginText}>
                        Já possui uma conta?&nbsp;
                        <Text
                            style={signUpStyle.loginTextLink}
                            onPress={handleLoginClick}>
                            Fazer login.
                        </Text>
                    </Text>
                </View>
            </View>
        </SafeAreaView>
    );
}

export default SignUp;