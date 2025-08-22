import Moment from 'moment';

import {Alert} from 'react-native';
import {constants} from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalTransaction,
    insertTransaction,
    selectAllTransactions,
    selectContAll,
    selectTransactionById,
    selectTransactionsTotals,
    updateTransaction
} from '../repository/transaction.repository';
import {
    deleteTransaction,
    getTransactions,
    postRecurringTransactions,
    postTransaction,
    putTransaction
} from '../services/transactions.api';
import {loadInternalPortfolio} from './portfolio.controller';
import {loadInternalOperation} from './operation.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from './synchronization.controller';
import {calculateBalanceByTransaction, calculateBalanceByTransactionFromUpdate} from "./balance.controller.tsx";
import {getUserLoginEncrypt} from "../utils.ts";

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

    let login = await getUserLoginEncrypt();
    transaction = await selectTransactionById(login, transaction.Id) ?? transaction;
        
    transaction.Operation = await loadInternalOperation(transaction.Operation ?? {} as I.Operation);
    transaction.Portfolio = await loadInternalPortfolio(transaction.Portfolio ?? {} as I.Portfolio);

    if (transaction.DestinationPortfolio !== null)
        transaction.DestinationPortfolio = await loadInternalPortfolio(transaction.DestinationPortfolio);

    if (transaction.ParentTransaction !== null)
        transaction.ParentTransaction = await loadInternalTransaction(transaction.ParentTransaction);

    let internalTransaction = await selectTransactionById(login, transaction.Id);

    if (internalTransaction === undefined){
        internalTransaction = await insertTransaction(login, transaction);
    }  

    return internalTransaction;
}

export const loadAllTransactionsInternal = async (mountDateInicio: Date, mountDateFim: Date, pageNumber: Number): Promise<I.Response> => {
    let responseTransactions = {} as I.Response;

    let login = await getUserLoginEncrypt();
    responseTransactions.isLogged = true;
    responseTransactions.data = await selectAllTransactions(login, mountDateInicio, mountDateFim, pageNumber as number);
    let totalRecords = await selectContAll(login, mountDateInicio, mountDateFim);
    
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

    let login = await getUserLoginEncrypt();
    for (const item of transactions) {
        var transaction = await selectTransactionById(login, item.Id);
        
        item.Operation = await loadInternalOperation(item.Operation?? {} as I.Operation);
        item.Portfolio = await loadInternalPortfolio(item.Portfolio ?? {} as I.Portfolio);
        
        if (item.DestinationPortfolio !== undefined && item.DestinationPortfolio !== null)
            item.DestinationPortfolio = await loadInternalPortfolio(item.DestinationPortfolio);
        
        if (item.ParentTransaction !== undefined && item.ParentTransaction !== null)
            item.ParentTransaction = await loadInternalTransaction(item.ParentTransaction ?? {} as I.Transaction);
        
        if (transaction === undefined) {
            let transaction: I.Transaction = await insertTransaction(login, item);
        } else {
            item.InternalId = transaction.InternalId;
            await updateTransaction(item);
        }
    }

    await setLastSynchronization(synchronization);
    return await loadAllTransactionsInternal(mountDateInicio, mountDateFim, pageNumber);
}

export const loadTotalsTransactions = async (mountDateInicio: Date, mountDateFim: Date): Promise<I.TransactionTotals> => {
    let login = await getUserLoginEncrypt();
    return await selectTransactionsTotals(login, mountDateInicio, mountDateFim);
}

export const createTransaction = async (transaction: I.Transaction): Promise<I.Response> => {
    let response = await postTransaction(transaction);

    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(transaction, response);

    if (transaction.Operation.InternalId === undefined)
        transaction.Operation = await loadInternalOperation(response.data.Operation);

    if (!response.isConnected) {
        let login = await getUserLoginEncrypt();
        transaction = await insertTransaction(login, transaction);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        let login = await getUserLoginEncrypt();
        transaction = await insertTransaction(login, response.data);
    }

    await calculateBalanceByTransactionFromUpdate(null, transaction);

    return response;
}

export const alterTransaction = async (sourceTransaction: I.Transaction, transaction: I.Transaction): Promise<I.Response> => {
    
    let response = await putTransaction(transaction);
    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(transaction, response);

    if (transaction.Operation.InternalId === undefined)
        transaction.Operation = await loadInternalOperation(transaction.Operation);

    if (!response.isConnected) {
        transaction = await updateTransaction(transaction);
        await calculateBalanceByTransactionFromUpdate(sourceTransaction, transaction);
        
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        transaction = await updateTransaction(response.data);
        await calculateBalanceByTransactionFromUpdate(sourceTransaction, transaction);
    }

    return response;
}

const populateInternalFields = (transaction: I.Transaction, response: I.Response) => {
    if (transaction.InternalId)
        response.data.InternalId = transaction.InternalId;
    response.data.Portfolio.InternalId = transaction.Portfolio.InternalId;
    
    if (transaction.DestinationPortfolio !== null)
        response.data.DestinationPortfolio.InternalId = transaction.DestinationPortfolio?.InternalId;
    
    response.data.Operation.InternalId = transaction.Operation.InternalId;
    
    if (transaction.ParentTransaction !== null)
        response.data.ParentTransaction.InternalId = transaction.ParentTransaction?.InternalId;
}

export const excludeTransaction = async (transaction: I.Transaction): Promise<I.Response> => {

    let response = await deleteTransaction(transaction.Id);
    if (response && !response.isLogged)
        return response;

    if (!response.isConnected) {
        await deleteInternalTransaction(transaction.InternalId);
        await calculateBalanceByTransactionFromUpdate(transaction, null);
        
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data){
        await deleteInternalTransaction(transaction.InternalId);
        await calculateBalanceByTransactionFromUpdate(transaction, null);
    }

    return response;
}

export const executeRecurringTransaction = async (mountDateInicio: Date): Promise<I.Response> => {
    let params = `BaseDate=${Moment(mountDateInicio).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await postRecurringTransactions(params);

    return response;
}