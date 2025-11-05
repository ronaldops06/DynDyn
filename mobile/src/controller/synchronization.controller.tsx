import moment from 'moment';
import {Synchronization, Trash} from "../interfaces/interfaces";
import {
    deleteAllSynchronizations,
    insertSynchronization,
    selectSynchronizationByCreationsDateAndOperation,
    updateSynchronization
} from "../repository/synchronization.repository";
import {getUserLoginEncrypt} from "../utils.ts";
import {processActionCategory} from "./category.controller.tsx";
import {processActionPortfolio} from "./portfolio.controller.tsx";
import {processActionOperation} from "./operation.controller.tsx";
import {processActionBalance} from "./balance.controller.tsx";
import {processActionTransaction} from "./transaction.controller.tsx";
import {constants} from "../constants";
import Moment from "moment/moment";
import {getTrashs} from "../services/trash.api.ts";
import * as I from "../interfaces/interfaces.tsx";
import {deleteAllBalances} from "../repository/balance.repository.tsx";
import {deleteAllTransactions} from "../repository/transaction.repository.tsx";
import {deleteAllPortfolios} from "../repository/portfolio.repository.tsx";
import {deleteAllOperations} from "../repository/operation.repository.tsx";
import {deleteAllCategories} from "../repository/category.repository.tsx";

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

export const executeCleanupDataAccount = async () => {
        
    let login = await getUserLoginEncrypt();
    await deleteAllBalances(login);
    await deleteAllTransactions(login);
    await deleteAllPortfolios(login);
    await deleteAllOperations(login);
    await deleteAllCategories(login);
    await deleteAllSynchronizations(login);
}

export const executeExcludeEntity = async (trash: Trash) => {
    if (trash && trash.Reference === constants.operations.category) {
        await processActionCategory(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.operation) {
        await processActionOperation(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.portfolio) {
        await processActionPortfolio(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.balance) {
        await processActionBalance(constants.acao.delete, trash.ReferenceId);
    } else if (trash && trash.Reference === constants.operations.transaction) {
        await processActionTransaction(constants.acao.delete, trash.ReferenceId);
    }
};