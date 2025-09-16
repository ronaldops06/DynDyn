import React, {useEffect, useState} from 'react';
import {ActivityIndicator, Alert, Image, SafeAreaView, Text, TouchableOpacity, View} from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import ReactNativeBiometrics from 'react-native-biometrics';

import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import {login} from './signin.api';

import {encrypt, getUserByStorage} from "../../utils.ts";
import VisibilityIcon from "../../assets/visibility.svg";
import VisibilityOffIcon from "../../assets/visibility_off.svg";

import { useTheme } from '../../contexts/ThemeContext';
import {getStyle} from '../../styles/styles';
import {getSignInStyle} from './styles';
import {executeLoginPasswordRecovery} from "../../controller/user.controller.tsx";
import {updateTokenCloudMessaging} from "../../controller/firebase.controller.tsx";

const SignIn = ({navigation}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const signInStyle = getSignInStyle(theme);
    
    const [biometricAvailable, setBiometricAvailable] = useState(false);
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        validateBiometricActivated();
    }, []);
    
    const validateBiometricActivated = async () => {
        await getUserByStorage();
        
        let isBiometricActivated = await EncryptedStorage.getItem("biometrics");
        
        if (isBiometricActivated === "yes") {
            await handleAuthenticateByBiometric();
        } else {
            await ReactNativeBiometrics.isSensorAvailable()
                .then((result) => {
                    const { available, biometryType } = result;
                    if (available && biometryType === ReactNativeBiometrics.Biometrics) {
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
    
    const handleAuthenticateByBiometric = async () => {
        const {success} = await ReactNativeBiometrics.simplePrompt({promptMessage: 'Autenticação biométrica'});
        
        if (success) {
            let user = await getUserByStorage();
            if (user)
                validateLogin(user.Login, user.Password);
        }
    };

    const validateLogin = async (email: string, password: string) => {
        setLoading(true);
        let isBiometricActivated = await EncryptedStorage.getItem("biometrics");
        
        let loginDTO = {} as I.Login;
        loginDTO.Login = email;
        loginDTO.Password = password;
        let userResponse = await login(loginDTO);
        setLoading(false);
        
        if (userResponse !== null) {
            if (biometricAvailable && isBiometricActivated !== "yes")
                validateAccessBionmetric();
            
            userResponse.Password = loginDTO.Password;
            await setUserInStorage(userResponse);

            await updateTokenCloudMessaging();
            
            navigation.reset({
                routes: [{name: 'MainTab'}]
            });
        } else {
            await EncryptedStorage.removeItem("user_session");
            await EncryptedStorage.removeItem("biometrics");
        }
    }

    const handleSignClick = async () => {
        await validateLogin(valueEmail, await encrypt(valuePassword));
    };

    const handleRegisterClick = () => {
        navigation.navigate("SignUp");
    };
    
    const handlePasswordRecoveryClick = async () => {
        navigation.navigate("RecoveryLogin", {
            login: valueEmail
        });
    }

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
                        secureTextEntry={!showPassword}
                        icon={showPassword ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                        onPressIcon={() => setShowPassword(!showPassword)}
                    />
                    <TouchableOpacity
                        style={signInStyle.button}
                        onPress={handleSignClick}
                    >
                        <Text style={signInStyle.buttonText}>Login</Text>
                    </TouchableOpacity>
                    {loading && (
                        <View style={style.overlay}>
                            <ActivityIndicator size="large" color={theme.colors.primaryTextColor} />
                        </View>
                    )}
                    <Text style={signInStyle.registerText}>
                        Não possui uma conta?&nbsp;
                        <Text
                            style={signInStyle.registerTextLink}
                            onPress={handleRegisterClick}
                        >
                            Cadastrar-se.
                        </Text>
                    </Text>
                    <Text
                        style={signInStyle.registerTextLink}
                        onPress={handlePasswordRecoveryClick}
                    >
                        Esqueceu a senha?
                    </Text>
                </View>
            </View>
        </SafeAreaView>
    );
}

export default SignIn;