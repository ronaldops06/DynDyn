import React, {useEffect, useState} from 'react';
import {Alert, Image, SafeAreaView, Text, TouchableOpacity, View} from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import ReactNativeBiometrics from 'react-native-biometrics';

import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import {login} from './signin.api';

import {style} from '../../styles/styles';
import {signInStyle} from './styles';
import {encrypt} from "../../utils.ts";

const SignIn = ({navigation}) => {
    
    const [biometricAvailable, setBiometricAvailable] = useState(false);
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");

    useEffect(() => {
        validateBiometricActivated();
    }, []);
    
    const validateBiometricActivated = async () => {
        await getUserByStorage();
        
        let isBiometricActivated = await EncryptedStorage.getItem("biometrics");

        if (isBiometricActivated === "yes") {
            await handleAuthenticateByBiometric();
        } else {
            const reactNativeBiometrics = new ReactNativeBiometrics();

            reactNativeBiometrics.isSensorAvailable()
                .then((result) => {
                    const { available, biometryType } = result;
                    if (available) {
                        setBiometricAvailable(true);
                    }
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }

    const validateAccessBionmetric = async () => {
        Alert.alert("Atenção!",
            "Deseja ativar o login por biometria?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        await EncryptedStorage.setItem("biometrics", "yes");
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const setUserInStorage = async (userStorage: I.User) => {
        await EncryptedStorage.setItem(
            "user_session",
            JSON.stringify(userStorage)
        );
    };

    const getUserByStorage = async (): Promise<I.User | null> => {
        const session = await EncryptedStorage.getItem("user_session");

        if (session) {
            let userStorage = JSON.parse(session);
            
            if (userStorage !== null)
                return userStorage;
        }
        
        return null;
    }

    const handleAuthenticateByBiometric = async () => {
        const reactNativeBiometrics = new ReactNativeBiometrics();

        reactNativeBiometrics.simplePrompt({promptMessage: 'Autenticação biométrica'})
            .then(async (result) => {
                const {success} = result;

                if (success) {
                    let user = await getUserByStorage();
                    if (user)
                        validateLogin(user.Login, user.Password);
                }
            })
            .catch((error) => {
                console.error(error);
                Alert.alert('Error', 'Ocorreu um erro durante a autenticação biométrica');
            });
    };

    const validateLogin = async (email: string, password: string) => {
        let isBiometricActivated = await EncryptedStorage.getItem("biometrics");
        
        let loginDTO = {} as I.Login;
        loginDTO.Login = email;
        loginDTO.Password = password;
        
        let userResponse = await login(loginDTO);
        
        if (userResponse !== null) {
            if (biometricAvailable && isBiometricActivated !== "yes")
                validateAccessBionmetric();
            
            userResponse.Password = loginDTO.Password;
            await setUserInStorage(userResponse);

            navigation.reset({
                routes: [{name: 'MainTab'}]
            });
        } else {
            await EncryptedStorage.removeItem("user_session");
            await EncryptedStorage.removeItem("biometrics");
        }
    }

    const handleSignClick = async () => {
        validateLogin(valueEmail, await encrypt(valuePassword));
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