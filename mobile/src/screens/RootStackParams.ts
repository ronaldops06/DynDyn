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
    CategoryCreate: {isEditing: boolean, data: I.Category } | undefined;
    Operation: undefined;
    OperationCreate: {isEditing: boolean, data: I.Operation } | undefined;
    MainTab: undefined;
};