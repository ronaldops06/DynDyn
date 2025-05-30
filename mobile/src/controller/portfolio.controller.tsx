import * as I from '../interfaces/interfaces';
import {
    deleteInternalPortfolio, existsPortfolioRelationshipPortfolio,
    insertPortfolio,
    selectPortfolioById,
    selectAllPortfolios,
    selectContAllPortfolios,
    updatePortfolio
} from '../repository/portfolio.repository';
import {loadInternalCategory} from './category.controller';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment/moment";
import {deletePortfolio, getPortfolios, postPortfolio, putPortfolio} from "../services/portfolio.api.ts";
import {Alert} from "react-native";
import {existsTransactionRelationshipPortfolio} from "../repository/transaction.repository.tsx";
import {selectTotalsByTreePortfolio} from "../repository/balance.repository.tsx";

/**
 * Método responsável por retornar a conta persistida internamente para ser utilizada como referência.
 * Se a conta ainda não estiver persistida irá criar.
 * Não é de responsabilidade deste método gerenciar a persistência da conta, somente assegurar a existência interna para
 * referência nas entidades mães.
 * @async
 * @param {I.Portfolio} portfolio - Objeto de conta externo
 * @returns {Promise<I.Portfolio>} - Promisse com o objeto da conta interno
 */
export const loadInternalPortfolio = async (portfolio: I.Portfolio): Promise<I.Portfolio> => {

    portfolio.Category = await loadInternalCategory(portfolio.Category);
    
    if (portfolio.ParentPortfolio !== null)
        portfolio.ParentPortfolio = await loadInternalPortfolio(portfolio.ParentPortfolio);

    let internalPortfolio = await selectPortfolioById(portfolio.Id);

    if (internalPortfolio === undefined) {
        internalPortfolio = await insertPortfolio(portfolio);
    }

    return internalPortfolio;
}

export const loadAllPortfolioInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.isLogged = true;
    response.data = await selectAllPortfolios(pageNumber as number);
    
    for (let portfolio of response.data) {
        portfolio.BalanceTotals = await selectTotalsByTreePortfolio(portfolio.InternalId);
    }

    response.totalPages = await selectContAllPortfolios();

    return response;
}

export const loadAllPortfolio = async (pageNumber: Number | null): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.portfolio);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;
    
    let response = await getPortfolios(params);
    
    if (response && !response.isLogged)
        return response;

    var portfolios = response?.data ?? [];
    
    for (const item of portfolios) {
        item.Category = await loadInternalCategory(item.Category);

        if (item.ParentPortfolio)
            item.ParentPortfolio = await loadInternalPortfolio(item.ParentPortfolio);

        var portfolio = await selectPortfolioById(item.Id);
        if (portfolio === undefined) {
            portfolio = await insertPortfolio(item);
        } else {
            item.InternalId = portfolio.InternalId;
            await updatePortfolio(item);
        }
    }

    await setLastSynchronization(synchronization);
    return await loadAllPortfolioInternal(pageNumber);
}

export const createPortfolio = async (portfolio: I.Portfolio): Promise<I.Response> => {
    let response = await postPortfolio(portfolio);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(portfolio, response);

    if (!response.isConnected) {
        portfolio = await insertPortfolio(portfolio);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        portfolio = await insertPortfolio(response.data);
    }

    return response;
}

export const alterPortfolio = async (portfolio: I.Portfolio): Promise<I.Response> => {
    let response = await putPortfolio(portfolio);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(portfolio, response);

    if (!response.isConnected) {
        portfolio = await updatePortfolio(portfolio);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        portfolio = await updatePortfolio(response.data);
    }

    return response;
}

const populateInternalFields = (portfolio: I.Portfolio, response: I.Response) => {
    if (portfolio.InternalId)
        response.data.InternalId = portfolio.InternalId;

    if (portfolio.Category !== null)
        response.data.Category.InternalId = portfolio.Category.InternalId;

    if (portfolio.ParentPortfolio !== null)
        response.data.ParentPortfolio.InternalId = portfolio.ParentPortfolio?.InternalId;

}

export const excludePortfolio = async (portfolioId: number, portfolioInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    
    if (await existsTransactionRelationshipPortfolio(portfolioInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a conta, pois existem transações vinculadas a ela.");
        return response;
    }

    if (await existsPortfolioRelationshipPortfolio(portfolioInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a conta, pois existem contas filhas relacionadas a ela.");
        return response;
    }

    response = await deletePortfolio(portfolioId);

    if (response && !response.isLogged)
        return response;

    if (!response.isConnected) {
        await deleteInternalPortfolio(portfolioInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data) {
        await deleteInternalPortfolio(portfolioInternalId);
    }

    return response;
}