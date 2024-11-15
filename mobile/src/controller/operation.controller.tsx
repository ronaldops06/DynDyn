import * as I from '../interfaces/interfaces';
import { insertOperation, selectOperationById } from '../repository/operation.repository';
import { loadInternalCategory } from './category.controller';

/**
 * Método responsável por retornar a operação persistida internamente para ser utilizada como referência.
 * Se a operação ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da operação, somente assegurar a existência interna para 
 * referência nas entidades mães.
 * @async
 * @param {I.Operation} operation - Objeto de operação externo
 * @returns {Promise<I.Operation>} - Promisse com o objeto da operação interno
 */
export const loadInternalOperation = async (operation: I.Operation): Promise<I.Operation> => {

    operation.Category = await loadInternalCategory(operation.Category);
    
    let internalOperation = await selectOperationById(operation.Id);

    if (internalOperation === undefined){
        internalOperation = await insertOperation(operation);
    }   

    return internalOperation;
}