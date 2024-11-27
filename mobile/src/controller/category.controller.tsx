import Moment from 'moment';
import { constants } from '../constants';
import * as I from '../interfaces/interfaces';
import {
    insertCategory,
    selectAllCategories,
    selectCategoryById, selectContAllCategories,
    updateCategory
} from '../repository/category.repository';
import { loadSynchronizationByCreationsDateAndOperation, setLastSynchronization } from './synchronization.controller';
import {getCategories} from "../services/category.api.ts";

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

export const loadAllCategoryInternal = async (type: Number, pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.data = await selectAllCategories(type as number, pageNumber as number);
    response.totalPages = await selectContAllCategories(type as number);

    return response;
}

export const loadAllCategory = async (type: Number, pageNumber: Number | null, navigation: any): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.category);
    
    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    
    let response = await getCategories(params, navigation);
    
    var categories = response?.data ?? [];

    var promises = categories.map(async (item: I.Category) => {
        var category = await selectCategoryById(item.Id);

        if (category === undefined) {
            category = await insertCategory(item);
        } else {
            item.InternalId = category.InternalId;
            await updateCategory(item);
        }
    });
    
    await Promise.all(promises);
    await setLastSynchronization(synchronization);
    return await loadAllCategoryInternal(type, pageNumber);
}