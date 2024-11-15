import moment from 'moment';
import { Synchronization } from "../interfaces/interfaces";
import { insertSynchronization, selectSynchronizationByCreationsDateAndOperation, updateSynchronization } from "../repository/synchronization.repository";

export const loadSynchronizationByCreationsDateAndOperation = async (startCreationDate: Date, endCreationDate: Date, operation: string): Promise<Synchronization> => {

    let synchronization = await selectSynchronizationByCreationsDateAndOperation(startCreationDate, endCreationDate, operation);
    
    if (!synchronization){
        const executionDate = moment().utc(true);
        executionDate.add(- 100, "days");

        synchronization = {
            InternalId: null,
            Operation: operation,
            ExecutionDate: executionDate.toDate(),
            StartCreationDate: startCreationDate,
            EndCreationDate: endCreationDate
        };

        synchronization = await insertSynchronization(synchronization);
    }

    return synchronization;
};

export const setLastSynchronization = async (synchronization: Synchronization): Promise<Synchronization> => {

    synchronization.ExecutionDate = moment().utc(true).toDate();
    return await updateSynchronization(synchronization);
};