import React from "react";
import {Switch, Text, View} from "react-native";

import CheckboxIcon from "../../assets/check_box.svg"

import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getCheckBoxStyle} from "./styles";

interface CheckBoxProps {
    value: boolean;
    onValueChange: (value: boolean) => void;
    label: string;
    description?: string | undefined;
}

const CheckBox = (props: CheckBoxProps) => {
    const {theme} = useTheme();
    const styleCheckBox = getCheckBoxStyle(theme);

    const getAreaCheckboxStyle = () => {
        return props.value ? styleCheckBox.areaCheckboxChecked : styleCheckBox.areaCheckboxUnchecked;
    }

    return (
        <View style={styleCheckBox.areaCard}>
            <CheckboxIcon width="45" height="45" stroke={theme.colors.primaryIconDashboard}/>
            <View style={styleCheckBox.areaText}>
                <Text style={styleCheckBox.label}>{props.label}</Text>
                {props.description &&
                    <Text style={styleCheckBox.description}>{props.description}</Text>
                }
            </View>
            <View style={[styleCheckBox.areaCheckbox, getAreaCheckboxStyle()]}>
                <Switch style={styleCheckBox.checkbox}
                        value={props.value}
                        onValueChange={props.onValueChange}
                        trackColor={{
                            false: theme.colors.secondaryBaseColor,
                            true: theme.colors.primaryBaseColor,
                        }}
                        thumbColor={props.value ? theme.colors.secondaryBaseColor : theme.colors.primaryBaseColor}/>
            </View>
        </View>
    );
}

export default CheckBox;