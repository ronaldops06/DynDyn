import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import React from 'react';

import CustomTabBar from '../components/CustomTabBar';

import Account from '../screens/Account';
import Category from '../screens/Category';
import Home from '../screens/Home';
import Transaction from '../screens/Transaction';
import Operation from '../screens/Operation';

type TabNavigatorParamList = {
    Home: undefined,
    Account: undefined,
    Transaction: undefined,
    Category: undefined,
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
            <Tab.Screen name="Home" component={Home} />
            <Tab.Screen name="Account" component={Account} />
            <Tab.Screen name="Transaction" component={Transaction} />
            <Tab.Screen name="Category" component={Category} />
            <Tab.Screen name="Operation" component={Operation} />
        </Tab.Navigator>
    );
}

export default MainTab;