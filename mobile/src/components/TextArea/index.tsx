import React from "react";
import {Text, TextInput, View} from "react-native";

import {useTheme} from "../../contexts/ThemeContext";
import {getTextAreaStyle} from "./styles";

interface TextAreaProps {
    label: string;
    value: string;
    setValue: (value: string) => void;
    placeholder?: string;
}

const TextArea = ({label, value, setValue, placeholder}: TextAreaProps) => {
    const {theme} = useTheme();
    const styles = getTextAreaStyle(theme);

    return (
        <View style={styles.field}>
            <Text style={styles.fieldLabel}>{label}</Text>
            <TextInput
                style={styles.textArea}
                value={value}
                onChangeText={setValue}
                placeholder={placeholder}
                placeholderTextColor={theme.colors.quintenaryTextColor}
                multiline
                textAlignVertical="top"
            />
        </View>
    );
};

export default TextArea;
