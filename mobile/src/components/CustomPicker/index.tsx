import { Picker } from '@react-native-picker/picker';
import React, { useEffect, useRef } from 'react';
import { Animated, Text, View } from 'react-native';


import { customPickerStyle } from './styles';

interface PickerParams {
    text: string,
    value: number,
    setValue: any,
    data: {
        Id: number,
        Name: string
    }[]
}

const CustomPicker = (props: PickerParams) => {

    const moveText = useRef(new Animated.Value(0)).current;

    useEffect(() => {
        if (props.value !== 0) {
            moveTextTop();
        } else if (props.value === 0) {
            moveTextBottom();
        }
    }, [props.value]);

    const onChangeText = (text: string) => {
        props.setValue(text);
    };

    const onFocusHandler = () => {
        if (props.value !== 0) {
            moveTextTop();
        }
    };

    const onBlurHandler = () => {
        if (props.value === 0) {
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
        Animated.timing(moveText, {
            toValue: 0,
            duration: 200,
            useNativeDriver: true,
        }).start();
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

    return (
        <View style={customPickerStyle.container}>
            <Animated.View style={[customPickerStyle.animatedStyle, animStyle]}>
                <Text style={customPickerStyle.label}>{props.text}</Text>
            </Animated.View>
            <Picker
                style={customPickerStyle.picker}
                selectedValue={props.value}
                onValueChange={(itemValue, itemIndex) => props.setValue(itemValue)}>
                {props.data.map((item, key) => (
                    <Picker.Item key={key} label={item.Name} value={item.Id} />
                ))}
            </Picker>
        </View>
    );
}

export default CustomPicker;