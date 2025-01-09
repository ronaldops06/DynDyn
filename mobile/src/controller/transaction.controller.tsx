import Moment from 'moment';

import { Alert } from 'react-native';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalTransaction,
    existsTransactionRelationshipOperation,
    insertTransaction,
    selectAllTransactions,
    selectContAll,
    selectTransactionById,
    selectTransactionsTotals,
    updateTransaction
} from '../repository/transaction.repository';
import {deleteTransaction, getTransactions, postTransaction, putTransaction} from '../services/transactions.api';
import { loadInternalAccount } from './account.controller';
import { loadInternalOperation } from './operation.controller';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';
import {deleteOperation} from "../services/operation.api.tsx";
import {deleteInternalOperation} from "../repository/operation.repository.tsx";

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

export const loadAllTransactionsInternal = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number): Promise<I.Response> => {
    let responseTransactions = {} as I.Response;

    responseTransactions.isLogged = true;
    responseTransactions.data = await selectAllTransactions(mountDateInicio, mountDateFim, pageNumber as number);
    let totalRecords = await selectContAll(mountDateInicio, mountDateFim);
    
    responseTransactions.totalPages = Math.ceil(totalRecords / constants.pageSize);

    return responseTransactions;
}

export const loadAndPersistAll = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(mountDateInicio, mountDateFim, constants.operations.transaction);

    let params = `DataCriacaoInicio=${Moment(mountDateInicio).format('YYYY-MM-DD HH:mm:ss')}&DataCriacaoFim=${Moment(mountDateFim).format('YYYY-MM-DD HH:mm:ss')}&LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    let response = await getTransactions(params);

    if (response && !response.isLogged)
        return response;

    var transactions = response?.data ?? [];
    
    for (const item of transactions) {
        var transaction = await selectTransactionById(item.Id);
        
        item.Operation = await loadInternalOperation(item.Operation?? {} as I.Operation);
        item.Account = await loadInternalAccount(item.Account ?? {} as I.Account);
        
        if (item.DestinationAccount !== undefined && item.DestinationAccount !== null)
            item.DestinationAccount = await loadInternalAccount(item.DestinationAccount);
        
        if (item.ParentTransaction !== undefined && item.ParentTransaction !== null)
            item.ParentTransaction = await loadInternalTransaction(item.ParentTransaction ?? {} as I.Transaction);
        
        if (transaction === undefined) {
            let transaction: I.Transaction = await insertTransaction(item);
        } else {
            item.InternalId = transaction.InternalId;
            await updateTransaction(item);
        }
    }
    
    await setLastSynchronization(synchronization);
    return await loadAllTransactionsInternal(mountDateInicio, mountDateFim, pageNumber);
}

export const loadTotalsTransactions = async (mountDateInicio: Date, mountDateFim: Date): Promise<I.TransactionTotals> => {
    return await selectTransactionsTotals(mountDateInicio, mountDateFim);
}

export const createTransaction = async (transaction: I.Transaction): Promise<I.Response> => {
    let response = await postTransaction(transaction);

    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(transaction, response);

    if (transaction.Operation.InternalId === undefined)
        transaction.Operation = await loadInternalOperation(response.data.Operation);

    if (!response.isConnected) {
        transaction = await insertTransaction(transaction);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        transaction = await insertTransaction(response.data);
    }

    return response;
}

export const alterTransaction = async (transaction: I.Transaction): Promise<I.Response> => {
    let response = await putTransaction(transaction);
    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(transaction, response);

    if (transaction.Operation.InternalId === undefined)
        transaction.Operation = await loadInternalOperation(transaction.Operation);

    if (!response.isConnected) {
        transaction = await updateTransaction(transaction);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        transaction = await updateTransaction(response.data);
    }

    return response;
}

const populateInternalFields = (transaction: I.Transaction, response: I.Response) => {
    if (transaction.InternalId)
        response.data.InternalId = transaction.InternalId;
    response.data.Account.InternalId = transaction.Account.InternalId;
    
    if (transaction.DestinationAccount !== null)
        response.data.DestinationAccount.InternalId = transaction.DestinationAccount?.InternalId;
    
    response.data.Operation.InternalId = transaction.Operation.InternalId;
    
    if (transaction.ParentTransaction !== null)
        response.data.ParentTransaction.InternalId = transaction.ParentTransaction?.InternalId;
}

export const excludeTransaction = async (transactionId: number, transactionInternalId: number): Promise<I.Response> => {

    let response = await deleteTransaction(transactionId);
    if (response && !response.isLogged)
        return response;

    if (!response.isConnected) {
        await deleteInternalTransaction(transactionInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data){
        await deleteInternalTransaction(transactionInternalId);
    }

    return response;
}