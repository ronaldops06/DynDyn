import {Image, TextInput, SafeAreaView, Text, View, TouchableOpacity, Alert, ActivityIndicator} from "react-native";
import React, {useState, useRef, useEffect} from "react";
import {postUser, userValidate} from "./signup.api.tsx";
import * as I from "../../interfaces/interfaces.tsx";
import EncryptedStorage from "react-native-encrypted-storage";
import ReactNativeBiometrics from "react-native-biometrics";

import { setUserInStorage } from "../../utils.ts";

import { useTheme } from '../../contexts/ThemeContext';
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import {getValidationStyle} from "./validation.styles";
import { getStyle } from "../../styles/styles"
import {updateTokenCloudMessaging} from "../../controller/firebase.controller.tsx";

const Validation = ({navigation, route}) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const validationStyle = getValidationStyle(theme);
    const style = getStyle(theme);
    
    const inputRefs = useRef([]);
    const [loading, setLoading] = useState(false);
    const [biometricAvailable, setBiometricAvailable] = useState(false);
    const [valueVerificationCode, setValueVerificationCode] = useState(Array(6).fill(""));
    const [email, setEmail] = useState("");

    useEffect(() => {
        let data = route.params?.data;
        validateBiometricActivated();
        setEmail(data.Login);
    }, []);
    
    const handleChange = (text, index) => {
        const newCodes = [...valueVerificationCode];
        newCodes[index] = text;
        setValueVerificationCode(newCodes);

        if (text && index < 5) {
            inputRefs.current[index + 1]?.focus();
        }
    };

    const handleKeyPress = ({nativeEvent}, index) => {
        if (nativeEvent.key === 'Backspace' && valueVerificationCode[index] === '' && index > 0) {
            inputRefs.current[index - 1]?.focus();
        }
    };

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

    const handleResendClick = async () => {
        let data = route.params?.data;
        setLoading(true);
        await postUser(data);
        setLoading(false);
    };

    const handleValidateClick = async () => {
        let data = route.params?.data;

        if (valueVerificationCode.join('').trim().length < 6) {
            Alert.alert("Informe os 6 caracteres do código de verificação.");
        } else if (data !== undefined && data !== null) {
            setLoading(true);
            let verificationUserDTO = {} as I.VerificationUser;
            verificationUserDTO.Login = data.Login;
            verificationUserDTO.VerificationCode = parseInt(valueVerificationCode.join('').trim());

            var response = await userValidate(verificationUserDTO);
            setLoading(false);
            
            if (!response.success){
                Alert.alert("Erro!", response.error);
            } else if (response.data !== null) {
                if (biometricAvailable)
                    validateAccessBionmetric();
                
                response.data.Password = data?.Password;
                await setUserInStorage(response.data);
                
                await updateTokenCloudMessaging();
                
                navigation.reset({
                    routes: [{name: 'MainTab'}]
                });
            }
        }
    };
    
    return (
        <SafeAreaView style={[validationStyle.container, validationStyle.containerCadastro]}>
            <Image
                style={validationStyle.viewHeaderCadastro}
                source={require('../../assets/header.jpg')}
            />
            <View style={validationStyle.viewBodyCadastro}>
                <Text
                    style={validationStyle.text}>
                    Você receberá um código de verificação no e-mail {email}, informe o código a seguir para validar o
                    usuário.
                </Text>
                <View style={validationStyle.areaFields}>
                    {valueVerificationCode.map((value, index) => (
                        <TextInput
                            key={index}
                            ref={ref => (inputRefs.current[index] = ref)}
                            style={validationStyle.input}
                            maxLength={1}
                            keyboardType="number-pad"
                            value={value}
                            onChangeText={text => handleChange(text, index)}
                            onKeyPress={e => handleKeyPress(e, index)}
                        />
                    ))}
                </View>
                <View style={validationStyle.areaButtonValidate}>
                    <TouchableOpacity
                        style={styleCadastro.buttonSave}
                        onPress={handleValidateClick}
                    >
                        <Text style={styleCadastro.textButtonSave}>Validar</Text>
                    </TouchableOpacity>
                </View>
                <View style={validationStyle.areaResend}>
                    <Text style={validationStyle.resendText}>
                        Não recebeu o código de verificação?&nbsp;
                        <Text style={validationStyle.resendTextLink}
                              onPress={handleResendClick}>
                            Enviar novamente.
                        </Text>
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
};

export default Validation;