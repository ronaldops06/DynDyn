import React, { useEffect, useRef } from "react";
import { View, TextInput, Text, Animated, TouchableOpacity } from "react-native";

import { customTextInputStyle } from './styles';

interface CustomTextInputParams {
    text: string;
    isMoveText?: boolean;
    value: string;
    setValue: any;
    messageText?: string;
    secureTextEntry?: boolean;
    icon?: any;
    onPressIcon?: any;
    width?: string;
};

const CustomTextInput = (props: CustomTextInputParams) => {
    const moveText = useRef(new Animated.Value((props.isMoveText) ? 0 : 1)).current;

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
        if (props.isMoveText) {
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
        <View style={[customTextInputStyle.container, {width:props.width}]}>
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
                    editable={true}
                    onFocus={onFocusHandler}
                    onBlur={onBlurHandler}
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

CustomTextInput.defaultProps = {
    isMoveText: true,
    width: "90%"
}

export default CustomTextInput;