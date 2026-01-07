import React, { useEffect, useRef } from "react";
import { View, TextInput, Text, Animated, TouchableOpacity } from "react-native";

import { useTheme } from '../../contexts/ThemeContext';
import { getCustomTextInputStyle } from './styles';

interface CustomTextInputParams {
    text: string;
    isMoveText?: boolean | undefined;
    value: string;
    setValue: any;
    messageText?: string | undefined;
    secureTextEntry?: boolean | undefined;
    icon?: any | undefined;
    onPressIcon?: any | undefined;
    width?: string | undefined;
    editable?: boolean;
    autoFocus?: boolean
}

const CustomTextInput = (props: CustomTextInputParams) => {
    const { theme } = useTheme();
    const customTextInputStyle = getCustomTextInputStyle(theme);
    
    const {
        isMoveText = true,
        width = "100%",
        editable = true,
        autoFocus = false
    } = props;
    
    const moveText = useRef(new Animated.Value((isMoveText) ? 0 : 1)).current;

    useEffect(() => {
        if (props.value !== ""){
            moveTextTop();
        } else if (props.value === ""){
            moveTextBottom();
        }
    }, [props.value]);

    const onChangeText = (text: string) => {
        props.setValue(text);
    };

    const onFocusHandler = () => {
        if (props.value !== ""){
            moveTextTop();
        }
    };

    const onBlurHandler = () => {
        if (props.value === "") {
            moveTextBottom();
        }
    };

    const moveTextTop = () => {
        Animated.timing(moveText, {
            toValue: 1,
            duration: 200,
            useNativeDriver: true,
        }).start();
    };

    const moveTextBottom = () => {
        if (isMoveText) {
            Animated.timing(moveText, {
                toValue: 0,
                duration: 200,
                useNativeDriver: true,
            }).start();
        }
    };

    const yVal = moveText.interpolate({
        inputRange: [0, 1],
        outputRange: [4, -20]
    });

    const xVal = moveText.interpolate({
        inputRange: [0, 1],
        outputRange: [4, -5]
    });

    const animStyle = {
        transform: [
            {
                translateY: yVal
            },
            {
                translateX: xVal
            }
        ],
    };    

    return(
        <View style={[customTextInputStyle.container, {width:width}]}>
            <View style={customTextInputStyle.containerInput}>
                <Animated.View style={[customTextInputStyle.animatedStyle, animStyle]}>
                    <Text style={customTextInputStyle.label}>{props.text}</Text>
                </Animated.View>
                <TextInput
                    autoCapitalize={"none"}
                    style={customTextInputStyle.input}
                    value={props.value}
                    secureTextEntry={props.secureTextEntry}
                    onChangeText={(text: string) => onChangeText(text)}
                    editable={editable}
                    onFocus={onFocusHandler}
                    onBlur={onBlurHandler}
                    autoFocus={autoFocus}
                    blurOnSubmit
                />
                {props.icon && 
                <TouchableOpacity 
                    style={customTextInputStyle.icon}
                    onPress={props.onPressIcon}>
                        {props.icon}
                </TouchableOpacity>}
            </View>
            {props.messageText != "" &&
            <Text style={customTextInputStyle.textMessage}>{props.messageText}</Text>}
        </View>
    );
};

export default CustomTextInput;