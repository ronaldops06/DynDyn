import { useNavigation } from '@react-navigation/core';
import { StackNavigationProp } from '@react-navigation/stack';
import React, { useEffect, useState } from 'react';
import { Image, SafeAreaView, Text, TouchableOpacity, View } from 'react-native';
import { MMKV } from 'react-native-mmkv';

import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import { RootStackParamList } from '../RootStackParams';
import { login } from './signin.api';

import { style } from '../../styles/styles';
import { signInStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'SignIn'>;

const SignIn = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [user, setUser] = useState<I.User | null>();

    useEffect(() => {

        if (user != null) {
            setToken();
            navigation.reset({
                routes: [{ name: 'MainTab' }]
            });
        }
    }, [user]);

    const setToken = async () => {
        const storage = new MMKV();
        storage.set('token', user != null ? user.AccessToken : "");
    };

    const handleSignClick = async () => {
        let loginDTO = {} as I.Login;
        loginDTO.Login = valueEmail;
        loginDTO.Password = valuePassword;

        setUser(await login(loginDTO));
    };

    const handleRegisterClick = () => {
        navigation.navigate("SignUp");
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
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