import React, {useCallback, useEffect, useState} from 'react';
import {ActivityIndicator, Alert, Image, SafeAreaView, Text, TouchableOpacity, View} from "react-native";

import TextInput from "../../../components/CustomTextInput";
import StepIndicator from "../../../components/StepIndicator";
import VisibilityOffIcon from "../../../assets/visibility_off.svg";
import VisibilityIcon from "../../../assets/visibility.svg";
import { executePasswordRecreation} from "../../../controller/user.controller.tsx";

import { useTheme } from '../../../contexts/ThemeContext';
import { getRecoveryStyle } from "./styles";
import { getStyle } from "../../../styles/styles";
import { getStyleCadastro } from "../../../styles/styles.cadastro";
import * as I from "../../../interfaces/interfaces.tsx";
import {encrypt, setUserInStorage} from "../../../utils.ts";
import EncryptedStorage from "react-native-encrypted-storage";
import ReactNativeBiometrics from "react-native-biometrics";
import {updateTokenCloudMessaging} from "../../../controller/firebase.controller.tsx";

const RecoveryPassword = ({navigation, route}) => {
    const { theme } = useTheme();
    const recoveryStyle = getRecoveryStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const style = getStyle(theme);
    
    const [step, setStep] = useState(3);
    const [loading, setLoading] = useState(false);
    const [biometricAvailable, setBiometricAvailable] = useState(false);
    const [email, setEmail] = useState("");
    const [verificationToken, setVerificationToken] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirm, setPasswordConfirm] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [showPasswordConfirm, setShowPasswordConfirm] = useState(false);

    useEffect(() => {
        setEmail(route.params?.login);
        setVerificationToken(route.params?.verificationToken);
        validateBiometricActivated();
    }, []);

    const validateBiometricActivated = async () => {
        await ReactNativeBiometrics.isSensorAvailable()
            .then((result) => {
                const {available, biometryType} = result;
                if (available && biometryType === ReactNativeBiometrics.Biometrics) {
                    setBiometricAvailable(true);
                }
            })
            .catch((error) => {
                console.error(error);
            });
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
    
    const handleRecuperarContaClick = async () => {

        if (password != passwordConfirm) {
            Alert.alert("Atenção!", "A senha de confirmação não corresponde com a senha.");
        } else {
            setLoading(true);
            let passwordRecreation = {} as I.PasswordRecreation;
            passwordRecreation.VerificationToken = verificationToken;
            passwordRecreation.Login = email;
            passwordRecreation.Password = await encrypt(passwordConfirm);

            let response = await executePasswordRecreation(passwordRecreation);
            setLoading(false);
            if (!response.success) {
                Alert.alert("Erro!", response.error);
            } else {
                if (biometricAvailable)
                    validateAccessBionmetric();

                response.data.Password = passwordRecreation.Password;
                await setUserInStorage(response.data);

                await updateTokenCloudMessaging();
                
                navigation.reset({
                    routes: [{name: 'MainTab'}]
                });
            }
        }
    }

    const handleCancelClick = async () => {
        navigation.navigate("SignIn");
    }
        
    return (
        <SafeAreaView style={[recoveryStyle.container, recoveryStyle.containerCadastro]}>
            <Image
                style={recoveryStyle.viewHeaderCadastro}
                source={require('../../../assets/header.jpg')}
            />
            <View style={recoveryStyle.viewBodyCadastro}>
                <StepIndicator currentStep={step} steps={['Usuário', 'Validação', 'Nova Senha']} />
                <Text
                    style={recoveryStyle.text}>
                    Pronto, agora basta informar a sua nova senha.
                </Text>
                <View style={recoveryStyle.areaFields}>
                    <TextInput
                        text={"Email"}
                        value={email}
                        setValue={setEmail}
                        editable={false}
                    />
                    <TextInput
                        text={"Senha"}
                        value={password}
                        setValue={setPassword}
                        secureTextEntry={!showPassword}
                        icon={showPassword ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                        onPressIcon={() => setShowPassword(!showPassword)}
                    />
                    <TextInput
                        text={"Repetir a senha"}
                        value={passwordConfirm}
                        setValue={setPasswordConfirm}
                        secureTextEntry={!showPasswordConfirm}
                        icon={showPasswordConfirm ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                        onPressIcon={() => setShowPasswordConfirm(!showPasswordConfirm)}
                    />
                </View>
                <View style={recoveryStyle.areaButtonValidate}>
                    <TouchableOpacity
                        style={styleCadastro.buttonSave}
                        onPress={handleRecuperarContaClick}
                    >
                        <Text style={styleCadastro.textButtonSave}>Recuperar</Text>
                    </TouchableOpacity>
                </View>
                <View style={recoveryStyle.areaCancel}>
                    <Text style={recoveryStyle.cancelTextLink}
                          onPress={handleCancelClick}>
                        Cancelar
                    </Text>
                </View>
                {loading && (
                    <View style={style.overlay}>
                        <ActivityIndicator size="large" color={theme.colors.primaryTextColor} />
                    </View>
                )}
            </View>
        </SafeAreaView>
    );
}

export default RecoveryPassword;