import React, {ReactNode} from "react";
import {SafeAreaView, ScrollView, Text, TouchableOpacity, View} from "react-native";
import PrevIcon from "../../../assets/nav_prev.svg";

import {useTheme} from "../../../contexts/ThemeContext";
import {getStyle} from "../../../styles/styles";
import {getStyleSpecial} from "../../../styles/styles.special";

interface PageSpecialProps {
    onBackClick: any;
    helpType: string;
    title: string;
    children: ReactNode;
}

const PageSpecial = ({onBackClick, helpType, title, children}: PageSpecialProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const stylesSpecial = getStyleSpecial(theme);
    
    return(
        <SafeAreaView style={stylesSpecial.container}>
            <View style={stylesSpecial.viewBody}>
                <View style={stylesSpecial.titleScreen}>
                    <TouchableOpacity
                        style={stylesSpecial.buttonBack}
                        onPress={onBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIconDashboard}/>
                    </TouchableOpacity>
                    <View style={style.titleScreenTitle}>
                        <Text style={style.textPrimary24Bold}>{title}</Text>
                    </View>
                </View>
                <ScrollView style={style.scrollCadastro}>
                    {children}
                </ScrollView>
            </View>
        </SafeAreaView>
    );
}

export default PageSpecial;