export interface Response {
    data: any,
    status: number,
    error: string,
    success: boolean,
    totalPages: number,
    isConnected: boolean,
    isLogged: boolean,
};

export interface Login {
    Login: string,
    Password: string
};

export interface User {
    Id: number,
    Login: string,
    Name: string,
    Password: string,
    AccessToken: string
};

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
};

export interface Account {
    InternalId: number,
    Id: number,
    Name: string,
    Status: number,
    Category: Category,
    ParentAccount: Account | null,
    DataCriacao: Date,
    DataAlteracao: Date
};

export interface Operation {
    InternalId: number,
    Id: number,
    Name: string,
    Type: number,
    Recurrent: boolean,
    Salary: boolean,
    Status: number,
    Category: Category,
    DataCriacao: Date,
    DataAlteracao: Date
};

export interface Transaction {
    InternalId: number,
    Id: number,
    Value: number,
    Observation: string,
    Consolidated: boolean,
    Installment: number,
    TotalInstallments: number,
    Account: Account,
    DestinationAccount: Account | null,
    Operation: Operation,
    ParentTransaction: Transaction | null,
    DataCriacao: Date,
    DataAlteracao: Date
};

export interface TransactionTotals {
    Credit: number,
    Debit: number,
    Tansfer: number
}