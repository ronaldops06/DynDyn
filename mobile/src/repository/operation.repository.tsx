import { Operation, OperationRole, Transaction} from "../interfaces/interfaces";
import {openDatabase} from "./database";
import SQLite, {ResultSet} from "react-native-sqlite-storage";
import {constants} from "../constants";
import {selectOperationRoleByOperationInternalId} from "./operation.role.repository.ts";

export const createTableOperation = async () => {
    const db = await openDatabase();

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS operations
        (
            internal_id    INTEGER PRIMARY KEY AUTOINCREMENT,
            id             NUMBER,
            name           TEXT,
            type           NUMBER,
            recurrent      NUMBER,
            salary         NUMBER,
            status         NUMBER,
            category_id    NUMBER,
            data_criacao   TEXT,
            data_alteracao TEXT,
            reference      TEXT
        );
    `);

    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_operations_id ' +
        'ON operations (id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_operations_type ON ' +
        'operations (type);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_operations_category_id ' +
        'ON operations (category_id);');

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS operations_roles_link
        (
            operation_id      NUMBER,
            operation_role_id NUMBER,
            reference           TEXT,
            FOREIGN KEY (operation_id) 
                REFERENCES operations(internal_id) ON DELETE CASCADE,
            FOREIGN KEY (operation_role_id)
                REFERENCES operation_roles(internal_id) ON DELETE CASCADE
        )
    `);
};

export const insertOperation = async (userLogin: string, operation: Operation): Promise<Operation> => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Type,
        Recurrent,
        Salary,
        Status,
        Category,
        DataCriacao,
        DataAlteracao,
        OperationRoles
    } = operation;

    const result = await db.executeSql(
        'INSERT INTO operations '
        + '( id'
        + ', name'
        + ', type'
        + ', recurrent'
        + ', salary'
        + ', status'
        + ', category_id'
        + ', data_criacao'
        + ', data_alteracao'
        + ', reference'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [Id,
            Name,
            Type,
            Recurrent,
            Salary,
            Status,
            Category?.InternalId,
            DataCriacao,
            DataAlteracao,
            userLogin]
    );

    operation.InternalId = result[0].insertId;

    for (const item of OperationRoles) {
        await insertOperationRoleLink(userLogin, operation.InternalId, item, db);
    }
    
    return operation;
};

export const updateOperation = async (userLogin: string, operation: Operation) => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Type,
        Recurrent,
        Salary,
        Status,
        Category,
        DataCriacao,
        DataAlteracao,
        InternalId,
        OperationRoles
    } = operation;

    await db.executeSql(
        'UPDATE operations '
        + 'SET id = ?'
        + ', name = ?'
        + ', type = ?'
        + ', recurrent = ?'
        + ', salary = ?'
        + ', status = ?'
        + ', category_id = ?'
        + ', data_criacao = ?'
        + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [Id,
            Name,
            Type,
            Recurrent,
            Salary,
            Status,
            Category?.InternalId,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
    
    let itens = await selectOperationRoleByOperationInternalId(InternalId);

    let remove = itens.filter(x => !OperationRoles?.includes(x));

    let add: OperationRole[] = [];
    if (OperationRoles !== null)
        add = OperationRoles.filter(x => !itens.includes(x));

    for (const item of remove) {
        await deleteOperationRoleLink(item.InternalId, InternalId, db);
    }
    
    for (const item of add) {
        await insertOperationRoleLink(userLogin, InternalId, item, db);
    }
    
    return operation;
};

export const insertOperationRoleLink = async (userLogin: string, operationInternalId: number, operationRole: OperationRole, db: SQLite.SQLiteDatabase) => {
    const {
        InternalId
    } = operationRole;

    const result = await db.executeSql(
        'INSERT INTO operations_roles_link ' +
        '( operation_id,' +
        '  operation_role_id,' +
        '  reference ' +
        ') VALUES (?, ?, ?)',
        [   operationInternalId,
            InternalId,
            userLogin
        ]);
}

export const deleteOperationRoleLink = async (operationRoleInternalId: number, operationInternalId: number, db: SQLite.SQLiteDatabase) => {
    await db.executeSql('DELETE FROM operations_roles_link' +
        ' WHERE operation_id = ?' +
        '   AND operation_role_id = ?',
        [operationInternalId, operationRoleInternalId]);
}

export const deleteInternalOperationByExternalId = async (userLogin: string, id: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM operations' +
        ' WHERE reference = ?' +
        '   AND id = ?'
        , [userLogin,
            id]);
};

