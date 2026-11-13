import React, { useEffect, useRef } from 'react';
import { Animated, Text, View } from 'react-native';
import { Picker } from '@react-native-picker/picker';

import { useTheme } from '../../contexts/ThemeContext';
import { getCustomPickerStyle } from './styles';

interface PickerProps {
    text: string;
    isMoveText?: boolean | undefined;
    value: number | string;
    setValue: any;
    required?: boolean | undefined;
    valueDefault?: number | undefined;
    width?: string | undefined;
    data: {
        Id: number | string;
        Name: string;
    }[]
}

const CustomPicker = (props: PickerProps) => {
    const { theme } = useTheme();
    const customPickerStyle = getCustomPickerStyle(theme);
    
    const {
        isMoveText = false,
        required = false,
        valueDefault = 0,
        width = '100%',
    } = props;
    
    const moveText = useRef(new Animated.Value((isMoveText) ? 0 : 1)).current;

    useEffect(() => {
        if (props.value !== valueDefault) {
            moveTextTop();
        } else if (props.value === valueDefault) {
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

    return (
        <View style={[customPickerStyle.container, {width:width}]}>
            <Animated.View style={[customPickerStyle.animatedStyle, animStyle]}>
                <Text style={customPickerStyle.label}>{props.text}</Text>
            </Animated.View>
            <Picker
                style={customPickerStyle.picker}
                selectedValue={props.value}
                onValueChange={(itemValue, itemIndex) => props.setValue(itemValue)}>
                {!required &&
                    <Picker.Item key={0} label="" value={valueDefault} />
                }
                {props.data.map((item, key) => (
                    <Picker.Item key={key} label={item.Name} value={item.Id} />
                ))}
            </Picker>
        </View>
    );
}

export default CustomPicker;