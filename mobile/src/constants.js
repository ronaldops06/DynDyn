export const constants = {
    pageSizeRequest: 20,
    pageSize: 150,
    operations: {
        attribute: 'attribute',
        category: 'category',
        portfolio: 'portfolio',
        operation: 'operation',
        transaction: 'transaction',
        balance: 'balance',
        trash: 'trash',
        operationRole: 'operationRole',
        totalizerRole: 'totalizerRole',
        destinationPortfolio: 'destinationPortfolio', 
    },
    categoryType: {
        account: { Id: 1, Name: 'Conta' },
        operation: { Id: 2, Name: 'Operação' }
    },
    attributeDataType: {
        text: { Id: 1, Name: 'Text' },
        number: { Id: 2, Name: 'Number' },
        date: { Id: 3, Name: 'Date' },
        boolean: { Id: 4, Name: 'Boolean' },
        listOptions: { Id: 5, Name: 'ListOptions' }
    },
    operationType: {
        revenue: { Id: 1, Name: 'Receita' },
        expense: { Id: 2, Name: 'Despesa' },
        transfer: { Id: 3, Name: 'Transferência' }
    },
    totalizerType: {
        ignore: { Id: 1, Name: 'Ignorar', Order: 1},
        discriminated: { Id: 2, Name: 'Discriminar', Order: 4 },
        groupedRevenue: { Id: 3, Name: 'Agrupar nas Receitas', Order: 2 },
        groupedExpense: { Id: 4, Name: 'Agrupar nas Despesas', Order: 3 },
    },
    totalizerCode: {
        transactionRevenue: { Id: "TRA_REV", Name: 'Transação - Receitas' },
        transactionExpense: { Id: "TRA_EXP", Name: 'Transação - Despesas' },
        homeRevenue: { Id: "HOM_REV", Name: 'Home - Receitas' },
        homeExpense: { Id: "HOM_EXP", Name: 'Home - Despesas' },
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
        delete: 'DELETE'
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
        ativo: {
            bens: {Id: 1, Name: 'Bens', IsVisible: false, Icon: 'home', Type: 1},
            contasBancarias: {Id: 2, Name: 'Contas Bancárias', IsVisible: false, Icon: 'account', Type: 1},
            empresas: {Id: 3, Name: 'Empresas', IsVisible: false, Icon: 'home', Type: 1},
            emprestimos: {Id: 4, Name: 'Empréstimos', IsVisible: false, Icon: 'home', Type: 1},
            imoveis: {Id: 5, Name: 'Imóveis', IsVisible: false, Icon: 'home', Type: 1},
            investimentos: {Id: 6, Name: 'Investimentos', IsVisible: false, Icon: 'home', Type: 1},
            veiculos: {Id: 7, Name: 'Veículos', IsVisible: false, Icon: 'home', Type: 1},
        },
        passivo: {
            cartoesCredito: {Id: 51, Name: 'Cartões de Crédito', IsVisible: false, Icon: 'home', Type: 2},
            contasBancarias: {Id: 52, Name: 'Contas Bancárias', IsVisible: false, Icon: 'account', Type: 2},
            emprestimos: {Id: 53, Name: 'Empréstimos', IsVisible: false, Icon: 'home', Type: 2},
            financiamentos: {Id: 54, Name: 'Financiamentos', IsVisible: false, Icon: 'home', Type: 2},
            imoveis: {Id: 55, Name: 'Imóveis', IsVisible: false, Icon: 'home', Type: 2},
            veiculos: {Id: 56, Name: 'Veículos', IsVisible: false, Icon: 'home', Type: 2},
        }
    },
    operators: [
        {Id: 1, Name: 'Igual', Key: '='},
        {Id: 2, Name: 'Maior', Key: '>'},
        {Id: 3, Name: 'Menor', Key: '<'},
        {Id: 4, Name: 'Maior ou Igual', Key: '>='},
        {Id: 5, Name: 'Menor ou Igual', Key: '<='},
        {Id: 6, Name: 'Diferente', Key: '!='}
    ],
    colors: {
        primaryBaseColor: '#6E8BB8',
        secondaryBaseColor: '#F1F1F1',
        tertiaryBaseColor: '#D4DBE6',
        dangerBaseColor: '#F06868',
        primaryBorderColor: '#DCDCDC',
        secondaryBorderColor: '#D4DBE6',
        primaryTextColor: '#4b688c',
        secondaryTextColor: '#808080',//'#A4BCE3',
        tertiaryTextColor: '#F1F1F1',
        quintenaryTextColor: '#99ABC9',
        dangerTextColor: '#F06868',
        primaryMonetaryColor: '#3C66F0',
        secondaryMonetaryColor: '#F06868',
        tertiaryMonetaryColor: '#4CA346',
    }
}