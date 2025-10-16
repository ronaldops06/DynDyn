import React from "react";
import {View, TouchableOpacity, Text} from 'react-native';

import HomeIcon from '../../assets/home.svg';
import AccountIcon from '../../assets/account.svg';
import CashRegisterIcon from '../../assets/cash-register.svg';
import CategoryIcon from '../../assets/category.svg';
import HistoryIcon from '../../assets/history.svg';

import { useTheme } from '../../contexts/ThemeContext';
import {getCustomTabBarStyle} from './styles';

interface CustomTabBarParms {
    state: any,
    navigation: any,
    descriptors: any,
    insets: any
}

const CustomTabBar = (props: CustomTabBarParms) => {
    const { theme } = useTheme();
    const customTabBarStyle = getCustomTabBarStyle(theme);
    
    const goTo = (screenName: string) => {
        props.navigation.navigate(screenName);
    };

    const opacityItem = (index: number) => {
        return props.state.index === index ? 1 : 0.7;
    };

    return (
        <View style={customTabBarStyle.tabArea}>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={() => goTo('Home')}>
                <HomeIcon style={{opacity: opacityItem(0)}} width="24" height="24" fill={theme.colors.quintenaryIcon}/>
                <Text style={[{opacity: opacityItem(0)}, customTabBarStyle.textItem]}>Home</Text>
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={() => goTo('Account')}>
                <AccountIcon style={{opacity: opacityItem(1)}} width="24" height="24" fill={theme.colors.quintenaryIcon}/>
                <Text style={[{opacity: opacityItem(1)}, customTabBarStyle.textItem]}>Conta</Text>
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItemCenter}
                onPress={() => goTo('Transaction')}>
                <CashRegisterIcon style={{opacity: opacityItem(2)}} width="32" height="32" fill={theme.colors.quintenaryIcon}/>
                <Text style={[{opacity: opacityItem(2)}, customTabBarStyle.textItem]}>Transação</Text>
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={() => goTo('Category')}>
                <CategoryIcon style={{opacity: opacityItem(3)}} width="24" height="24" fill={theme.colors.quintenaryIcon}/>
                <Text style={[{opacity: opacityItem(3)}, customTabBarStyle.textItem]}>Categoria</Text>
            </TouchableOpacity>
            <TouchableOpacity
                style={customTabBarStyle.tabItem}
                onPress={() => goTo('Operation')}>
                <HistoryIcon style={{opacity: opacityItem(4)}} width="24" height="24" fill={theme.colors.quintenaryIcon}/>
                <Text style={[{opacity: opacityItem(3)}, customTabBarStyle.textItem]}>Operação</Text>
            </TouchableOpacity>
        </View>
    );
}

export default CustomTabBar;