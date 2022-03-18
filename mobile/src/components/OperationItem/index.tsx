import React from 'react';
import { View, Text } from 'react-native';

import * as I from '../../interfaces/interfaces';

import { operationItemStyle } from './styles';

interface OperationItemParms {
    data: I.Operacao,
    onPress: any
}

const OperationItem = (props: OperationItemParms) => {

    return(
        <View 
            style={operationItemStyle.card}
            onTouchEndCapture={() => props.onPress(props.data)}>
            <View style={operationItemStyle.rowHeader}>
                <Text style={operationItemStyle.textOperationName}>{props.data.nome}</Text>
            </View>
            <View style={operationItemStyle.rowInfo}>
                <Text style={operationItemStyle.textCategoriaName} >
                    {props.data.categoria.nome}
                </Text>
            </View>
        </View>
    );
}

export default OperationItem;