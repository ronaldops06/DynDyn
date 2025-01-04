import React from "react";
import { View, TouchableOpacity } from 'react-native';

import  DashboardIcon from '../../assets/dashboard.svg';
import  AccountIcon from '../../assets/account.svg';
import  CashRegisterIcon from '../../assets/cash-register.svg';
import  CategoryIcon from '../../assets/category.svg';

import { customTabBarStyle } from './styles';

interface CustomTabBarParms {
    state: any,
    navigation: any,
    descriptors: any,
    insets: any
};

const CustomTabBar = (props: CustomTabBarParms) => {
    const goTo = (screenName: string) => {
        props.navigation.navigate(screenName);
    };

    const opacityItem = (index: number) => {
        return props.state.index === index ? 1 : 0.7;
    };

    return(
        <View style={customTabBarStyle.tabArea}>
            <TouchableOpacity 
                style={customTabBarStyle.tabItem}
                onPress={ ()=> goTo('Dashboard') }>
                <DashboardIcon style={{ opacity: opacityItem(0) }} width="24" height="24" fill="#6E8BB8" />
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={ ()=> goTo('Account') }>
                <AccountIcon style={{ opacity: opacityItem(1) }} width="24" height="24" fill="#6E8BB8" />
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItemCenter}
                onPress={ ()=> goTo('Transaction') }>
                <CashRegisterIcon style={{ opacity: opacityItem(2) }} width="32" height="32" fill="#6E8BB8" />
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={ ()=> goTo('Category') }>
                <CategoryIcon style={{ opacity: opacityItem(3) }} width="24" height="24" fill="#6E8BB8" />
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={ ()=> goTo('Operation') }>
                <CategoryIcon style={{ opacity: opacityItem(4) }} width="24" height="24" fill="#6E8BB8" />
            </TouchableOpacity>
        </View>
    );
}

export default CustomTabBar;