export const deleteInternalOperation = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM operations' +
        ' WHERE internal_id = ?'
        , [internalId,]);
};

export const deleteAllOperations= async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM operations' +
        ' WHERE reference = ?', [userLogin,]);
}

export const selectAllOperations = async (userLogin: string, type: number | null, pageNumber: number | null, activated: boolean | null): Promise<Operation[]> => {
    const db = await openDatabase();

    let query = queryBase();

    let params = [];
    params.push(userLogin);

    if (activated !== null) {
        query += ' AND ope.status = ?';
        params.push(activated);
    }
    
    if (type !== null) {
        query += ' AND ope.type = ?';
        params.push(type);
    }
    
    let results: ResultSet[];
    if (pageNumber) {
        query += ' ORDER BY ope.name LIMIT ? OFFSET ? ';
        params.push(constants.pageSize);
        params.push((pageNumber - 1) * constants.pageSize);

        results = await db.executeSql(query, params);
    } else {
        query += ' ORDER BY ope.name';
        results = await db.executeSql(query, params);
    }

    const operations: Operation[] = [];
    for (const result of results){
        for (let i = 0; i < result.rows.length; i++) {
            let operation = formatResult(result.rows.item(i));
                        
            operation.OperationRoles = await selectOperationRoleByOperationInternalId(operation.InternalId);
            operations.push(operation);
        }
    }

    return operations;
};

export const selectContAllOperations = async (userLogin: string, type: number): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql('SELECT * ' +
        ' FROM operations ' +
        'WHERE reference = ?' +
        '  AND type = ?', 
        [userLogin, type]);

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectOperationById = async (userLogin: string, id: number): Promise<Operation | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(queryBase() + ' AND ope.id = ?', [userLogin, id]);
    
    let operation = undefined;
    if (result[0]?.rows.length > 0) {
        operation = formatResult(result[0]?.rows?.item(0));
        operation.OperationRoles = await selectOperationRoleByOperationInternalId(operation.InternalId);
    }
    
    return operation;
}

export const existsOperationRelationshipCategory = async (userLogin: string, categoryInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM operations' +
        ' WHERE reference = ?' +
        '   AND category_id = ?' +
        ' LIMIT 1'
        , [userLogin, categoryInternalId]);

    return result[0]?.rows.length > 0;
}

export const existsOperationRelationshipOperationRole = async (userLogin: string, operationRoleInternalId: number): Promise<boolean> => {
    const db = await openDatabase();
    
    const result = await db.executeSql(
        'SELECT ope.id' +
        ' FROM operations ope' +
        '      INNER JOIN operations_roles_link link ON ope.internal_id = link.operation_id' +
        ' WHERE ope.reference = ?' +
        '   AND link.operation_role_id = ?' +
        ' LIMIT 1'
        , [userLogin, operationRoleInternalId]);

    return result[0]?.rows.length > 0;
}

const queryBase = () => {
    return 'SELECT ope.*'
        + '     , cat.internal_id AS category_internal_id'
        + '     , cat.id AS category_id'
        + '     , cat.name AS category_name'
        + '     , cat.type AS category_type'
        + '     , cat.status AS category_status'
        + '     , cat.data_criacao AS category_data_criacao'
        + '     , cat.data_alteracao AS category_data_alteracao'
        + '  FROM operations ope'
        + '       INNER JOIN categories cat ON ope.category_id = cat.internal_id'
        + ' WHERE ope.reference = ?';
}

const formatResult = (item: any): Operation => {
    const operation: Operation = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Type: item.type,
        Recurrent: (item.recurrent === 1),
        Salary: (item.salary === 1),
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
        Category: {
            InternalId: item.category_internal_id,
            Id: item.category_id,
            Name: item.category_name,
            Type: item.category_type,
            Status: item.category_status,
            DataCriacao: item.category_data_criacao,
            DataAlteracao: item.category_data_alteracao,
        },
        OperationRoles: null,
        Roles: null
    }

    return operation;
}