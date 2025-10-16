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
        end_creation_date   TEXT,
        reference           TEXT
      );
    `);
};

export const insertSynchronization = async (userLogin: string, synchronization: Synchronization): Promise<Synchronization> => {
    
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
                + ', reference'
                + ') VALUES (?, ?, ?, ?, ?)',
      [ Operation,
        ExecutionDate.toString(), 
        StartCreationDate?.toString(), 
        EndCreationDate?.toString(),
        userLogin]
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
          ExecutionDate.toString(), 
          StartCreationDate?.toString(),
          EndCreationDate?.toString(),
          InternalId
        ]
    );
    
    return synchronization;
};

export const deleteSynchronization = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(`DELETE FROM synchronizations WHERE internal_id = ?`, [internalId]);
};

export const deleteAllSynchronizations = async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM synchronizations' +
        ' WHERE reference = ?', [userLogin]);
}

export const selectSynchronizationByCreationsDateAndOperation = async (userLogin: string, startCreationDate: Date | null, endCreationDate: Date | null, operation: string): Promise<Synchronization | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(getQuery(startCreationDate, endCreationDate),
                            getParams(userLogin, startCreationDate, endCreationDate, operation)
                        );

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}  

const formatResult = (item: any): Synchronization => {
    const synchronization: Synchronization = {
        InternalId: item.internal_id,
        Operation: item.operation,
        ExecutionDate: new Date(item.execution_date),
        StartCreationDate: new Date(item.start_creation_date),
        EndCreationDate: new Date(item.end_creation_date)
    }

    return synchronization;
}

const getQuery = (startCreationDate: Date | null, endCreationDate: Date | null): string => {
    let query = 'SELECT * ' +
            '  FROM synchronizations ' +
            ' WHERE reference = ?' +
            '   AND operation = ?';
    
    if (startCreationDate)
        query += '   AND start_creation_date = ? ';
            
    if(endCreationDate)
        query += '   AND end_creation_date = ?';
    
    return query;
} 

const getParams = (userLogin: string, startCreationDate: Date | null, endCreationDate: Date | null, operation: string): any[] => {
    let params: any[] = [];
    params.push(userLogin);
    params.push(operation);
    
    if (startCreationDate)
        params.push(Moment(startCreationDate, 'YYYY-MM-DD').format());

    if(endCreationDate)
        params.push(Moment(endCreationDate, 'YYYY-MM-DD').format());
    
    return params;
}