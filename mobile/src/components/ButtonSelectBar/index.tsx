import React from 'react';
import {Text, TouchableOpacity, View} from 'react-native';

import {buttonSelectStyle} from './styles';

export interface ButtonsSelectedProps {
    value: number;
    text: string;
}

interface ButtonSelectProps {
    buttons: ButtonsSelectedProps[];
    handleValueSelected: any;
    valueSelected: number;
    disabled: boolean;
}

const ButtonSelectBar = (props: ButtonSelectProps) => {
    const getButtonStyle = (type: number) => {
        return [buttonSelectStyle.button, (props.valueSelected == type ?
            buttonSelectStyle.buttonSelected :
            buttonSelectStyle.buttonDefault)];
    };

    const getTextButtonStyle = (type: number) => {
        return [buttonSelectStyle.textButton, (props.valueSelected == type ?
            buttonSelectStyle.textButtonSelected :
            buttonSelectStyle.textButtonDefault)];
    };

    return (
        <View style={buttonSelectStyle.areaButtonSelect}>
            {props.buttons.map((button) => {
                return (
                    <TouchableOpacity
                        style={getButtonStyle(button.value)}
                        onPress={() => props.handleValueSelected(button.value)}
                        disabled={props.disabled}>
                        <Text style={getTextButtonStyle(button.value)}>{button.text}</Text>
                    </TouchableOpacity>)
            })}

        </View>
    );
}

export default ButtonSelectBar;