import moment from 'moment';
import { Synchronization } from "../interfaces/interfaces";
import { insertSynchronization, selectSynchronizationByCreationsDateAndOperation, updateSynchronization } from "../repository/synchronization.repository";

export const loadSynchronizationByCreationsDateAndOperation = async (startCreationDate: Date | null, endCreationDate: Date | null, operation: string): Promise<Synchronization> => {

    let synchronization = await selectSynchronizationByCreationsDateAndOperation(startCreationDate, endCreationDate, operation);
    
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

        synchronization = await insertSynchronization(synchronization);
    }

    return synchronization;
};

export const setLastSynchronization = async (synchronization: Synchronization): Promise<Synchronization> => {

    synchronization.ExecutionDate = new Date(moment().utc(true).format('YYYY-MM-DDTHH:mm:ss.SSS'));
    return await updateSynchronization(synchronization);
};