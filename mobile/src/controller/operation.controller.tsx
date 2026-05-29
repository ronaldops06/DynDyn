import * as I from '../interfaces/interfaces';
import {
    insertOperation,
    selectAllOperations,
    selectContAllOperations,
    selectOperationById,
    updateOperation,
    deleteInternalOperation, deleteInternalOperationByExternalId
} from '../repository/operation.repository';
import { loadInternalCategory } from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment";
import {getOperations, postOperation, putOperation, deleteOperation} from "../services/operation.api.ts";
import {Alert} from "react-native";
import {existsTransactionRelationshipOperation} from "../repository/transaction.repository.tsx";
import {getUserLoginEncrypt} from "../utils.ts";
import {deleteInternalBalanceByExternalId} from "../repository/balance.repository.tsx";
import {loadInternalOperationRole} from "./operation.role.controller.ts";

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

    for (let operationRole of operation.OperationRoles) {
        operationRole = await loadInternalOperationRole(operationRole);
    }
    
    let login = await getUserLoginEncrypt();
    let internalOperation = await selectOperationById(login, operation.Id);

    if (internalOperation === undefined){
        internalOperation = await insertOperation(login, operation);
    }

    return internalOperation;
}

export const loadAllOperationInternal = async (type: Number | null, pageNumber: Number | null, activated: boolean | null): Promise<I.Response> => {
    let response = {} as I.Response;

    let login = await getUserLoginEncrypt();
    response.isLogged = true;
    response.data = await selectAllOperations(login, type as number, pageNumber as number, activated);
    let totalRecords = await selectContAllOperations(login, type as number);

    response.totalPages = Math.ceil(totalRecords/ constants.pageSize);
    
    return response;
}

export const synchronizationAllOperation = async (): Promise<I.Response | null> => {
    console.log("inicio sync operation");
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.operation);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    let response = await getOperations(params);

    if (response && !response.isLogged)
        return response;

    let operations = response?.data ?? [];

    let login = await getUserLoginEncrypt();
    for (const item of operations) {
        item.Category = await loadInternalCategory(item.Category);

        for (let operationRole of item.OperationRoles) {
            operationRole = await loadInternalOperationRole(operationRole);
        }

        let operation = await selectOperationById(login, item.Id);
        if (operation === undefined) {
            operation = await insertOperation(login, item);
        } else {
            item.InternalId = operation.InternalId;
            operation = await updateOperation(login, item);
        }
    }

    if (response?.isConnected)
        await setLastSynchronization(synchronization);
    console.log("fim sync operation");
    return response;
}

export const loadAllOperation = async (type: Number, pageNumber: Number | null, activated: boolean | null): Promise<I.Response> => {
    
    let response = await synchronizationAllOperation();
    if (response && !response.isLogged)
        return response;
    
    return await loadAllOperationInternal(type, pageNumber, activated);
}

export const createOperation = async (operation: I.Operation): Promise<I.Response> => {
    let response = await postOperation(operation);

    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(operation, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        await insertOperation(login, response.data);
    }

    return response;
}

export const alterOperation = async (operation: I.Operation): Promise<I.Response> => {

    let response = await putOperation(operation);

    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(operation, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        await updateOperation(login, response.data);
    }

    return response;
}

const populateInternalFields = (operation: I.Operation, response: I.Response) => {
    if (operation.InternalId)
        response.data.InternalId = operation.InternalId;

    for (const operationRole: I.OperationRole of response.data.OperationRoles) {
        let operationRoleSrc = operation.OperationRoles?.find(x => x.Id === operationRole.Id);
        operationRole.InternalId = operationRoleSrc.InternalId;
    }
}

export const excludeOperation = async (operationId: number, operationInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    response.isLogged = true;

    let login = await getUserLoginEncrypt();
    
    if (await existsTransactionRelationshipOperation(login, operationInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a operação, pois existem transações relacionadas a ela.");
        return response;
    }

    response = await deleteOperation(operationId);

    if (response && !response.isLogged)
        return response;

    if (response.data){
        await deleteInternalOperation(operationInternalId);
    }

    return response;
}

export const processActionOperation = async (operation: string, id: number) => {
    let login = await getUserLoginEncrypt();

    if (operation === constants.acao.delete)
        await deleteInternalOperationByExternalId(login, id);
}