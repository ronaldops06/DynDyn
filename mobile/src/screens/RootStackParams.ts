import * as I from '../interfaces/interfaces';

export type RootStackParamList = { 
    Preload: undefined; 
    SignIn: undefined;
    SignUp: undefined;
    Validation: { data: I.User | null } | undefined;
    RecoveryLogin: { login: string } | undefined;
    RecoveryValidation: { login: string } | undefined;
    RecoveryPassword: { login: string, verificationToken: string } | undefined;
    Home: undefined;
    UserAccount: undefined;
    ChangePassword: undefined;
    DeleteAccount: { login: string } | undefined;
    Dashboard: undefined;
    Account: undefined;
    AccountCreate: { isEditing: boolean, data: I.Portfolio | null } | undefined;
    Transaction: undefined;
    TransactionCreate: {isEditing: boolean, data: I.Transaction | null } | undefined;
    Category: { actionNavigation: string } | undefined;
    CategoryCreate: {isEditing: boolean, data: I.Category | null, sourceScreen: string | null } | undefined;
    Operation: undefined;
    OperationCreate: {isEditing: boolean, data: I.Operation | null } | undefined;
    OperationRole: undefined;
    OperationRoleCreate: {isEditing: boolean, data: I.OperationRole | null } | undefined;
    TotalizerRole: undefined;
    TotalizerRoleCreate: {isEditing: boolean, data: I.TotalizerRole | null } | undefined;
    MainTab: undefined;
};