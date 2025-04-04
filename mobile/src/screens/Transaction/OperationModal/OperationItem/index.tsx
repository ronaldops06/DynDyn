import React from 'react';
import { Text, View } from 'react-native';

import * as I from '../../../../interfaces/interfaces';

import { operationItemStyle } from './styles';

interface OperationItemProps {
    data: I.Operation,
    onPress: any
}

const OperationItem = (props: OperationItemProps) => {

    return (
        <View
            style={operationItemStyle.card}
            onTouchEndCapture={() => props.onPress(props.data)}>
            <View style={operationItemStyle.rowHeader}>
                <Text style={operationItemStyle.textOperationName}>{props.data.Name}</Text>
            </View>
            <View style={operationItemStyle.rowInfo}>
                <Text style={operationItemStyle.textCategoriaName} >
                    {props.data.Category.Name}
                </Text>
            </View>
        </View>
    );
}

export default OperationItem;