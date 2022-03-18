import React from 'react';
import { View, Text } from 'react-native';
import { StackNavigationProp } from '@react-navigation/stack';
import { useNavigation } from '@react-navigation/core';

import { RootStackParamList } from '../RootStackPrams';
import api from '../../services/api';

import { style } from '../../styles/styles';
import { dashboardStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Dashboard'>;

const Dashboard = () => {
    const navigation = useNavigation<homeScreenProp>();

    return(
        <View>
            <Text>Dashboard</Text>
        </View>
    );
}

export default Dashboard;