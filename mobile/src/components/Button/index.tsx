import {ActivityIndicator, Text, TouchableOpacity} from "react-native";
import React, {useState} from "react";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import {useTheme} from "../../contexts/ThemeContext.tsx";

interface ButtonProps {
    label: string,
    onPress: any,
    disabled?: boolean | false,
    loading?: boolean | false
}

const Button = (props: ButtonProps) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    
    return (
        <TouchableOpacity
            style={styleCadastro.buttonSave}
            onPress={props.onPress}
            disabled={props.disabled}
        >
            {props.loading && 
                <ActivityIndicator size="small" color={theme.colors.tertiaryTextColor} />
            }
            <Text style={styleCadastro.textButtonSave}>{props.label}</Text>
        </TouchableOpacity>
    );
}

export default Button;