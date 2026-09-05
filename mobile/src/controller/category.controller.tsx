import Moment from 'moment';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalCategory, 
    deleteInternalCategoryByExternalId,
    insertCategory,
    selectAllCategories,
    selectCategoryById,
    selectContAllCategories,
    updateCategory
} from '../repository/category.repository';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';
import {deleteCategory, getCategories, postCategory, putCategory} from "../services/category.api";
import {Alert} from "react-native";
import {existsPortfolioRelationshipCategory} from "../repository/portfolio.repository";
import {existsOperationRelationshipCategory} from "../repository/operation.repository";
import {getUserLoginEncrypt} from "../utils";

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
    let login = await getUserLoginEncrypt();
    let internalCategory = await selectCategoryById(login, category.Id);

    if (internalCategory === undefined){
        internalCategory = await insertCategory(login, category);
    }

    return internalCategory;
}

export const loadAllCategoryInternal = async (type: Number | null, pageNumber: Number | null, activated: boolean | null): Promise<I.Response> => {
    let response = {} as I.Response;
    
    let login = await getUserLoginEncrypt();
    response.isLogged = true;
    response.data = await selectAllCategories(login, type as number, pageNumber as number, activated);
    let totalRecords = await selectContAllCategories(login, type as number);
    
    response.totalPages = Math.ceil(totalRecords/ constants.pageSize);

    return response;
}

export const synchronizationAllCategory = async (): Promise<I.Response | null> => {
    console.log("inicio sync category");
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.category);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await getCategories(params);

    if (response && !response.isLogged)
        return response;

    let categories = response?.data ?? [];

    let login = await getUserLoginEncrypt();
    for (const item of categories) {
        let category = await selectCategoryById(login, item.Id);

        if (category === undefined) {
            category = await insertCategory(login, item);
        } else {
            item.InternalId = category.InternalId;
            category = await updateCategory(item);
        }
    }

    if (response?.isConnected)
        await setLastSynchronization(synchronization);
    console.log("fim sync category");
    return response;
}

export const loadAllCategory = async (type: Number | null, pageNumber: Number | null, activated: boolean | null): Promise<I.Response> => {
    let response = await synchronizationAllCategory();

    if (response && !response.isLogged)
        return response;
    
    return await loadAllCategoryInternal(type, pageNumber, activated);
}

export const createCategory = async (category: I.Category): Promise<I.Response> => {
    let response = await postCategory(category);
    
    if (response && !response.isLogged)
        return response;
    
    populateInternalFields(category, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        category = await insertCategory(login, response.data);
    }
    
    return response;
}

export const alterCategory = async (category: I.Category): Promise<I.Response> => {
    let response = await putCategory(category);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(category, response);

    if (response.data !== null){
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
    response.isLogged = true;
    
    let login = await getUserLoginEncrypt();
    
    if (await existsPortfolioRelationshipCategory(login, categoryInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a categoria, pois existem contas vinculadas a ela.");
        return response;
    }
    
    if (await existsOperationRelationshipCategory(login, categoryInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a categoria, pois existem operações relacionadas a ela.");
        return response;
    }

    response = await deleteCategory(categoryId);

    if (response && !response.isLogged)
        return response;
    
    if (response.data){
        await deleteInternalCategory(categoryInternalId);
    }
    
    return response;
}

export const processActionCategory = async (operation: string, id: number) => {
    let login = await getUserLoginEncrypt();
    
    if (operation === constants.acao.delete)
        await deleteInternalCategoryByExternalId(login, id);
}