import {OperationRole} from "../interfaces/interfaces";
import { openDatabase } from "./database";
import {constants} from "../constants";
import {ResultSet} from "react-native-sqlite-storage";

export const createTableOperationRole = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS operation_roles (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        name         TEXT,
        data_criacao TEXT,
        data_alteracao TEXT,
        reference    TEXT
      );
    `);

    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_operation_roles_id ON operation_roles (id);`);
};

export const insertOperationRole = async (userLogin: string, operationRole: OperationRole): Promise<OperationRole> => {
    
    const db = await openDatabase();
    const { Id,
            Name,
            DataCriacao, 
            DataAlteracao } = operationRole;

    const result = await db.executeSql(
      'INSERT INTO operation_roles '
                + '( id'
                + ', name'
                + ', data_criacao'
                + ', data_alteracao'
                + ', reference'
                + ') VALUES (?, ?, ?, ?, ?)',
      [ Id,
        Name,
        DataCriacao, 
        DataAlteracao, 
        userLogin]
    );

    operationRole.InternalId = result[0].insertId;

    return operationRole;
};

export const updateOperationRole = async (operationRole: OperationRole): Promise<OperationRole> => {
    const db = await openDatabase();
    const { Id,
            Name,
            DataCriacao,
            DataAlteracao,
            InternalId
            } = operationRole;

    await db.executeSql(
        'UPDATE operation_roles '
        + 'SET id = ?'
            + ', name = ?'
            + ', data_criacao = ?'
            + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [ Id,
        Name,
        DataCriacao, 
        DataAlteracao,
        InternalId
        ]
    );
    
    return operationRole;
};

export const deleteInternalOperationRole = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM operation_roles' +
        ' WHERE internal_id = ?'
        , [internalId,]);
};

export const deleteInternalOperationRoleByExternalId = async (userLogin: string, id: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM operation_roles' +
        ' WHERE reference = ?' +
        '   AND id = ?'
        , [userLogin,
            id]);
};

export const deleteAllOperationRoles = async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM operation_roles' +
        ' WHERE reference = ?', [userLogin,]);
}

export const selectAllOperationRoles = async (userLogin: string, pageNumber: number | null): Promise<OperationRole[]> => {
    const db = await openDatabase();
    
    let results: ResultSet[];
    let query = 'SELECT *' + 
        '  FROM operation_roles' +
        ' WHERE reference = ?';
    
    let params = [];
    params.push(userLogin);
    
    query += ' ORDER BY name';
    
    if (pageNumber){
        query += ' LIMIT ?' +
                 ' OFFSET ?';
        params.push(constants.pageSize);
        params.push((pageNumber - 1) * constants.pageSize);
    }
    
    results = await db.executeSql(query, params);
    
    const operation_roles: OperationRole[] = [];
    results.map(async result => {
        for (let i = 0; i < result.rows.length; i++) {
            operation_roles.push(await formatResult(result.rows.item(i)));
        }
    });

    return operation_roles;
};

export const selectContAllOperationRoles = async (userLogin: string): Promise<number> => {
    const db = await openDatabase();
    
    let query = 'SELECT * ' +
        '  FROM operation_roles ' +
        ' WHERE reference = ?';

    let params = [];
    params.push(userLogin);
        
    const results = await db.executeSql(query, params);

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectOperationRoleById = async (userLogin: string, id: number): Promise<OperationRole | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT * ' +
        '  FROM operation_roles ' +
        ' WHERE reference = ?' +
        '   AND id        = ?'
        , [userLogin, id]);


    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const selectOperationRoleByOperationInternalId = async (operationInternalId: number): Promise<OperationRole[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT ope_rol.* ' +
        '  FROM operation_roles ope_rol' +
        '     , operations_roles_link link ' +
        ' WHERE ope_rol.internal_id = link.operation_role_id' +
        '   AND link.operation_id   = ?'
        , [operationInternalId,]);

    const operationRoles: OperationRole[] = [];
    for(const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            operationRoles.push(formatResult(result.rows.item(i)));
        }
    }

    return operationRoles;
}

export const selectOperationRoleByTotalizerRoleInternalId = async (totalizerRoleInternalId: number): Promise<OperationRole[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT ope_rol.* ' +
        '  FROM operation_roles ope_rol' +
        '     , totalizers_roles_link link ' +
        ' WHERE ope_rol.internal_id = link.operation_role_id' +
        '   AND link.totalizer_role_id   = ?'
        , [totalizerRoleInternalId,]);

    const operationRoles: OperationRole[] = [];
    for(const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            operationRoles.push(formatResult(result.rows.item(i)));
        }
    }

    return operationRoles;
}

const formatResult = (item: any): OperationRole => {
    const operationRole: OperationRole = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao
    }

    return operationRole;
}