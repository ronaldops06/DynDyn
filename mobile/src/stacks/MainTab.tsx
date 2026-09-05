import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import React from 'react';

import CustomTabBar from '../components/CustomTabBar';
import {
    AccountStack,
    CategoryStack,
    HomeStack,
    OperationStack,
    TransactionStack
} from './MainStack'
import {hideTabOnScreens} from "./navigation.utils";

type TabNavigatorParamList = {
    Home: undefined,
    Account: undefined,
    Transaction: undefined,
    Category: { actionNavigation: string } | undefined,
    Operation: undefined
}

const Tab = createBottomTabNavigator<TabNavigatorParamList>();

const MainTab = () => {
    return (
        <Tab.Navigator
            screenOptions={{
                headerShown: false
            }}
            tabBar={props => <CustomTabBar {...props} />}
        >
            <Tab.Screen name="Home" component={HomeStack} options={hideTabOnScreens(['TransactionCreate'])}/>
            <Tab.Screen name="Account" component={AccountStack} options={hideTabOnScreens(['AccountCreate', 'CategoryCreate'])}/>
            <Tab.Screen 
                name="Transaction" 
                component={TransactionStack} 
                options={hideTabOnScreens(['TransactionCreate','CategoryCreate','AccountCreate'])}
            />
            <Tab.Screen name="Category" component={CategoryStack} options={hideTabOnScreens(['CategoryCreate'])}/>
            <Tab.Screen name="Operation" component={OperationStack} options={hideTabOnScreens(['OperationCreate', 'CategoryCreate'])}/>
        </Tab.Navigator>
    );
}

export default MainTab;