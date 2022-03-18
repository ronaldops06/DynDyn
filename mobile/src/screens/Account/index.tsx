import React from 'react';
import { View, Text } from 'react-native';
import { StackNavigationProp } from '@react-navigation/stack';
import { useNavigation } from '@react-navigation/core';

import { RootStackParamList } from '../RootStackPrams';
import api from '../../services/api';

import { style } from '../../styles/styles';
import { accountStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Account'>;

const Account = () => {
    const navigation = useNavigation<homeScreenProp>();

    return(
        <View>
            <Text>Account</Text>
        </View>
    );
}

export default Account;