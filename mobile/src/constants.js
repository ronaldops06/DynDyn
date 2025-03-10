export const constants = {
    pageSizeRequest: 200,
    pageSize: 10,
    operations: {
        category: 'category',
        account: 'account',
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
    }
}