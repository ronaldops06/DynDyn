import React, {useEffect, useState} from "react";
import {ActivityIndicator, Alert, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import TextInput from "../../../../components/CustomTextInput";

import PrevIcon from "../../../../assets/nav_prev.svg";

import * as I from "../../../../interfaces/interfaces.tsx";
import { alterPasswordUser } from "../../../../controller/user.controller.tsx";
import {encrypt, getUserByStorage, validateLogin, validateSuccess} from "../../../../utils.ts";

import { useTheme } from '../../../../contexts/ThemeContext';
import {getStyleCadastro} from '../../../../styles/styles.cadastro';
import {getStyle} from "../../../../styles/styles.ts";
import {getChangePasswordStyle} from "./styles";
import VisibilityOffIcon from "../../../../assets/visibility_off.svg";
import VisibilityIcon from "../../../../assets/visibility.svg";

const ChangePassword = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const changePasswordStyle = getChangePasswordStyle(theme);

    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [newPasswordConfirm, setNewPasswordConfirm] = useState("");
    const [showOldPassword, setShowOldPassword] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [showPasswordConfirm, setShowPasswordConfirm] = useState(false);
    const [loading, setLoading] = useState(false);
        
    const handleBackClick = () => {
        navigation.goBack();
    };

    const validateRequiredFields = () => {

        if (oldPassword === "") {
            Alert.alert("A senha atual deve ser informada.");
            return false;
        }

        if (newPassword.length < 8) {
            Alert.alert("A nova senha deve ter no mínimo 8 caracteres.");
            return false;
        }
        
        if (newPassword !== newPasswordConfirm) {
            Alert.alert("A senha de confirmação não corresponde com a nova senha.");
            return false;
        }
        
        return true;
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;
        
        setLoading(true);
        let user = await getUserByStorage();
        if (user === null)
            navigation.navigate("SignIn");
        
        let userDto = {} as I.ChangePasswordUser;
        userDto.Login = user?.Login ?? "";
        userDto.Password = await encrypt(oldPassword);
        userDto.NewPassword = await encrypt(newPassword);

        let response =  await alterPasswordUser(userDto);
        setLoading(false);
        
        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'UserAccount');
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                </View>
                <View style={styleCadastro.viewBodyCadastro}>
                    <View style={changePasswordStyle.areaFields}>
                        <TextInput
                            text={"Senha atual"}
                            value={oldPassword}
                            setValue={setOldPassword}
                            secureTextEntry={!showOldPassword}
                            icon={showOldPassword ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                            onPressIcon={() => setShowOldPassword(!showOldPassword)}
                        />
                        <TextInput
                            text={"Nova senha"}
                            value={newPassword}
                            setValue={setNewPassword}
                            secureTextEntry={!showPassword}
                            icon={showPassword ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                            onPressIcon={() => setShowPassword(!showPassword)}
                        />
                        <TextInput
                            text={"Repetir a nova senha"}
                            value={newPasswordConfirm}
                            setValue={setNewPasswordConfirm}
                            secureTextEntry={!showPasswordConfirm}
                            icon={showPasswordConfirm ? <VisibilityOffIcon width={30} fill={theme.colors.primaryIcon}/> : <VisibilityIcon width={30} fill={theme.colors.primaryIcon}/>}
                            onPressIcon={() => setShowPasswordConfirm(!showPasswordConfirm)}
                        />
                    </View>
                    <View style={changePasswordStyle.areaButtonSave}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleSaveClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Alterar</Text>
                        </TouchableOpacity>
                    </View>
                    {loading && (
                        <View style={style.overlay}>
                            <ActivityIndicator size="large" color={theme.colors.primaryTextColor} />
                        </View>
                    )}
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default ChangePassword;