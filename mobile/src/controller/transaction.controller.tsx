import Moment from 'moment';

import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import { insertTransaction, selectAllTransactions, selectContAll, selectTransactionById, selectTransactionsTotals, updateTransaction } from '../repository/transaction.repository';
import { getTransactions } from '../services/transactions.api';
import { loadInternalAccount } from './account.controller';
import { loadInternalOperation } from './operation.controller';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';

/**
 * Método responsável por retornar a transação persistida internamente para ser utilizada como referência.
 * Se a transação ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da transação, somente assegurar a existência interna para 
 * referência nas entidades mães.
 * @async
 * @param {I.Transaction} transaction - Objeto de transação externo
 * @returns {Promise<I.Transaction>} - Promisse com o objeto da transação interno
 */
export const loadInternalTransaction = async (transaction: I.Transaction): Promise<I.Transaction> => {

    var transaction = await selectTransactionById(transaction.Id);
        
    transaction.Operation = await loadInternalOperation(transaction.Operation ?? {} as I.Operation);
    transaction.Account = await loadInternalAccount(transaction.Account ?? {} as I.Account);

    if (transaction.DestinationAccount !== undefined)
        transaction.DestinationAccount = await loadInternalAccount(transaction.DestinationAccount);

    if (transaction.ParentTransaction !== undefined)
        transaction.ParentTransaction = await loadInternalTransaction(transaction.ParentTransaction);

    let internalTransaction = await selectTransactionById(transaction.Id);

    if (internalTransaction === undefined){
        internalTransaction = await insertTransaction(transaction);
    }  

    return internalTransaction;
}

export const loadAndPersistAll = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number, navigation: any): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(mountDateInicio, mountDateFim, constants.operations.transaction);
console.log(synchronization);
    let params = `dataCriacaoInicio=${Moment(mountDateInicio, 'YYYY-MM-DD').format()}&dataCriacaoFim=${Moment(mountDateFim, 'YYYY-MM-DD').format()}&lastSyncDate=${Moment(synchronization.ExecutionDate, 'YYYY-MM-DD HH:mm:ss').format()}`;
    let responseTransactions = await getTransactions(params, navigation);

    var transactions = responseTransactions?.data ?? [];
    
    await transactions.map(async (item: I.Transaction) => {
        var transaction = await selectTransactionById(item.Id);      
                
        item.Operation = await loadInternalOperation(item.Operation?? {} as I.Operation);
        item.Account = await loadInternalAccount(item.Account ?? {} as I.Account);
        
        if (item.DestinationAccount !== undefined && item.DestinationAccount !== null)
            item.DestinationAccount = await loadInternalAccount(item.DestinationAccount);
        
        if (item.ParentTransaction !== undefined && item.ParentTransaction !== null)
            item.ParentTransaction = await loadInternalTransaction(transaction.ParentTransaction ?? {} as I.Transaction);

        if (transaction === undefined) {
            await insertTransaction(item);
        } else {
            item.InternalId = transaction.InternalId;
            await updateTransaction(item);
        }

        console.log('InternalId', transaction.InternalId);
    });

    await setLastSynchronization(synchronization);
    
    if (responseTransactions === null)
        responseTransactions = {} as I.Response;

    let dataInicio = new Date(Moment(mountDateInicio, 'YYYY-MM-DD').format());
    let dataFim = new Date(Moment(mountDateFim, 'YYYY-MM-DD').format());
    responseTransactions.data = await selectAllTransactions(dataInicio, dataFim, pageNumber as number);
    responseTransactions.totalPages = await selectContAll(dataInicio, dataFim);

    return responseTransactions;
}

export const loadTotalsTransactions = async (mountDateInicio: Date, mountDateFim: Date): Promise<I.TransactionTotals> => {
    return await selectTransactionsTotals(mountDateInicio, mountDateFim);
}