import * as I from '../interfaces/interfaces';
import {
    insertOperation,
    selectAllOperations,
    selectContAllOperations,
    selectOperationById, updateOperation
} from '../repository/operation.repository';
import { loadInternalCategory } from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment";
import {getOperations} from "../services/operation.api.tsx";

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

export const loadAllOperationInternal = async (type: Number, pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;
    
    response.data = await selectAllOperations(type as number, pageNumber as number);
    response.totalPages = await selectContAllOperations(type as number);
    return response;
}

export const loadAllOperation = async (type: Number, pageNumber: Number | null, navigation: any): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.operation);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    let response = await getOperations(params, navigation);

    let operations = response?.data ?? [];

    var promises = operations.map(async (item: I.Operation) => {
        item.Category = await loadInternalCategory(item.Category);

        let operation = await selectOperationById(item.Id);
        
        if (operation === undefined) {
            await insertOperation(item);
        } else {
            item.InternalId = operation.InternalId;
            await updateOperation(item);
        }
    });

    await Promise.all(promises);
    await setLastSynchronization(synchronization);
    return await loadAllOperationInternal(type, pageNumber);
}