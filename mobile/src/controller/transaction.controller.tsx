import Moment from 'moment';

import { Alert } from 'react-native';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import { insertTransaction, selectAllTransactions, selectContAll, selectTransactionById, selectTransactionsTotals, updateTransaction } from '../repository/transaction.repository';
import { getTransactions, postTransaction } from '../services/transactions.api';
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

    transaction = await selectTransactionById(transaction.Id) ?? transaction;
        
    transaction.Operation = await loadInternalOperation(transaction.Operation ?? {} as I.Operation);
    transaction.Account = await loadInternalAccount(transaction.Account ?? {} as I.Account);

    if (transaction.DestinationAccount !== null)
        transaction.DestinationAccount = await loadInternalAccount(transaction.DestinationAccount);

    if (transaction.ParentTransaction !== null)
        transaction.ParentTransaction = await loadInternalTransaction(transaction.ParentTransaction);

    let internalTransaction = await selectTransactionById(transaction.Id);

    if (internalTransaction === undefined){
        internalTransaction = await insertTransaction(transaction);
    }  

    return internalTransaction;
}

export const loadInternalAll = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number): Promise<I.Response> => {
    let responseTransactions = {} as I.Response;
    
    responseTransactions.data = await selectAllTransactions(mountDateInicio, mountDateFim, pageNumber as number);
    responseTransactions.totalPages = await selectContAll(mountDateInicio, mountDateFim);

    return responseTransactions;
}

export const loadAndPersistAll = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number, navigation: any): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(mountDateInicio, mountDateFim, constants.operations.transaction);

    let params = `DataCriacaoInicio=${Moment(mountDateInicio).format('YYYY-MM-DD HH:mm:ss')}&DataCriacaoFim=${Moment(mountDateFim).format('YYYY-MM-DD HH:mm:ss')}&LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    let responseTransactions = await getTransactions(params, navigation);

    var transactions = responseTransactions?.data ?? [];
    
    var promises = transactions.map(async (item: I.Transaction) => {
        
        var transaction = await selectTransactionById(item.Id);
        
        item.Operation = await loadInternalOperation(item.Operation?? {} as I.Operation);
        item.Account = await loadInternalAccount(item.Account ?? {} as I.Account);
        
        if (item.DestinationAccount !== undefined && item.DestinationAccount !== null)
            item.DestinationAccount = await loadInternalAccount(item.DestinationAccount);
        
        if (item.ParentTransaction !== undefined && item.ParentTransaction !== null)
            item.ParentTransaction = await loadInternalTransaction(item.ParentTransaction ?? {} as I.Transaction);
        
        if (transaction === undefined) {
            await insertTransaction(item);
        } else {
            item.InternalId = transaction.InternalId;
            await updateTransaction(item);
        }
    });
    
    await Promise.all(promises);
    
    await setLastSynchronization(synchronization);
    
    return loadInternalAll(mountDateInicio, mountDateFim, pageNumber);
}

export const loadTotalsTransactions = async (mountDateInicio: Date, mountDateFim: Date): Promise<I.TransactionTotals> => {
    return await selectTransactionsTotals(mountDateInicio, mountDateFim);
}

export const createTransaction = async (transaction: I.Transaction, navigation: any): Promise<I.Transaction> => {
    let response = await postTransaction(transaction, navigation);

    if (!response.isConnected) {
        populateInternalFields(transaction, response);
        
        transaction = await insertTransaction(transaction);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feito uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
        navigation.goBack();
    } else if (response.data !== null){
        populateInternalFields(transaction, response);
        
        transaction = await insertTransaction(response.data);
        navigation.goBack();
    }

    return transaction;
}

const populateInternalFields = (transaction: I.Transaction, response: I.Response) => {
    response.data.Account.InternalId = transaction.Account.InternalId;
    response.data.DestinationAccount.InternalId = transaction.DestinationAccount?.InternalId;
    response.data.Operation.InternalId = transaction.Operation.InternalId;
    response.data.ParentTransaction.InternalId = transaction.ParentTransaction?.InternalId;
}