import Moment from 'moment';
import { Synchronization } from "../interfaces/interfaces";
import { openDatabase } from "./database";

export const createTableSynchronization = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS synchronizations (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        operation           TEXT,
        execution_date      TEXT,
        start_creation_date TEXT,
        end_creation_date   TEXT
      );
    `);
};

export const insertSynchronization = async (synchronization: Synchronization): Promise<Synchronization> => {
    
    const db = await openDatabase();
    const { Operation,
            ExecutionDate, 
            StartCreationDate, 
            EndCreationDate } = synchronization;

    const result = await db.executeSql(
      'INSERT INTO synchronizations '
                + '( operation'
                + ', execution_date'
                + ', start_creation_date'
                + ', end_creation_date'
                + ') VALUES (?, ?, ?, ?)',
      [ Operation,
        ExecutionDate, 
        StartCreationDate, 
        EndCreationDate ]
    );

    synchronization.InternalId = result[0].insertId;

    return synchronization;
};

export const updateSynchronization = async (synchronization: Synchronization): Promise<Synchronization> => {
    const db = await openDatabase();
    const { Operation,
            ExecutionDate, 
            StartCreationDate, 
            EndCreationDate,
            InternalId } = synchronization;

    await db.executeSql(
        'UPDATE synchronizations '
         + 'SET operation = ?'
           + ', execution_date = ?'
           + ', start_creation_date = ?'
           + ', end_creation_date = ?'
        + 'WHERE internal_id = ?',
        [ Operation,
          ExecutionDate, 
          StartCreationDate, 
          EndCreationDate,
          InternalId
        ]
    );

    return synchronization;
};

export const deleteSynchronization = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(`DELETE FROM synchronizations WHERE internal_id = ?`, [internalId]);
};

export const selectSynchronizationByCreationsDateAndOperation = async (startCreationDate: Date, endCreationDate: Date, operation: string): Promise<Synchronization | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(
                            'SELECT * '
                          + '  FROM synchronizations '
                          + ' WHERE start_creation_date = ? '
                          + '   AND end_creation_date = ?'
                          + '   AND operation = ?',
                          [Moment(startCreationDate, 'YYYY-MM-DD').format(),
                           Moment(endCreationDate, 'YYYY-MM-DD').format(),
                           operation]
                        );

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}  

const formatResult = (item: any): Synchronization => {
    const synchronization: Synchronization = {
        InternalId: item.internal_id,
        Operation: item.operation,
        ExecutionDate: item.execution_date,
        StartCreationDate: item.start_creation_date,
        EndCreationDate: item.end_creation_date
    }

    return synchronization;
}