import * as I from '../interfaces/interfaces';
import { insertCategory, selectCategoryById } from '../repository/category.repository';

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