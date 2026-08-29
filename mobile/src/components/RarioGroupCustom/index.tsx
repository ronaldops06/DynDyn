import {SymbolString} from "../../interfaces/interfaces.tsx";
import RadioGroup, {RadioButtonProps} from "react-native-radio-buttons-group";
import {getRadioGroupStyles} from "./styles";
import {useTheme} from "../../contexts/ThemeContext.tsx";
import React from "react";
import {Text, View} from "react-native";

interface RadioGroupCustomProps {
    label: string;
    data: SymbolString[];
    onPress: (item: string) => void;
    value: string;
    isEditing?: boolean | undefined;
}

const RadioGroupCustom = (props: RadioGroupCustomProps) => {
    const {theme} = useTheme();
    const radioGroupStyles = getRadioGroupStyles(theme);

    const getRadioButtonsData = (): RadioButtonProps[] => {
        return props.data.map(item => {
            return {
                id: item.Id,
                label: item.Name,
                value: item.Id,
                color: theme.colors.quaternaryTextColor,
                size: 16,
                labelStyle: radioGroupStyles.labelOption,
                selected: false,
                disabled: props.isEditing ?? false
            };
        }) as RadioButtonProps[];
    }

    return (
        <View>
            <Text style={radioGroupStyles.radioLabel}>{props.label}</Text>
            <RadioGroup
                radioButtons={getRadioButtonsData()}
                onPress={props.onPress}
                selectedId={props.value}
                layout="row"
                containerStyle={radioGroupStyles.radioActionType}
                
            />
        </View>
    );
}

export default RadioGroupCustom;