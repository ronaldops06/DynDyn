export const constants = {
    pageSize: 10,
    operations: {
        category: 'category',
        account: 'account',
        operation: 'operation',
        transaction: 'transaction'
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
    status: {
        inactive: { Id: 0, Name: 'Inativo'},
        active: { Id: 1, Name: 'Ativo'},
    }
}