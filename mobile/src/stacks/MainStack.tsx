import { createStackNavigator } from '@react-navigation/stack';
import React from 'react';

import Account from '../screens/Account';
import Category from '../screens/Category';
import CategoryCreate from '../screens/Category/create';
import Operation from '../screens/Operation';
import OperationCreate from '../screens/Operation/create';
import Dashboard from '../screens/Dashboard';
import Preload from '../screens/Preload';
import { RootStackParamList } from '../screens/RootStackParams';
import SignIn from '../screens/SignIn';
import SignUp from '../screens/SignUp';
import Transaction from '../screens/Transaction';
import TransactionCreate from '../screens/Transaction/create';

import MainTab from './MainTab';

const Stack = createStackNavigator<RootStackParamList>();

const MainStack = () => {
    return (
        <Stack.Navigator
            initialRouteName="Preload"
            screenOptions={{
                headerShown: false
            }}
        >
            <Stack.Screen name="Preload" component={Preload} />
            <Stack.Screen name="SignIn" component={SignIn} />
            <Stack.Screen name="SignUp" component={SignUp} />
            <Stack.Screen name="MainTab" component={MainTab} />
            <Stack.Screen name="Dashboard" component={Dashboard} />
            <Stack.Screen name="Account" component={Account} />
            <Stack.Screen name="Transaction" component={Transaction} />
            <Stack.Screen name="TransactionCreate" component={TransactionCreate} />
            <Stack.Screen name="Category" component={Category} />
            <Stack.Screen name="CategoryCreate" component={CategoryCreate} />
            <Stack.Screen name="Operation" component={Operation} />
            <Stack.Screen name="OperationCreate" component={OperationCreate} />
        </Stack.Navigator>
    );
}

export default MainStack;