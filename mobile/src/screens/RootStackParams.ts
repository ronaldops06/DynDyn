import * as I from '../interfaces/interfaces';

export type RootStackParamList = { 
    Preload: undefined; 
    SignIn: undefined;
    SignUp: undefined;
    Dashboard: undefined;
    Account: undefined;
    AccountCreate: { isEditing: boolean, data: I.Account | null, onGoBack?: (actionNavigation: string) => void | undefined } | undefined;
    Transaction: undefined;
    TransactionCreate: {isEditing: boolean, data: I.Transaction | null, onGoBack?: (actionNavigation: string) => void | undefined } | undefined;
    Category: undefined;
    CategoryCreate: {isEditing: boolean, data: I.Category | null, onGoBack?: (actionNavigation: string) => void | undefined } | undefined;
    Operation: undefined;
    OperationCreate: {isEditing: boolean, data: I.Operation | null, onGoBack?: (actionNavigation: string) => void | undefined } | undefined;
    MainTab: undefined;
};