import React from 'react';
import { Text, View } from 'react-native';

import * as I from '../../interfaces/interfaces';

import { operationItemStyle } from './styles';

interface OperationItemParms {
    data: I.Operation,
    onPress: any
}

const OperationItem = (props: OperationItemParms) => {

    return (
        <View
            style={operationItemStyle.card}
            onTouchEndCapture={() => props.onPress(props.data)}>
            <View style={operationItemStyle.rowHeader}>
                <Text style={operationItemStyle.textOperationName}>{props.data.name}</Text>
            </View>
            <View style={operationItemStyle.rowInfo}>
                <Text style={operationItemStyle.textCategoriaName} >
                    {props.data.category.name}
                </Text>
            </View>
        </View>
    );
}

export default OperationItem;