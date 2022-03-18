import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import CustomTabBar from '../components/CustomTabBar';

import Dashboard from '../screens/Dashboard';
import Account from '../screens/Account';
import Transaction from '../screens/Transaction';
import Category from '../screens/Category';

type TabNavigatorParamList = {
    Dashboard: undefined,
    Account: undefined,
    Transaction: undefined,
    Category: undefined
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
            <Tab.Screen name="Dashboard" component={Dashboard} />
            <Tab.Screen name="Account" component={Account} />
            <Tab.Screen name="Transaction" component={Transaction}/>
            <Tab.Screen name="Category" component={Category} />
        </Tab.Navigator>
    );
}

export default MainTab;