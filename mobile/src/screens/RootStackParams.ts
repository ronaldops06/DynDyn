import * as I from '../interfaces/interfaces';
import Recovery from "./SignUp/Recovery";
import RecoveryValidation from "./SignUp/Recovery/recovery.validation.tsx";

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
    Dashboard: undefined;
    Account: undefined;
    AccountCreate: { isEditing: boolean, data: I.Portfolio | null } | undefined;
    Transaction: undefined;
    TransactionCreate: {isEditing: boolean, data: I.Transaction | null } | undefined;
    Category: undefined;
    CategoryCreate: {isEditing: boolean, data: I.Category | null } | undefined;
    Operation: undefined;
    OperationCreate: {isEditing: boolean, data: I.Operation | null } | undefined;
    MainTab: undefined;
};