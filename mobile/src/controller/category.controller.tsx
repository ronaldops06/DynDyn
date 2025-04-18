import Moment from 'moment';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalCategory,
    insertCategory,
    selectAllCategories,
    selectCategoryById, 
    selectContAllCategories,
    updateCategory
} from '../repository/category.repository';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';
import {deleteCategory, getCategories, postCategory, putCategory} from "../services/category.api.ts";
import {Alert} from "react-native";
import {existsPortfolioRelationshipCategory} from "../repository/portfolio.repository.tsx";
import {existsOperationRelationshipCategory} from "../repository/operation.repository.tsx";

/**
 * Método responsável por retornar a categoria persistida internamente para ser utilizada como referência.
 * Se a categoria ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da categoria, somente assegurar a existência interna para 
 * referência nas entidades mães.
 * @async
 * @param {I.Category} category - Objeto de categoria externo
 * @returns {Promise<I.Category>} - Promisse com o objeto da categoria interno
 */
export const loadInternalCategory = async (category: I.Category): Promise<I.Category> => {
    let internalCategory = await selectCategoryById(category.Id);

    if (internalCategory === undefined){
        internalCategory = await insertCategory(category);
    }

    return internalCategory;
}

export const loadAllCategoryInternal = async (type: Number | null, pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.isLogged = true;
    response.data = await selectAllCategories(type as number, pageNumber as number);
    response.totalPages = await selectContAllCategories(type as number);

    return response;
}

export const loadAllCategory = async (type: Number | null, pageNumber: Number | null): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.category);
    
    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    
    let response = await getCategories(params);

    if (response && !response.isLogged)
        return response;
    
    var categories = response?.data ?? [];

    for (const item of categories) {
        var category = await selectCategoryById(item.Id);

        if (category === undefined) {
            category = await insertCategory(item);
        } else {
            item.InternalId = category.InternalId;
            await updateCategory(item);
        }
    }
    
    await setLastSynchronization(synchronization);
    return await loadAllCategoryInternal(type, pageNumber);
}

export const createCategory = async (category: I.Category): Promise<I.Response> => {
    let response = await postCategory(category);
    
    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(category, response);

    if (!response.isConnected) {
        category = await insertCategory(category);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        category = await insertCategory(response.data);
    }
    
    return response;
}

export const alterCategory = async (category: I.Category): Promise<I.Response> => {
    let response = await putCategory(category);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(category, response);

    if (!response.isConnected) {
        category = await updateCategory(category);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null){
        category = await updateCategory(response.data);
    }

    return response;
}

const populateInternalFields = (category: I.Category, response: I.Response) => {
    if (category.InternalId)
        response.data.InternalId = category.InternalId;

}

export const excludeCategory = async (categoryId: number, categoryInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    
    if (await existsPortfolioRelationshipCategory(categoryInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a categoria, pois existem contas vinculadas a ela.");
        return response;
    }
    
    if (await existsOperationRelationshipCategory(categoryInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a categoria, pois existem operações relacionadas a ela.");
        return response;
    }

    response = await deleteCategory(categoryId);

    if (response && !response.isLogged)
        return response;
    
    if (!response.isConnected) {
        await deleteInternalCategory(categoryInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data){
        await deleteInternalCategory(categoryInternalId);
    }
    
    return response;
}