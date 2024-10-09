export interface Response {
    data: any,
    status: number,
    error: string,
    success: boolean,
    totalPages: number,
};

export interface Login {
    login: string,
    password: string
};

export interface User {
    id: number,
    login: string,
    name: string,
    password: string,
    accessToken: string
};

export interface Category {
    id: number,
    name: string
};

export interface Account {
    id: number,
    name: string
};

export interface Operation {
    id: number,
    name: string,
    type: number,
    recurrent: number,
    status: number,
    categoriaID: number,
    category: Category
};

export interface Transaction {
    id: number,
    value: number,
    observation: string,
    consolidated: number,
    installment: number,
    totalInstallments: number,
    contaID: number,
    account: Account,
    contaDestinoID?: number,
    destinationAccount: Account,
    operacaoID: number,
    operation: Operation,
    movimentoPaiID: number,
    parentTransaction: Transaction,
    dataCriacao: Date,
    dataAlteracao: Date
};

export interface TransactionTotals {
    credit: number,
    debit: number,
    transfer: number
}