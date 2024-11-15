import * as I from '../interfaces/interfaces';
import { insertAccount, selectAccountById } from '../repository/account.repository';
import { loadInternalCategory } from './category.controller';

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