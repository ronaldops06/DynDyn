import Moment from 'moment';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalOperationRole, 
    deleteInternalOperationRoleByExternalId,
    insertOperationRole,
    selectAllOperationRoles,
    selectOperationRoleById,
    selectContAllOperationRoles,
    updateOperationRole
} from '../repository/operation.role.repository';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';
import {deleteOperationRole, getOperationsRoles, postOperationRole, putOperationRole} from "../services/operation.role.api.ts";
import {Alert} from "react-native";
//import {existsTransactionRelationshipOperationRole} from "../repository/transaction.repository.tsx";
import {getUserLoginEncrypt} from "../utils.ts";
import {existsOperationRelationshipOperationRole} from "../repository/operation.repository.tsx";
import {existsTotalizerRelationshipOperationRole} from "../repository/totalizer.role.repository.ts";

/**
 * Método responsável por retornar a papeis de transação persistida internamente para ser utilizada como referência.
 * Se a papeis de transação ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da papeis de transação, somente assegurar a existência interna para 
 * referência nas entidades mães.
 * @async
 * @param {I.OperationRole} operationRole - Objeto de papeis de transação externo
 * @returns {Promise<I.OperationRole>} - Promisse com o objeto da papeis de transação interno
 */
export const loadInternalOperationRole = async (operationRole: I.OperationRole): Promise<I.OperationRole> => {
    let login = await getUserLoginEncrypt();
    let internalOperationRole = await selectOperationRoleById(login, operationRole.Id);
    
    if (internalOperationRole === undefined){
        internalOperationRole = await insertOperationRole(login, operationRole);
    }
    operationRole.InternalId = internalOperationRole.InternalId;
    return operationRole;
}

export const loadAllOperationRoleInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;
    
    let login = await getUserLoginEncrypt();
    response.isLogged = true;
    response.data = await selectAllOperationRoles(login, pageNumber as number);
    let totalRecords = await selectContAllOperationRoles(login);
    
    response.totalPages = Math.ceil(totalRecords/ constants.pageSize);

    return response;
}

export const synchronizationAllOperationRole = async (): Promise<I.Response | null> => {
    console.log("inicio sync operationRole");
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.operationRole);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await getOperationsRoles(params);

    if (response && !response.isLogged)
        return response;

    var categories = response?.data ?? [];

    let login = await getUserLoginEncrypt();
    for (const item of categories) {
        var operationRole = await selectOperationRoleById(login, item.Id);

        if (operationRole === undefined) {
            operationRole = await insertOperationRole(login, item);
        } else {
            item.InternalId = operationRole.InternalId;
            await updateOperationRole(item);
        }
    }

    if (response?.isConnected)
        await setLastSynchronization(synchronization);
    console.log("fim sync operationRole");
    return response;
}

export const loadAllOperationRole = async (pageNumber: Number | null): Promise<I.Response> => {
    
    let response = await synchronizationAllOperationRole();
    
    if (response && !response.isLogged)
        return response;
    
    return await loadAllOperationRoleInternal(pageNumber);
}

export const createOperationRole = async (operationRole: I.OperationRole): Promise<I.Response> => {
    let response = await postOperationRole(operationRole);
    
    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(operationRole, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        operationRole = await insertOperationRole(login, response.data);
        response.data = operationRole;
    }
    
    return response;
}

export const alterOperationRole = async (operationRole: I.OperationRole): Promise<I.Response> => {
    let response = await putOperationRole(operationRole);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(operationRole, response);

    if (response.data !== null){
        operationRole = await updateOperationRole(response.data);
        response.data = operationRole;
    }

    return response;
}

const populateInternalFields = (operationRole: I.OperationRole, response: I.Response) => {
    if (operationRole.InternalId)
        response.data.InternalId = operationRole.InternalId;

}

export const excludeOperationRole = async (operationRoleId: number, operationRoleInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    response.isLogged = true;
    
    let login = await getUserLoginEncrypt();
    
    if (await existsOperationRelationshipOperationRole(login, operationRoleInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir o papél de operação, pois existem operação(ões) vinculado(s) a ele.");
        return response;
    }

    if (await existsTotalizerRelationshipOperationRole(login, operationRoleInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir o papél de operação, pois existem totalizador(es) vinculado(s) a ele.");
        return response;
    }
    
    response = await deleteOperationRole(operationRoleId);

    if (response && !response.isLogged)
        return response;
    
    if (response.data){
        await deleteInternalOperationRole(operationRoleInternalId);
    }
    
    return response;
}

export const processActionOperationRole = async (operation: string, id: number) => {
    let login = await getUserLoginEncrypt();
    
    if (operation === constants.acao.delete)
        await deleteInternalOperationRoleByExternalId(login, id);
}