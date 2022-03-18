import * as I from '../interfaces/interfaces';

export type RootStackParamList = { 
    Preload: undefined; 
    SignIn: undefined;
    SignUp: undefined;
    Dashboard: undefined;
    Account: undefined;
    Transaction: undefined;
    TransactionCreate: {isEditing: boolean, data: I.Transaction } | undefined;
    Category: undefined;
    MainTab: undefined;
};