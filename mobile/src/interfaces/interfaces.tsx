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
    token: string
};

export interface Categoria {
    id: number,
    nome: string
};

export interface Conta {
    id: number,
    nome: string
};

export interface Operacao {
    id: number,
    nome: string,
    tipo: number,
    recorrente: number,
    status: number,
    categoriaID: number,
    categoria: Categoria
};

export interface Transaction {
    id: number,
    valor: number,
    observacao: string,
    consolidado: number,
    parcela: number,
    totalParcelas: number,
    contaID: number,
    conta: Conta,
    contaDestinoID?: number,
    contaDestino: Conta,
    operacaoID: number,
    operacao: Operacao,
    movimentoPaiID: number,
    movimentoPai: Transaction,
    dataCriacao: Date,
    dataAlteracao: Date
};

export interface TransactionTotals {
    credito: number,
    debito: number,
    transferencia: number
}