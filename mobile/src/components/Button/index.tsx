import React from "react";
import {ActivityIndicator, Text, TouchableOpacity} from "react-native";

import {useTheme} from "../../contexts/ThemeContext";
import {getStyleCadastro} from "../../styles/styles.cadastro";

interface ButtonProps {
    label: string,
    onPress: any,
    disabled?: boolean | false,
    loading?: boolean | false,
    type?: string | 'primary'
}

const Button = (props: ButtonProps) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);

    const {
        type = 'primary'
    } = props;

    const getButtonStyle = (type: string) => {
        switch (type) {
            case 'primary':
                return styleCadastro.buttonSave;
            case 'secondary':
                return styleCadastro.secondaryButton;
            case 'tertiary':
                return styleCadastro.tertiaryButton;
            case 'quaternary':
                return styleCadastro.quaternaryButton;
        }
    }

    return (
        <TouchableOpacity
            style={getButtonStyle(type)}
            onPress={props.onPress}
            disabled={props.disabled}
        >
            {props.loading &&
                <ActivityIndicator size="small" color={theme.colors.tertiaryTextColor}/>
            }
            <Text
                style={type === 'primary' ? styleCadastro.textButtonSave : styleCadastro.textSecondaryButton}>{props.label}</Text>
        </TouchableOpacity>
    );
}

export default Button;