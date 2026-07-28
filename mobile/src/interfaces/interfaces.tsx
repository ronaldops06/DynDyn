import {Operator} from "../utils.ts";

export interface Response {
    data: any,
    status: number,
    error: string,
    success: boolean,
    totalPages: number,
    isConnected: boolean,
    isLogged: boolean,
}

export interface Login {
    Login: string,
    Password: string
}

export interface User {
    Id: number,
    Login: string,
    Name: string,
    Password: string,
    AccessToken: string
}

export interface ChangePasswordUser {
    Login: string,
    Password: string,
    NewPassword: string
}

export interface VerificationUser {
    Login: string,
    VerificationCode: number
}

export interface PasswordRecreation {
    Login: string,
    Password: string,
    VerificationToken: string
}

export interface NotificationOperation {
    Operation: string,
    Reference: string,
    Id: number
}

export interface Device {
    NotificationToken: string,
    PhisicalDeviceId: string
}

export interface Trash {
    Reference: string,
    ReferenceId: number
}

export interface Synchronization {
    InternalId: number | null,
    Operation: string,
    ExecutionDate: Date,
    StartCreationDate: Date | null,
    EndCreationDate: Date | null
}

export interface Category {
    InternalId: number,
    Id: number,
    Name: string
    Type: number,
    Status: number,
    DataCriacao: Date | null,
    DataAlteracao: Date | null
}

export interface AttributeOption {
    InternalId: number,
    Id: number,
    Label: string,
    IsDefault: number,
    Status: number,
    tempId?: string,
    DataCriacao: Date | null,
    DataAlteracao: Date | null
}

export interface Attribute {
    InternalId: number,
    Id: number,
    Name: string,
    Description?: string,
    DataType: number,
    Status: number,
    Options?: AttributeOption[],
    DataCriacao: Date | null,
    DataAlteracao: Date | null
}

export interface AttributeFilter {
    Search: string,
    Situation: number
}

export interface CategoryFilter {
    Search: string,
    Situation: number
}

export interface Portfolio {
    InternalId: number,
    Id: number,
    Name: string,
    Type: number,
    Group: number,
    Status: number,
    Category: Category,
    ParentPortfolio: Portfolio | null,
    DataCriacao: Date,
    DataAlteracao: Date,
    BalanceTotals: BalanceTotals | null
}

export interface PortfolioFilter {
    Search: string,
    Situation: number,
    CategoryId: number,
    ParentPortfolioId: number,
    ValueFilter: ValueFilter
}

export interface Operation {
    InternalId: number,
    Id: number,
    Name: string,
    Type: number,
    Recurrent: boolean,
    Salary: boolean,
    Status: number,
    Category: Category,
    OperationRoles: OperationRole[] | null,
    Roles: string | null,
    DataCriacao: Date,
    DataAlteracao: Date
}

export interface OperationFilter {
    Search: string,
    Situation: number,
    CategoryId: number,
    Recurrent: number,
    Salary: number,
}

export interface OperationRole {
    InternalId: number,
    Id: number,
    Name: string,
    DataCriacao: Date,
    DataAlteracao: Date,
}

export interface OperationRoleFilter {
    Search: string
}

export interface TotalizerRole {
    InternalId: number,
    Id: number,
    Code: string,
    Type: number,
    OperationRoles: OperationRole[] | null,
    DataCriacao: Date,
    DataAlteracao: Date
}

export interface Transaction {
    InternalId: number,
    Id: number,
    Value: number,
    Observation: string,
    Consolidated: boolean,
    Installment: number,
    TotalInstallments: number,
    Portfolio: Portfolio,
    DestinationPortfolio: Portfolio | null,
    Operation: Operation,
    ParentTransaction: Transaction | null,
    DataCriacao: Date,
    DataAlteracao: Date,
    IsSelectedItem: boolean | null
}

export interface TransactionView {
    InternalId: number,
    Id: number,
    DataCriacao: Date,
    Installment: number,
    TotalInstallments: number,
    Value: number,
    Consolidated: boolean,
    OperationType: number,
    OperationName: string,
    PortfolioName: string,
    DestinationPortfolioName: string | null
}

export interface TransactionsGroup {
    date: string,
    transactions: Transaction[]
}

export interface TransactionTotals {
    Credit: number,
    Debit: number,
    Transfer: number,
    DebitSalary: number,
    CreditSalary: number,
    CreditTotal: number,
    DebitTotal: number
}

export interface TransactionFilter {
    Search: string,
    Situation: number,
    CategoryId: number,
    OperationId: number,
    PortfolioId: number,
    ValueFilter: ValueFilter
}

export interface Balance {
    InternalId: number,
    Id: number,
    Value: number,
    Valuation: number,
    Dividends: number,
    Income: number,
    PercentageValuation: number,
    PercentageIncome: number,
    Credit: number,
    Debit: number,
    SalaryCredit: number,
    SalaryDebit: number,
    Outflow: number,
    Inflow: number,
    Month: number,
    Year: number,
    Portfolio: Portfolio,
    DataCriacao: Date,
    DataAlteracao: Date
}

export interface BalanceTotals {
    Value: number
}

export interface CalculateBalance {
    Value: number,
    Portfolio: Portfolio,
    Month: number,
    Year: number,
    CalculateBalanceType: number,
    OperationType: number,
    Salary: boolean
}

export interface DashboardItem {
    Label: string;
    Value: number;
}

export interface ValueFilter {
    Operator: Operator;
    Value: number;
}

export interface SymbolString {
    Id: string,
    Name: string,
}

export interface SymbolStringView {
    Id: string,
    Name: string,
    IsVisible: boolean
}

export interface SymbolNumber {
    Id: number,
    Name: string,
}