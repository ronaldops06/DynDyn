import {constants} from '../constants';
import * as I from '../interfaces/interfaces';
import {
    deleteInternalAttribute,
    deleteInternalAttributeByExternalId,
    insertAttribute,
    selectAllAttributes,
    selectAttributeById,
    selectContAllAttributes,
    updateAttribute
} from '../repository/attribute.repository';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from './synchronization.controller';
import {getUserLoginEncrypt} from "../utils.ts";
import Moment from "moment/moment";
import {deleteCategory} from "../services/category.api.ts";
import {getAttributes, postAttribute, putAttribute} from "../services/attribute.api.ts";

export const loadAllAttributeInternal = async (pageNumber: Number | null, activated: number | null): Promise<I.Response> => {
    let response = {} as I.Response;
    
    let login = await getUserLoginEncrypt();
    response.isLogged = true;
    response.data = await selectAllAttributes(login, pageNumber as number, activated as number);
    let totalRecords = await selectContAllAttributes(login);
    
    response.totalPages = Math.ceil(totalRecords/ constants.pageSize);

    return response;
}

export const synchronizationAllAttribute = async (): Promise<I.Response | null> => {
    console.log("inicio sync attribute");
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.attribute);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await getAttributes(params);

    if (response && !response.isLogged)
        return response;

    let attributes = response?.data ?? [];

    let login = await getUserLoginEncrypt();
    for (const item of attributes) {
        let attribute = await selectAttributeById(login, item.Id);

        if (attribute === undefined) {
            attribute = await insertAttribute(login, item);
        } else {
            item.InternalId = attribute.InternalId;
            attribute = await updateAttribute(login, item);
        }
    }

    if (response?.isConnected)
        await setLastSynchronization(synchronization);
    console.log("fim sync attribute");
    return response;
}

export const loadAllAttribute = async (pageNumber: Number | null, activated: number | null): Promise<I.Response> => {
    let response = await synchronizationAllAttribute();

    if (response && !response.isLogged)
        return response;
    
    return await loadAllAttributeInternal(pageNumber, activated);
}

export const createAttribute = async (attribute: I.Attribute): Promise<I.Response> => {
    let response = await postAttribute(attribute);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(attribute, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        attribute = await insertAttribute(login, response.data);
    }
    
    return response;
}

export const alterAttribute = async (attribute: I.Attribute): Promise<I.Response> => {
    let response = await putAttribute(attribute);
    
    if (response && !response.isLogged)
        return response;

    populateInternalFields(attribute, response);

    if (response.data !== null){
        let login = await getUserLoginEncrypt();
        attribute = await updateAttribute(login, response.data);
    }

    return response;
}

const populateInternalFields = (attribute: I.Attribute, response: I.Response) => {
    if (attribute.InternalId)
        response.data.InternalId = attribute.InternalId;

    for (let attributeOption: I.AttributeOption of response.data.Options) {
        let attributeOptionSrc = attribute.Options?.find(x => x.Id === attributeOption.Id);
        attributeOption.InternalId = attributeOptionSrc.InternalId;
    }
}

export const excludeAttribute = async (attributeId: number, attributeInternalId: number): Promise<I.Response> => {
    let response: I.Response = {} as I.Response;
    response.success = false;
    response.isLogged = true;
    
    let login = await getUserLoginEncrypt();

    response = await deleteCategory(attributeId);

    if (response && !response.isLogged)
        return response;

    if (response.data){
        await deleteInternalAttribute(attributeInternalId);
    }

    return response;
}

export const processActionAttribute = async (operation: string, id: number) => {
    let login = await getUserLoginEncrypt();
    
    if (operation === constants.acao.delete)
        await deleteInternalAttributeByExternalId(login, id);
}