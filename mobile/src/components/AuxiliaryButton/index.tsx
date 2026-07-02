import React, {ReactNode} from "react";
import {Text, TouchableOpacity} from "react-native";
import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";

interface AuxiliaryButtonProps {
    text: string;
    onPress: () => void;
    icon: ReactNode;
    type?: string | undefined;
}

const AuxiliaryButton = (props: AuxiliaryButtonProps) => {
    const { theme } = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    
    const {
        type = 'primary'
    } = props;
    
    return(
        <TouchableOpacity
            style={type === 'primary' ? styleCadastro.primaryAuxiliaryButton : styleCadastro.secondaryAuxiliaryButton}
            onPress={props.onPress}>
            {props.icon}
            <Text style={type === 'primary' ? styleCadastro.textButtonSave : styleCadastro.textSecondaryButton }>{props.text}</Text>
        </TouchableOpacity>
    );
}

export default AuxiliaryButton;