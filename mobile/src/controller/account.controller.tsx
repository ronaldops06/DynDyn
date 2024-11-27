import * as I from '../interfaces/interfaces';
import {
    insertAccount,
    selectAccountById,
    selectAllAccounts,
    selectContAllAccounts,
    updateAccount
} from '../repository/account.repository';
import { loadInternalCategory } from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment/moment";
import {getAccounts} from "../services/account.api.ts";

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
    
    let internalAccount = await selectAccountById(account.Id);
    
    if (internalAccount === undefined){
        internalAccount = await insertAccount(account);
    }    
    
    return internalAccount;
}

export const loadAllAccountInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.data = await selectAllAccounts(pageNumber as number);
    response.totalPages = await selectContAllAccounts();

    return response;
}

export const loadAllAccount = async (pageNumber: Number | null, navigation: any): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.account);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    let response = await getAccounts(params, navigation);

    var accounts = response?.data ?? [];

    var promises = accounts.map(async (item: I.Account) => {
        item.Category = await loadInternalCategory(item.Category);
        
        var account = await selectAccountById(item.Id);
        if (account === undefined) {
            account = await insertAccount(item);
        } else {
            item.InternalId = account.InternalId;
            await updateAccount(item);
        }
    });
    
    await Promise.all(promises);
    await setLastSynchronization(synchronization);
    return await loadAllAccountInternal(pageNumber);
}