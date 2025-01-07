import * as I from '../interfaces/interfaces';
import {
    insertOperation,
    selectAllOperations,
    selectContAllOperations,
    selectOperationById, 
    updateOperation,
    deleteInternalOperation
} from '../repository/operation.repository';
import { loadInternalCategory } from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment";
import {getOperations, postOperation, putOperation, deleteOperation} from "../services/operation.api.tsx";
import {Alert} from "react-native";
import {existsTransactionRelationshipOperation} from "../repository/transaction.repository.tsx";

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

    for (const item of operations) {
        item.Category = await loadInternalCategory(item.Category);

        let operation = await selectOperationById(item.Id);
        
        if (operation === undefined) {
            await insertOperation(item);
        } else {
            item.InternalId = operation.InternalId;
            await updateOperation(item);
        }
    }
    
    await setLastSynchronization(synchronization);
    return await loadAllOperationInternal(type, pageNumber);
}

export const createOperation = async (operation: I.Operation, navigation: any): Promise<I.Operation> => {
    let response = await postOperation(operation, navigation);

    populateInternalFields(operation, response);

    if (!response.isConnected) {
        operation = await insertOperation(operation);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
        navigation.goBack();
    } else if (response.data !== null){
        operation = await insertOperation(response.data);
        navigation.goBack();
    }

    return operation;
}

export const alterOperation = async (operation: I.Operation, navigation: any): Promise<I.Operation> => {
    let response = await putOperation(operation, navigation);

    populateInternalFields(operation, response);

    if (!response.isConnected) {
        operation = await updateOperation(operation);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
        navigation.goBack();
    } else if (response.data !== null){
        operation = await updateOperation(response.data);
        navigation.goBack();
    }

    return operation;
}

const populateInternalFields = (operation: I.Operation, response: I.Response) => {
    if (operation.InternalId)
        response.data.InternalId = operation.InternalId;

}

export const excludeOperation = async (operationId: number, operationInternalId: number, navigation: any): Promise<boolean> => {
    if (await existsTransactionRelationshipOperation(operationInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a operação, pois existem transações relacionadas a ela.");
        return false;
    }

    let response = await deleteOperation(operationId, navigation);

    if (!response.isConnected) {
        await deleteInternalOperation(operationInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
        navigation.goBack();
    } else if (response.data){
        await deleteInternalOperation(operationInternalId);
        navigation.goBack();
    }

    return true;
}