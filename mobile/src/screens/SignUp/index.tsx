import React, {useState} from 'react';
import {Alert, Image, SafeAreaView, ScrollView, Text, TouchableOpacity, View, ActivityIndicator} from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';

import TextInput from '../../components/CustomTextInput';
import * as I from '../../interfaces/interfaces';
import {postUser} from './signup.api';
import {encrypt} from "../../utils.ts";

import {style} from '../../styles/styles';
import {styleCadastro} from "../../styles/styles.cadastro.ts";
import {signUpStyle} from './styles';
import VisibilityIcon from "../../assets/visibility.svg";
import VisibilityOffIcon from "../../assets/visibility_off.svg";
import {constants} from "../../constants";

const SignUp = ({navigation}) => {

    const [loading, setLoading] = useState(false);
    const [valueName, setValueName] = useState("");
    const [valueEmail, setValueEmail] = useState("");
    const [valuePassword, setValuePassword] = useState("");
    const [valuePasswordConfirm, setValuePasswordConfirm] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [showPasswordConfirm, setShowPasswordConfirm] = useState(false);
    
    const handleRegisterClick = async () => {
        if (valuePassword != valuePasswordConfirm) {
            Alert.alert("Senha de confirmação não corresponde com a senha.");
        } else {
            setLoading(true);
            let userDTO = {} as I.User;
            userDTO.Name = valueName;
            userDTO.Login = valueEmail;
            userDTO.Password = await encrypt(valuePasswordConfirm);

            let response = await postUser(userDTO);
            setLoading(false);
            if (!response.success) {
                Alert.alert("Erro!", response.error);
            } else {
                navigation.navigate("Validation", {
                    data: userDTO,
                });
            }
        }
    };

    const handleLoginClick = () => {
        navigation.navigate("SignIn");
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <Image
                    style={style.viewHeaderCadastro}
                    source={require('../../assets/header.jpg')}
                />
                <View style={styleCadastro.viewBodyCadastro}>
                    <View style={styleCadastro.areaFields}>
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
                            secureTextEntry={!showPassword}
                            icon={showPassword ? <VisibilityOffIcon width={30} fill="#6E8BB8"/> : <VisibilityIcon width={30} fill="#6E8BB8"/>}
                            onPressIcon={() => setShowPassword(!showPassword)}
                        />
                        <TextInput
                            text={"Repetir a senha"}
                            value={valuePasswordConfirm}
                            setValue={setValuePasswordConfirm}
                            secureTextEntry={!showPasswordConfirm}
                            icon={showPasswordConfirm ? <VisibilityOffIcon width={30} fill="#6E8BB8"/> : <VisibilityIcon width={30} fill="#6E8BB8"/>}
                            onPressIcon={() => setShowPasswordConfirm(!showPasswordConfirm)}
                        />
                    </View>
                    <View style={signUpStyle.areaButtonSave}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleRegisterClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Salvar</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={signUpStyle.areaTextLogin}>
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
                {loading && (
                    <View style={signUpStyle.overlay}>
                        <ActivityIndicator size="large" color={constants.colors.primaryTextColor} />
                    </View>
                )}
            </ScrollView>
        </SafeAreaView>
    );
}

export default SignUp;