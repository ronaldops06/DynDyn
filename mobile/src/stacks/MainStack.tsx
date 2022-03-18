import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';

import { RootStackParamList } from '../screens/RootStackPrams';
import Preload from '../screens/Preload';
import SignIn from '../screens/SignIn';
import SignUp from '../screens/SignUp';
import Dashboard from '../screens/Dashboard';
import Account from '../screens/Account';
import Transaction from '../screens/Transaction';
import TransactionCreate from '../screens/Transaction/create';
import Category from '../screens/Category';

import MainTab from './MainTab';

const Stack = createStackNavigator<RootStackParamList>();

const MainStack = () => {
    return(
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
            <Stack.Screen name="TransactionCreate" component={TransactionCreate}/>
            <Stack.Screen name="Category" component={Category} />
        </Stack.Navigator>
    );
}

export default MainStack;