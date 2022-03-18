import React from 'react';
import { View } from 'react-native';

import { buttonSelectStyle } from './styles';

interface ButtonSelectParams {
    children: any;
}

const ButtonSelectBar = (props: ButtonSelectParams) => {
    return (
        <View style={buttonSelectStyle.areaButtonSelect} >
            {props.children}
        </View>
    );
}

export default ButtonSelectBar;