export const constants = {
    pageSizeRequest: 200,
    pageSize: 10,
    operations: {
        category: 'category',
        portfolio: 'portfolio',
        operation: 'operation',
        transaction: 'transaction',
        balance: 'balance'
    },
    categoryType: {
        account: { Id: 1, Name: 'Conta' },
        operation: { Id: 2, Name: 'Operação' }
    },
    operationType: {
        revenue: { Id: 1, Name: 'Receita' },
        expense: { Id: 2, Name: 'Despesa' },
        transfer: { Id: 3, Name: 'Transferência' }
    },
    calculateBalanceType: {
        inflow: 1,
        outflow: 2
    },
    status: {
        inactive: { Id: 0, Name: 'Inativo'},
        active: { Id: 1, Name: 'Ativo'},
    },
    acao: {
        update: 'UPDATE',
        insert: 'INSERT',
    },
    actionNavigation: {
        none: 'NONE',
        reload: 'RELOAD',
    },
    portfolioType: {
        ativo: { Id: 1, Name: 'Ativo' },
        passivo: { Id: 2, Name: 'Passivo' },
    },
    portfolioGroupType: {
        bens: 1,
        cartoesCredito: 2,
        contasBancarias: 3,
        empresas: 4,
        emprestimos: 5,
        financiamentos: 6,
        imoveis: 7,
        investimentos: 8,
        veiculos: 9
    }
}