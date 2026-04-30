import {ActivityIndicator, Text, TouchableOpacity} from "react-native";
import React, {useState} from "react";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import {useTheme} from "../../contexts/ThemeContext.tsx";

interface ButtonProps {
    label: string,
    onPress: any,
    disabled?: boolean | false,
    loading?: boolean | false,
    type?: string | 'primary'
}

const Button = (props: ButtonProps) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    
    const {
        type = 'primary'
    } = props;
    
    return (
        <TouchableOpacity
            style={type === 'primary' ? styleCadastro.buttonSave : styleCadastro.secondaryButton}
            onPress={props.onPress}
            disabled={props.disabled}
        >
            {props.loading && 
                <ActivityIndicator size="small" color={theme.colors.tertiaryTextColor} />
            }
            <Text style={type === 'primary' ? styleCadastro.textButtonSave : styleCadastro.textSecondaryButton }>{props.label}</Text>
        </TouchableOpacity>
    );
}

export default Button;