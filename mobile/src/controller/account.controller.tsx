import * as I from '../interfaces/interfaces';
import {
    deleteInternalAccount, existsAccountRelationshipAccount,
    insertAccount,
    selectAccountById,
    selectAllAccounts,
    selectContAllAccounts,
    updateAccount
} from '../repository/account.repository';
import {loadInternalCategory} from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment/moment";
import {deleteAccount, getAccounts, postAccount, putAccount} from "../services/account.api.ts";
import {Alert} from "react-native";
import {existsTransactionRelationshipAccount} from "../repository/transaction.repository.tsx";
import {selectTotalsByTreeAccount} from "../repository/balance.repository.tsx";

/**
 * Método responsável por retornar a conta persistida internamente para ser utilizada como referência.
 * Se a conta ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da conta, somente assegurar a existência interna para
 * referência nas entidades mães.
 * @async
 * @param {I.Account} account - Objeto de conta externo
 * @returns {Promise<I.Account>} - Promisse com o objeto da conta interno
 */
export const loadInternalAccount = async (account: I.Account): Promise<I.Account> => {

    account.Category = await loadInternalCategory(account.Category);
    
    if (account.ParentAccount !== null)
        account.ParentAccount = await loadInternalAccount(account.ParentAccount);

    let internalAccount = await selectAccountById(account.Id);

    if (internalAccount === undefined) {
        internalAccount = await insertAccount(account);
    }

    return internalAccount;
}

export const loadAllAccountInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.isLogged = true;
    response.data = await selectAllAccounts(pageNumber as number);
    
    for (let account of response.data) {
        account.BalanceTotals = await selectTotalsByTreeAccount(account.InternalId);
    }

    response.totalPages = await selectContAllAccounts();

    return response;
}

export const loadAllAccount = async (pageNumber: Number | null): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.account);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    
    let response = await getAccounts(params);
    
    if (response && !response.isLogged)
        return response;

    var accounts = response?.data ?? [];
    
    for (const item of accounts) {
        item.Category = await loadInternalCategory(item.Category);

        if (item.ParentAccount)
            item.ParentAccount = await loadInternalAccount(item.ParentAccount);

        var account = await selectAccountById(item.Id);
        if (account === undefined) {
            account = await insertAccount(item);
        } else {
            item.InternalId = account.InternalId;
            await updateAccount(item);
        }
    }

    await setLastSynchronization(synchronization);
    return await loadAllAccountInternal(pageNumber);
}

export const createAccount = async (account: I.Account): Promise<I.Response> => {
    let response = await postAccount(account);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(account, response);

    if (!response.isConnected) {
        account = await insertAccount(account);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        account = await insertAccount(response.data);
    }

    return response;
}

export const alterAccount = async (account: I.Account): Promise<I.Response> => {
    let response = await putAccount(account);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(account, response);

    if (!response.isConnected) {
        account = await updateAccount(account);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        account = await updateAccount(response.data);
    }

    return response;
}

const populateInternalFields = (account: I.Account, response: I.Response) => {
    if (account.InternalId)
        response.data.InternalId = account.InternalId;

    if (account.Category !== null)
        response.data.Category.InternalId = account.Category.InternalId;

    if (account.ParentAccount !== null)
        response.data.ParentAccount.InternalId = account.ParentAccount?.InternalId;

}

export const excludeAccount = async (accountId: number, accountInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    
    if (await existsTransactionRelationshipAccount(accountInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a conta, pois existem transações vinculadas a ela.");
        return response;
    }

    if (await existsAccountRelationshipAccount(accountInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a conta, pois existem contas filhas relacionadas a ela.");
        return response;
    }

    response = await deleteAccount(accountId);

    if (response && !response.isLogged)
        return response;

    if (!response.isConnected) {
        await deleteInternalAccount(accountInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data) {
        await deleteInternalAccount(accountInternalId);
    }

    return response;
}