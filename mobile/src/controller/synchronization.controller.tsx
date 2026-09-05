import moment from 'moment';
import {Synchronization, Trash} from "../interfaces/interfaces";
import {
    deleteAllSynchronizations,
    insertSynchronization,
    selectSynchronizationByCreationsDateAndOperation,
    updateSynchronization
} from "../repository/synchronization.repository";
import {getUserLoginEncrypt} from "../utils";
import {processActionCategory, synchronizationAllCategory} from "./category.controller";
import {processActionPortfolio, synchronizationAllPortfolio} from "./portfolio.controller";
import {processActionOperation, synchronizationAllOperation} from "./operation.controller";
import {processActionBalance, synchronizationAllBalance} from "./balance.controller";
import {processActionTransaction, synchronizationAllOTransaction} from "./transaction.controller";
import {constants} from "../constants";
import Moment from "moment/moment";
import {getTrashs} from "../services/trash.api";
import * as I from "../interfaces/interfaces";
import {deleteAllBalances} from "../repository/balance.repository";
import {deleteAllTransactions} from "../repository/transaction.repository";
import {deleteAllPortfolios} from "../repository/portfolio.repository";
import {deleteAllOperations} from "../repository/operation.repository";
import {deleteAllCategories} from "../repository/category.repository";
import {processActionOperationRole, synchronizationAllOperationRole} from "./operation.role.controller";
import {processActionTotalizerRole, synchronizationAllTotalizerRole} from "./totalizer.role.controller";
import {processActionAttribute, synchronizationAllAttribute} from "./attribute.controller";
import {deleteAllAttributes} from "../repository/attribute.repository";
import {deleteAllTotalizerRoles} from "../repository/totalizer.role.repository";
import {deleteAllOperationRoles} from "../repository/operation.role.repository";

export const loadSynchronizationByCreationsDateAndOperation = async (startCreationDate: Date | null, endCreationDate: Date | null, operation: string): Promise<Synchronization> => {
    let login = await getUserLoginEncrypt();
    let synchronization = await selectSynchronizationByCreationsDateAndOperation(login, startCreationDate, endCreationDate, operation);
    
    if (!synchronization){
        const executionDate = moment().utc(true);
        executionDate.add(- 100, "y");

        synchronization = {
            InternalId: null,
            Operation: operation,
            ExecutionDate: new Date(executionDate.format('YYYY-MM-DDTHH:mm:ss.SSS')),
            StartCreationDate: startCreationDate,
            EndCreationDate: endCreationDate
        };

        synchronization = await insertSynchronization(login, synchronization);
    }

    return synchronization;
};

export const setLastSynchronization = async (synchronization: Synchronization): Promise<Synchronization> => {

    synchronization.ExecutionDate = new Date(moment().utc(true).format('YYYY-MM-DDTHH:mm:ss.SSS'));
    return await updateSynchronization(synchronization);
};

export const loadAllTrash = async () => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.trash);

    let dataAtual = new Date(moment().utc(true).format('YYYY-MM-DDTHH:mm:ss.SSS'));
    let dataCorte = new Date(moment(dataAtual.setDate(dataAtual.getDate() - 29)).format('YYYY-MM-DDTHH:mm:ss.SSS'));

    //Se a última sincronização aconteceu a mais de 29 dias limpa todos os registros, assim, ao acessar cada funcionalidade, 
    // elas serão recarregadas. Isso garante a integridade dos dados, sendo que o server só guarda os registros excluídos no banco por 30 dias. 
    if (synchronization.ExecutionDate < dataCorte){
        await executeCleanupDataAccount();
        //Faz a busca da sincronização novamente pois a limpeza acima removeu o registro
        synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.trash);
    } else {

        let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

        let response = await getTrashs(params);
        var trashs = response?.data ?? [];
        
        for (const item of trashs) {
            await executeExcludeEntity(item);
        }
    }
    
    await setLastSynchronization(synchronization);
}

export const executeFullSynchronization = async (): Promise<I.Response> => {
    console.log('Inicio Sincronização');
    //Implementar tratamento de retorn em todos eles
    let response = await synchronizationAllCategory();
    if (response && !response.isLogged)
        return response;

    response = await synchronizationAllOperationRole();
    response = await synchronizationAllAttribute();
    response = await synchronizationAllOperation();
    response = await synchronizationAllPortfolio();
    response = await synchronizationAllTotalizerRole();
    response = await synchronizationAllBalance();
    
    //É definido um período de três meses (1 anterior e 1 depois do atual) para busca
    let date = new Date();
    let mountDateInicio = new Date(date.getFullYear(), date.getMonth() - 1, 5, 0, 0, 0);
    let mountDateFim = new Date(date.getFullYear(), date.getMonth() + 2, 4, 23, 59, 59);
    response = await synchronizationAllOTransaction(mountDateInicio, mountDateFim);

    console.log('Final Sincronização');
    return response ?? {} as I.Response;
}

export const executeCleanupDataAccount = async () => {
        
    let login = await getUserLoginEncrypt();
    await deleteAllBalances(login);
    await deleteAllTransactions(login);
    await deleteAllPortfolios(login);
    await deleteAllOperations(login);
    await deleteAllCategories(login);
    await deleteAllAttributes(login);
    await deleteAllTotalizerRoles(login);
    await deleteAllOperationRoles(login);
    await deleteAllSynchronizations(login);
}

export const executeExcludeEntity = async (trash: Trash) => {
    if (trash && trash.Reference === constants.operations.category) {
        await processActionCategory(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.attribute) {
        await processActionAttribute(constants.acao.delete, trash.ReferenceId)
    } else if (trash && trash.Reference === constants.operations.operation) {
        await processActionOperation(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.portfolio) {
        await processActionPortfolio(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.balance) {
        await processActionBalance(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.transaction) {
        await processActionTransaction(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.operationRole) {
        await processActionOperationRole(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.totalizerRole) {
        await processActionTotalizerRole(constants.acao.delete, trash.ReferenceId);
    }
};