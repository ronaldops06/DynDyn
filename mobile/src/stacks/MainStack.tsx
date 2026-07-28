import React from 'react';

import Account from '../screens/Account';
import AccountCreate from "../screens/Account/Register";
import Category from '../screens/Category';
import CategoryCreate from '../screens/Category/Register';
import Attribute from '../screens/Attribute';
import AttributeCreate from '../screens/Attribute/Register';
import Operation from '../screens/Operation';
import OperationCreate from '../screens/Operation/Register';
import Home from '../screens/Home';
import UserAccount from "../screens/Home/UserAccount";
import ChangePassword from "../screens/Home/UserAccount/ChangePassword";
import DeleteAccount from "../screens/Home/UserAccount/DeleteAccount";
import Dashboard from '../screens/Dashboard';
import Portfolio from '../screens/Portfolio';
import PortfolioRegister from "../screens/Portfolio/Register";
import PortfolioView from "../screens/Portfolio/View";
import Preload from '../screens/Preload';

import SignIn from '../screens/SignIn';
import SignUp from '../screens/SignUp';
import Validation from "../screens/SignUp/validation.tsx";
import RecoveryLogin from "../screens/SignUp/Recovery/recovery.login.tsx"
import RecoveryValidation from "../screens/SignUp/Recovery/recovery.validation.tsx"
import RecoveryPassword from "../screens/SignUp/Recovery/recovery.password.tsx"
import Transaction from '../screens/Transaction';
import TransactionCreate from '../screens/Transaction/Register';
import TotalizerRole from "../screens/Home/TotalizerRole";
import TotalizerRoleCreate from "../screens/Home/TotalizerRole/Register";
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import {RootStackParamList} from '../screens/RootStackParams';

import MainTab from './MainTab';
import {constants} from "../constants";

const Stack = createNativeStackNavigator<RootStackParamList>();

export const HomeStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="HomeHome" component={Home} />
        <Stack.Screen name="TransactionCreate" component={TransactionCreate}/>
    </Stack.Navigator>
);

export const AccountStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="AccountHome" component={Account} initialParams={{ actionNavigation: constants.actionNavigation.reload }}/>
        <Stack.Screen name="AccountCreate" component={AccountCreate}/>
        <Stack.Screen name="CategoryCreate" component={CategoryCreate} />
    </Stack.Navigator>
);

export const TransactionStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="TransactionHome" component={Transaction}/>
        <Stack.Screen name="TransactionCreate" component={TransactionCreate}/>
        <Stack.Screen name="CategoryCreate" component={CategoryCreate} />
        <Stack.Screen name="AccountCreate" component={AccountCreate}/>
    </Stack.Navigator>
);

export const CategoryStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="CategoryHome" component={Category} />
        <Stack.Screen name="CategoryCreate" component={CategoryCreate} />
    </Stack.Navigator>
);

export const OperationStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="OperationHome" component={Operation}  initialParams={{ actionNavigation: constants.actionNavigation.reload }}/>
        <Stack.Screen name="OperationCreate" component={OperationCreate}/>
        <Stack.Screen name="CategoryCreate" component={CategoryCreate} />
    </Stack.Navigator>
);

const MainStack = () => {
    return (
        <Stack.Navigator
            initialRouteName="Preload"
            screenOptions={{
                headerShown: false
            }}
        >
            <Stack.Screen name="Preload" component={Preload}/>
            <Stack.Screen name="SignIn" component={SignIn}/>
            <Stack.Screen name="SignUp" component={SignUp}/>
            <Stack.Screen name="Validation" component={Validation}/>
            <Stack.Screen name="RecoveryLogin" component={RecoveryLogin}/>
            <Stack.Screen name="RecoveryValidation" component={RecoveryValidation}/>
            <Stack.Screen name="RecoveryPassword" component={RecoveryPassword}/>
            
            <Stack.Screen name="UserAccount" component={UserAccount}/>
            <Stack.Screen name="ChangePassword" component={ChangePassword}/>
            <Stack.Screen name="DeleteAccount" component={DeleteAccount}/>
            <Stack.Screen name="Dashboard" component={Dashboard}/>
            <Stack.Screen name="Portfolio" component={Portfolio}/>
            <Stack.Screen name="PortfolioRegister" component={PortfolioRegister}/>
            <Stack.Screen name="PortfolioView" component={PortfolioView}/>
            <Stack.Screen name="TotalizerRole" component={TotalizerRole}/>
            <Stack.Screen name="TotalizerRoleCreate" component={TotalizerRoleCreate}/>
            <Stack.Screen name="Attribute" component={Attribute}/>
            <Stack.Screen name="AttributeCreate" component={AttributeCreate}/>
            <Stack.Screen name="MainTab" component={MainTab}/>
        </Stack.Navigator>
    );
}

export default MainStack;