import {Image, TextInput, SafeAreaView, Text, View, TouchableOpacity, Alert, ActivityIndicator} from "react-native";
import React, {useState, useRef, useEffect} from "react";
import {postUser, userValidate} from "../signup.api.tsx";
import * as I from "../../../interfaces/interfaces.tsx";
import EncryptedStorage from "react-native-encrypted-storage";

import { useTheme } from '../../../contexts/ThemeContext';
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import {getRecoveryStyle} from "./styles";
import { getStyle } from "../../../styles/styles"
import StepIndicator from "../../../components/StepIndicator";
import {executePasswordRecoveryValidate} from "../../../controller/user.controller.tsx";

const RecoveryValidation = ({navigation, route}) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const recoveryStyle = getRecoveryStyle(theme);
    const style = getStyle(theme);

    const [step, setStep] = useState(2);
    const inputRefs = useRef([]);
    const [loading, setLoading] = useState(false);
    const [valueVerificationCode, setValueVerificationCode] = useState(Array(6).fill(""));
    const [email, setEmail] = useState("");

    useEffect(() => {
        setEmail(route.params?.login);
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
    
    const handleValidateClick = async () => {
 
        if (valueVerificationCode.join('').trim().length < 6) {
            Alert.alert("Atenção!", "Informe os 6 caracteres do código de verificação.");
        } else if (email !== "") {
            setLoading(true);
            let verificationUserDTO = {} as I.VerificationUser;
            verificationUserDTO.Login = email;
            verificationUserDTO.VerificationCode = parseInt(valueVerificationCode.join('').trim());

            var response = await executePasswordRecoveryValidate(verificationUserDTO);
            console.log(response)
            setLoading(false);
            
            if (!response.success){
                Alert.alert("Erro!", response.error);
            } else if (response.data !== null) {
                navigation.navigate("RecoveryPassword", {
                    login: email,
                    verificationToken: response?.data?.VerificationToken
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
                    Você receberá um código de verificação no e-mail {email}, informe o código a seguir para validar o
                    usuário.
                </Text>
                <View style={recoveryStyle.areaFieldsValidation}>
                    {valueVerificationCode.map((value, index) => (
                        <TextInput
                            key={index}
                            ref={ref => (inputRefs.current[index] = ref)}
                            style={recoveryStyle.input}
                            maxLength={1}
                            keyboardType="number-pad"
                            value={value}
                            onChangeText={text => handleChange(text, index)}
                            onKeyPress={e => handleKeyPress(e, index)}
                        />
                    ))}
                </View>
                <View style={recoveryStyle.areaButtonValidate}>
                    <TouchableOpacity
                        style={styleCadastro.buttonSave}
                        onPress={handleValidateClick}
                    >
                        <Text style={styleCadastro.textButtonSave}>Validar</Text>
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
};

export default RecoveryValidation;