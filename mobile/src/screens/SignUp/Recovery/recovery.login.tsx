import React, {useEffect, useState} from 'react';
import {Alert, Image, SafeAreaView, Text, View} from "react-native";
import TextInput from "../../../components/CustomTextInput";
import StepIndicator from "../../../components/StepIndicator";
import Button from "../../../components/Button";

import {executeLoginPasswordRecovery} from "../../../controller/user.controller";

import {useTheme} from '../../../contexts/ThemeContext';
import {getRecoveryStyle} from "./styles";

const RecoveryLogin = ({navigation, route}) => {
    const {theme} = useTheme();
    const recoveryStyle = getRecoveryStyle(theme);

    const [step, setStep] = useState(1);
    const [email, setEmail] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        setEmail(route.params?.login);
    }, []);

    const handleValidateClick = async () => {

        if (email === null || email === "") {
            Alert.alert("O e-mail deve ser informado.");
        } else {
            setLoading(true);

            let response = await executeLoginPasswordRecovery(email);

            navigation.navigate("RecoveryValidation", {
                login: email
            });

            setLoading(true);
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
                <StepIndicator currentStep={step} steps={['Usuário', 'Validação', 'Nova Senha']}/>
                <Text
                    style={recoveryStyle.text}>
                    Para recuperar sua senha, precisaremos seguir alguns passos. Para começar, informe seu e-mail.
                </Text>
                <View style={recoveryStyle.areaFields}>
                    <TextInput
                        text={"Email"}
                        value={email}
                        setValue={setEmail}
                        isMoveText
                    />
                </View>
                <View style={recoveryStyle.areaButtonValidate}>
                    <Button
                        label={"Enviar"}
                        onPress={handleValidateClick}
                        loading={loading}
                        disabled={loading}
                    />
                </View>
                <View style={recoveryStyle.areaCancel}>
                    <Text style={recoveryStyle.cancelTextLink}
                          onPress={handleCancelClick}>
                        Cancelar
                    </Text>
                </View>
            </View>
        </SafeAreaView>
    );
}

export default RecoveryLogin;