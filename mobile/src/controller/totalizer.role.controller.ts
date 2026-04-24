import * as I from "../interfaces/interfaces.tsx";
import {getUserLoginEncrypt} from "../utils.ts";
import {
    deleteInternalTotalizerRole,
    deleteInternalTotalizerRoleByExternalId,
    selectAllTotalizerRoles,
    selectContAllTotalizerRoles,
    selectTotalizerRoleById,
    insertTotalizerRole,
    updateTotalizerRole, selectTotalizerRoleByCodeAndType
} from "../repository/totalizer.role.repository";
import {constants} from "../constants";
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller";
import Moment from "moment/moment";
import {getTotalizersRoles, deleteTotalizerRole, postTotalizerRole, putTotalizerRole} from "../services/totalizer.role.api";
import {loadInternalOperationRole} from "./operation.role.controller.ts";

export const loadAllTotalizerRoleInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    let login = await getUserLoginEncrypt();
    response.isLogged = true;
    response.data = await selectAllTotalizerRoles(login, pageNumber as number);
    let totalRecords = await selectContAllTotalizerRoles(login);

    response.totalPages = Math.ceil(totalRecords/ constants.pageSize);

    return response;
}

export const synchronizationAllTotalizerRole = async (): Promise<I.Response | null> => {
    console.log("inicio sync totalizer");
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.totalizerRole);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await getTotalizersRoles(params);

    if (response && !response.isLogged)
        return response;

    var totalizers = response?.data ?? [];

    let login = await getUserLoginEncrypt();
    for (const item of totalizers) {

        for (let operationRole of item.OperationRoles) {
            operationRole = await loadInternalOperationRole(operationRole);
        }

        var totalizerRole = await selectTotalizerRoleById(login, item.Id);

        if (totalizerRole === undefined) {
            totalizerRole = await insertTotalizerRole(login, item);
        } else {
            item.InternalId = totalizerRole.InternalId;
            await updateTotalizerRole(login, item);
        }
    }

    if (response?.isConnected)
        await setLastSynchronization(synchronization);
    console.log("fim sync totalizer");
    return response;
}

export const loadAllTotalizerRole = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = await synchronizationAllTotalizerRole();
    if (response && !response.isLogged)
        return response;
    
    return await loadAllTotalizerRoleInternal(pageNumber);
}

export const loadTotalizerRoleByCodeAndType = async (code: string, type: number): Promise<I.TotalizerRole> => {
    let login = await getUserLoginEncrypt();
    return await selectTotalizerRoleByCodeAndType(login, code, type);
}
export const createTotalizerRole = async (totalizerRole: I.TotalizerRole): Promise<I.Response> => {
    let response = await postTotalizerRole(totalizerRole);

    if (response && !response.isLogged)
        return response;

    if (response.data !== null){
        populateInternalFields(totalizerRole, response);
        let login = await getUserLoginEncrypt();
        totalizerRole = await insertTotalizerRole(login, response.data);
        response.data = totalizerRole;
    }

    return response;
}

export const alterTotalizerRole = async (totalizerRole: I.TotalizerRole): Promise<I.Response> => {
    let response = await putTotalizerRole(totalizerRole);

    if (response && !response.isLogged)
        return response;
    
    if (response.data !== null){
        populateInternalFields(totalizerRole, response);
        let login = await getUserLoginEncrypt();
        totalizerRole = await updateTotalizerRole(login, response.data);
        response.data = totalizerRole;
    }

    return response;
}

const populateInternalFields = (totalizerRole: I.TotalizerRole, response: I.Response) => {
    if (totalizerRole.InternalId)
        response.data.InternalId = totalizerRole.InternalId;

    for (let operationRole: I.OperationRole of response.data.OperationRoles) {
        let operationRoleSrc = totalizerRole.OperationRoles?.find(x => x.Id === operationRole.Id);
        operationRole.InternalId = operationRoleSrc.InternalId;
    }
}

export const excludeTotalizerRole = async (totalizerRoleId: number, totalizerRoleInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    let login = await getUserLoginEncrypt();

    /*if (await existsPortfolioRelationshipTotalizerRole(login, totalizerRoleInternalId)) {
        Alert.alert("Atenção!", "Não é possível excluir a papeis de transação, pois existem contas vinculadas a ela.");
        return response;
    }*/

    response = await deleteTotalizerRole(totalizerRoleId);

    if (response && !response.isLogged)
        return response;

    if (response.data){
        await deleteInternalTotalizerRole(totalizerRoleInternalId);
    }

    return response;
}

export const processActionTotalizerRole = async (operation: string, id: number) => {
    let login = await getUserLoginEncrypt();

    if (operation === constants.acao.delete)
        await deleteInternalTotalizerRoleByExternalId(login, id);
}