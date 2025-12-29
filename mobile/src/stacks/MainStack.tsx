import React from 'react';

import Account from '../screens/Account';
import AccountCreate from "../screens/Account/create.tsx";
import Category from '../screens/Category';
import CategoryCreate from '../screens/Category/create';
import Operation from '../screens/Operation';
import OperationCreate from '../screens/Operation/create';
import Home from '../screens/Home';
import UserAccount from "../screens/Home/UserAccount";
import ChangePassword from "../screens/Home/UserAccount/ChangePassword";
import DeleteAccount from "../screens/Home/UserAccount/DeleteAccount";
import Dashboard from '../screens/Dashboard';
import Preload from '../screens/Preload';
import {RootStackParamList} from '../screens/RootStackParams';
import SignIn from '../screens/SignIn';
import SignUp from '../screens/SignUp';
import Validation from "../screens/SignUp/validation.tsx";
import RecoveryLogin from "../screens/SignUp/Recovery/recovery.login.tsx"
import RecoveryValidation from "../screens/SignUp/Recovery/recovery.validation.tsx"
import RecoveryPassword from "../screens/SignUp/Recovery/recovery.password.tsx"
import Transaction from '../screens/Transaction';
import TransactionCreate from '../screens/Transaction/create';
import {createNativeStackNavigator} from '@react-navigation/native-stack';

import MainTab from './MainTab';
import {constants} from "../constants";

const Stack = createNativeStackNavigator<RootStackParamList>();

export const HomeStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="Home" component={Home} />
    </Stack.Navigator>
);

export const AccountStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="AccountHome" component={Account} initialParams={{ actionNavigation: constants.actionNavigation.reload }}/>
        <Stack.Screen name="AccountCreate" component={AccountCreate}/>
    </Stack.Navigator>
);

export const TransactionStack = () => (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="TransactionHome" component={Transaction}/>
        <Stack.Screen name="TransactionCreate" component={TransactionCreate}/>
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
            <Stack.Screen name="Home" component={Home}/>
            <Stack.Screen name="UserAccount" component={UserAccount}/>
            <Stack.Screen name="ChangePassword" component={ChangePassword}/>
            <Stack.Screen name="DeleteAccount" component={DeleteAccount}/>
            <Stack.Screen name="Dashboard" component={Dashboard}/>
            <Stack.Screen name="MainTab" component={MainTab}/>
        </Stack.Navigator>
    );
}

export default MainStack;