import { useNavigation } from '@react-navigation/core';
import { StackNavigationProp } from '@react-navigation/stack';
import React, { useState } from 'react';
import { Alert, Image, SafeAreaView, Text, TouchableOpacity, View } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';

import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import { RootStackParamList } from '../RootStackParams';
import { postUser } from './signup.api';

import { style } from '../../styles/styles';
import { signUpStyle } from './styles';
import {encrypt} from "../../utils.ts";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'SignUp'>;

const SignUp = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [valueName, setValueName] = useState("");
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [valuePasswordConfirm, setValuePasswordConfirm] = useState("");
    const [user, setUser] = useState<I.User>({ Id: 0, Name: "", Login: "", AccessToken: "", Password: "" });

    const handleRegisterClick = async () => {
        if (valuePassword != valuePasswordConfirm) {
            Alert.alert("Senha de confirmação não corresponde com a senha.");
        } else {
            let userDTO = {} as I.User;
            userDTO.Name = valueName;
            userDTO.Login = valueEmail;
            userDTO.Password = await encrypt(valuePasswordConfirm);

            const response = await postUser(userDTO, navigation);
            setUser(response?.data);

            if (user.AccessToken) {
                user.Password = userDTO.Password;
                await setUserInStorage();

                navigation.reset({
                    routes: [{ name: 'MainTab' }]
                })
            }
        }
    };

    const setUserInStorage = async () => {
        await EncryptedStorage.setItem(
            "user_session",
            JSON.stringify(user)
        );
    };

    const handleLoginClick = () => {
        navigation.navigate("SignIn");
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
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