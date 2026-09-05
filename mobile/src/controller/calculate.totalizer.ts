import * as I from "../interfaces/interfaces";
import {constants} from "../constants";

export const calculateIgnored = (transactions: I.Transaction[], totalizerRole: I.TotalizerRole[]): void=> {
    try {
        for (const totalizer of totalizerRole.filter(x => x.Type === constants.totalizerType.ignore.Id)) {

            const totalizerRoles = new Set(
                totalizer.OperationRoles.map(r => r.Name)
            );

            transactions.filter(t => {
                return !t.Operation.Roles?.split(';')?.some(role => totalizerRoles.has(role));
            });
        }
    } catch (error) {
        console.log(error);
    }
}

//Somar na receita
export const calculateGroupedRevenue = (transactions: I.Transaction[], totalizerRole: I.TotalizerRole[], transactionTotals: I.TransactionTotals): void => {
    for (const totalizer of totalizerRole.filter(x => x.Type === constants.totalizerType.groupedRevenue.Id)) {

        const totalizerRoles = new Set(
            totalizer.OperationRoles.map(r => r.Name)
        );

        let result = transactions.filter(t => {
            return t.Operation.Roles?.split(';')?.some(role => totalizerRoles.has(role));
        });

        let valorDespesas = result.filter(x => x.Operation.Type === constants.operationType.expense.Id).reduce((sum, t) => sum + t.Value, 0);
        let valorReceitas = result.filter(x => x.Operation.Type === constants.operationType.revenue.Id).reduce((sum, t) => sum + t.Value, 0);
        transactionTotals.CreditTotal += valorReceitas - valorDespesas;

        const idsToRemove = new Set(result.map(x => x.Id));
        transactions = transactions.filter(x => !idsToRemove.has(x.Id));
    }
}

//Discriminar
export const calculateDiscrimineted = (transactions: I.Transaction[], totalizerRole: I.TotalizerRole[], transactionTotals: I.TransactionTotals): void => {
    for (const totalizer of totalizerRole.filter(x => x.Type === constants.totalizerType.discriminated.Id)){

        const totalizerRoles = new Set(
            totalizer.OperationRoles.map(r => r.Name)
        );

        transactions = transactions.filter(t => {
            return t.Operation.Roles?.split(';')?.some(role => totalizerRoles.has(role));
        });

        transactionTotals.CreditTotal += transactions.filter(x => x.Operation.Type === constants.operationType.revenue.Id).reduce((sum, t) => sum + t.Value, 0);
        transactionTotals.DebitTotal += transactions.filter(x => x.Operation.Type === constants.operationType.expense.Id).reduce((sum, t) => sum + t.Value, 0);
    }
}

export const calculateTotals = (transactions: I.Transaction[], totalizerRole: I.TotalizerRole[]): I.TransactionTotals => {
    let transactionTotals: I.TransactionTotals = {} as I.TransactionTotals;

    transactionTotals.CreditTotal = 0;
    transactionTotals.DebitTotal = 0;
    
    calculateIgnored(transactions, totalizerRole);
    calculateGroupedRevenue(transactions, totalizerRole, transactionTotals);
    calculateDiscrimineted(transactions, totalizerRole, transactionTotals);

    transactionTotals.Credit = transactionTotals.CreditTotal;
    transactionTotals.Debit = transactionTotals.DebitTotal;
    
    return transactionTotals;
}