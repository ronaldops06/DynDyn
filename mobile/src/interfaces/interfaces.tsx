export interface Response {
    data: any,
    status: number,
    error: string,
    success: boolean,
    totalPages: number,
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

export interface Category {
    Id: number,
    Name: string
    Type: number,
    Status: number
};

export interface Account {
    Id: number,
    Name: string,
    Status: number,
    Category: Category
};

export interface Operation {
    Id: number,
    Name: string,
    Type: number,
    Recurrent: boolean,
    Status: number,
    categoriaID: number,
    Category: Category
};

export interface Transaction {
    Id: number,
    Value: number,
    Observation: string,
    Consolidated: boolean,
    Installment: number,
    TotalInstallments: number,
    ContaID: number,
    Account: Account,
    ContaDestinoID?: number,
    DestinationAccount: Account | undefined,
    operacaoID: number,
    Operation: Operation,
    movimentoPaiID: number,
    ParentTransaction: Transaction,
    DataCriacao: Date,
    DataAlteracao: Date
};

export interface TransactionTotals {
    Credit: number,
    Debit: number,
    Tansfer: number
}