import React, {useEffect, useState} from "react";
import {SafeAreaView, Text, TouchableOpacity, View, Switch, Alert} from "react-native";
import DeviceInfo from 'react-native-device-info';

import {getAccountUserStyles} from "./styles";
import {getStyle} from "../../../styles/styles.ts";
import { useTheme } from '../../../contexts/ThemeContext';
import PrevIcon from "../../../assets/nav_prev.svg";
import NextIcon from "../../../assets/nav_next.svg";
import LockIcon from "../../../assets/lock.svg";
import NotificationIcon from "../../../assets/notification.svg";
import DarkModeIcon from "../../../assets/dark_mode.svg";
import HelpIcon from "../../../assets/help_center.svg";
import TrashIcon from "../../../assets/trash.svg";
import LogoutIcon from "../../../assets/logout.svg";

import {User} from "../../../interfaces/interfaces.tsx";
import {getUserByStorage} from "../../../utils.ts";
import EncryptedStorage from "react-native-encrypted-storage";

const UserAccount = ({navigation, route}) => {
    const { theme, toggleTheme } = useTheme();
    const accountUserStyles = getAccountUserStyles(theme);
    const style = getStyle(theme);
    
    const [user, setUser] = useState<User>({} as User);

    useEffect(() => {
        loadUser();
    }, []);

    const loadUser = async () => {
        const user = await getUserByStorage();
        setUser(user);
    };

    const handleBackClick = () => {
        navigation.goBack();
    };
    
    const handleLogoutClick = () => {
        Alert.alert("Atenção!",
            "Deseja realmente sair do aplicativo?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        await EncryptedStorage.removeItem("user_session");
                        await EncryptedStorage.removeItem("biometrics");
                        navigation.navigate("SignIn");
                    }
                }
            ],
            {cancelable: false}
        );
    };
    
    const getAbbreviationName = (value) => {
        return value?.split(' ')
            .map(palavra => palavra.charAt(0))
            .join('');
    }

    return (
        <SafeAreaView style={accountUserStyles.container}>
                <View style={accountUserStyles.viewBody}>
                    <View style={accountUserStyles.titleScreen}>
                        <TouchableOpacity
                            style={accountUserStyles.buttonBack}
                            onPress={handleBackClick}>
                            <PrevIcon width="40" height="40" fill={theme.colors.primaryIconDashboard}/>
                        </TouchableOpacity>
                        <View style={style.titleScreenTitle}>
                            <Text style={accountUserStyles.titleScreemText}>Perfil</Text>
                        </View>
                    </View>
                    <View style={[accountUserStyles.viewBlock, accountUserStyles.viewUser]}>
                        <View style={accountUserStyles.viewUserProfile}>
                            <Text style={accountUserStyles.userProfileText}>{getAbbreviationName(user.Name)}</Text>
                        </View>
                        <View style={accountUserStyles.row}>
                            <Text style={accountUserStyles.userNameText}>{user.Name}</Text>
                            <Text style={accountUserStyles.userLoginText}>{user.Login}</Text>
                        </View>
                        <NextIcon style={accountUserStyles.iconNext} width="30" height="30"
                                  fill={theme.colors.primaryIconDashboard}/>
                    </View>
                    <Text style={accountUserStyles.textSettings}>Configurações</Text>
                    <View style={[accountUserStyles.viewBlock, accountUserStyles.viewSettings]}>
                        <View style={accountUserStyles.menuItem}>
                            <LockIcon width="30" height="30" fill={theme.colors.primaryIconDashboard}/>
                            <Text style={accountUserStyles.menuItemText}>Alterar a senha</Text>
                            <NextIcon style={accountUserStyles.iconNext} width="30" height="30"
                                      fill={theme.colors.primaryIconDashboard}/>
                        </View>
                        <View style={accountUserStyles.menuItem}>
                            <NotificationIcon width="30" height="30" fill={theme.colors.primaryIconDashboard}/>
                            <Text style={accountUserStyles.menuItemText}>Notificações</Text>
                            <NextIcon style={accountUserStyles.iconNext}
                                      width="30"
                                      height="30"
                                      fill={theme.colors.primaryIconDashboard}
                            />
                        </View>
                        <View style={accountUserStyles.menuItem}>
                            <DarkModeIcon width="30" height="30" fill={theme.colors.primaryIconDashboard}/>
                            <Text style={accountUserStyles.menuItemText}>Modo Escuro</Text>
                            <Switch style={accountUserStyles.iconNext}
                                    value={theme.dark}
                                    onValueChange={toggleTheme}
                                    trackColor={{
                                        false: theme.colors.tertiaryBaseColor,
                                        true: theme.colors.tertiaryBaseColor
                                    }}
                                    thumbColor={theme.dark ? theme.colors.primaryBaseColor : theme.colors.primaryBaseColor}/>
                        </View>
                    </View>
                    <View style={[accountUserStyles.viewBlock, accountUserStyles.viewAccount]}>
                        <View style={accountUserStyles.menuItem}>
                            <HelpIcon width="30" height="30" fill={theme.colors.primaryIconDashboard}/>
                            <Text style={accountUserStyles.menuItemText}>Ajuda</Text>
                            <NextIcon style={accountUserStyles.iconNext}
                                      width="30"
                                      height="30"
                                      fill={theme.colors.primaryIconDashboard}
                            />
                        </View>
                        <View style={accountUserStyles.menuItem}>
                            <TrashIcon width="30" height="30" fill={theme.colors.dangerBaseColor}/>
                            <Text style={[accountUserStyles.menuItemText, accountUserStyles.menuItemTextDanger]}>Excluir conta</Text>
                            <NextIcon style={accountUserStyles.iconNext}
                                      width="30"
                                      height="30"
                                      fill={theme.colors.primaryIconDashboard}
                            />
                        </View>
                    </View>
                </View>
                    <View style={accountUserStyles.viewAbout}>
                        <TouchableOpacity
                            style={accountUserStyles.abboutSairButton}
                            onPress={handleLogoutClick}
                        >
                            <LogoutIcon width="25" height="25" fill={theme.colors.secondaryIconDashboard}/>
                            <Text style={accountUserStyles.abboutSairButtonText}> Sair</Text>
                        </TouchableOpacity>
                        <Text style={accountUserStyles.abboutText}>SageMoney - Versão {DeviceInfo.getVersion()} (Build {DeviceInfo.getBuildNumber()})</Text>
                    </View>
        </SafeAreaView>
    );
}

export default UserAccount;