import React from "react";
import {Text, TouchableOpacity} from "react-native";
import Icon from "../Icon";

import {useTheme} from "../../contexts/ThemeContext";
import {getStyleCadastro} from "../../styles/styles.cadastro";

interface AuxiliaryButtonProps {
    text: string;
    onPress: () => void;
    icon: string;
    iconColor: string;
    type?: string | undefined;
    disabled?: boolean | undefined;
}

const AuxiliaryButton = (props: AuxiliaryButtonProps) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    
    const {
        type = 'primary',
        disabled = false
    } = props;
    
    return(
        <TouchableOpacity
            style={type === 'primary' ? styleCadastro.primaryAuxiliaryButton : styleCadastro.secondaryAuxiliaryButton}
            onPress={props.onPress}
            disabled={disabled}
        >
            <Icon name={props.icon} size={30} color={props.iconColor} />
            <Text style={type === 'primary' ? styleCadastro.textButtonSave : styleCadastro.textSecondaryButton }>{props.text}</Text>
        </TouchableOpacity>
    );
}

export default AuxiliaryButton;