import React, {useState} from "react";
import {ActivityIndicator, Alert, SafeAreaView, Text, TouchableOpacity, View} from 'react-native';
import TextInput from "../../../../components/CustomTextInput";
import PrevIcon from "../../../../assets/nav_prev.svg";

import {validateLogin, validateSuccess} from "../../../../utils";

import {executeCleanupUserAccount} from "../../../../controller/user.controller";

import {useTheme} from '../../../../contexts/ThemeContext';
import {getStyle} from "../../../../styles/styles";
import {getStyleCadastro} from '../../../../styles/styles.cadastro';
import {getDeleteAccountStyle} from "./styles";

const DeleteAccount = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const deleteAccountStyle = getDeleteAccountStyle(theme);

    const [loading, setLoading] = useState(false);
    const [email, setEmail] = useState("");

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleExcluirClick = async () => {
        let login = route.params?.login;
        
        if (login !== email)
            Alert.alert("Atenção!", "O usuário informado está incorreto.");

        setLoading(true);
        let response = await executeCleanupUserAccount();

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'SignIn');
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <View style={styleCadastro.viewHeaderCadastro}>
                <TouchableOpacity
                    style={styleCadastro.buttonBack}
                    onPress={handleBackClick}>
                    <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                </TouchableOpacity>
            </View>
            <View style={styleCadastro.viewBodyCadastro}>
                <Text
                    style={deleteAccountStyle.text}>
                    A exclusão da conta é irreversível: todos os dados serão apagados e não poderão ser recuperados.
                </Text>
                <Text
                    style={deleteAccountStyle.text}>
                    Para continuar, informe o seu usuário no campo abaixo.
                </Text>
                <View style={deleteAccountStyle.areaFields}>
                    <TextInput
                        text={"Usuário"}
                        value={email}
                        setValue={setEmail}
                    />
                    <View style={deleteAccountStyle.areaButtonExcluir}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleExcluirClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Excluir</Text>
                        </TouchableOpacity>
                    </View>
                    {loading && (
                        <View style={style.overlay}>
                            <ActivityIndicator size="large" color={theme.colors.primaryTextColor}/>
                        </View>
                    )}
                </View>
            </View>
        </SafeAreaView>
    );
}

export default DeleteAccount;