import {OperationRole, TotalizerRole} from "../interfaces/interfaces";
import {openDatabase} from "./database";
import {constants} from "../constants";
import SQLite, {ResultSet} from "react-native-sqlite-storage";
import {
    selectOperationRoleByOperationInternalId,
    selectOperationRoleByTotalizerRoleInternalId
} from "./operation.role.repository";

export const createTableTotalizerRole = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS totalizer_roles (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        code         TEXT,
        type         NUMBER,
        data_criacao TEXT,
        data_alteracao TEXT,
        reference    TEXT
      );
    `);

    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_totalizer_roles_id ON totalizer_roles (id);`);
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_totalizer_reference ON totalizer_roles (reference);`);

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS totalizers_roles_link
        (
            totalizer_role_id      NUMBER,
            operation_role_id NUMBER,
            reference           TEXT,
            FOREIGN KEY (totalizer_role_id) 
                REFERENCES totalizer_roles(internal_id) ON DELETE CASCADE,
            FOREIGN KEY (operation_role_id)
                REFERENCES operation_roles(internal_id) ON DELETE CASCADE
        )
    `);

    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_totalizers_link_totalizer_role_id ON totalizers_roles_link (totalizer_role_id);`);
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_totalizer_link_operation_role_id ON totalizers_roles_link (operation_role_id);`);
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_totalizer_link_reference ON totalizers_roles_link (reference);`);
};

export const insertTotalizerRole = async (userLogin: string, totalizerRole: TotalizerRole): Promise<TotalizerRole> => {
    
    const db = await openDatabase();
    const { Id,
            Code,
            Type,
            DataCriacao, 
            DataAlteracao,
            OperationRoles } = totalizerRole;
    
    const result = await db.executeSql(
      'INSERT INTO totalizer_roles '
                + '( id'
                + ', code'
                + ', type'
                + ', data_criacao'
                + ', data_alteracao'
                + ', reference'
                + ') VALUES (?, ?, ?, ?, ?, ?)',
      [ Id,
        Code,
        Type,
        DataCriacao, 
        DataAlteracao, 
        userLogin]
    );

    totalizerRole.InternalId = result[0].insertId;

    for (const item of OperationRoles) {
        await insertTotalizerRoleLink(userLogin, totalizerRole.InternalId, item, db);
    }

    return totalizerRole;
};

export const updateTotalizerRole = async (userLogin: string, totalizerRole: TotalizerRole): Promise<TotalizerRole> => {
    const db = await openDatabase();
    const { Id,
            Code,
            Type,
            DataCriacao,
            DataAlteracao,
            InternalId,
            OperationRoles
            } = totalizerRole;

    await db.executeSql(
        'UPDATE totalizer_roles '
        + 'SET id = ?'
            + ', code = ?'
            + ', type = ?'
            + ', data_criacao = ?'
            + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [ Id,
        Code,
        Type,
        DataCriacao, 
        DataAlteracao,
        InternalId
        ]
    );

    let itens = await selectOperationRoleByTotalizerRoleInternalId(InternalId);

    let remove = itens.filter(x => !OperationRoles?.includes(x));

    let add: OperationRole[] = [];
    if (OperationRoles !== null)
        add = OperationRoles.filter(x => !itens.includes(x));

    for (const item of remove) {
        await deleteTotalizerRoleLink(item.InternalId, InternalId, db);
    }

    for (const item of add) {
        await insertTotalizerRoleLink(userLogin, InternalId, item, db);
    }
    
    return totalizerRole;
};

export const insertTotalizerRoleLink = async (userLogin: string, totalizerRoleInternalId: number, operationRole: OperationRole, db: SQLite.SQLiteDatabase) => {
    const {
        InternalId
    } = operationRole;
    
    if (!InternalId)
        return;
    
    const result = await db.executeSql(
        'INSERT INTO totalizers_roles_link ' +
        '( totalizer_role_id,' +
        '  operation_role_id,' +
        '  reference ' +
        ') VALUES (?, ?, ?)',
        [   totalizerRoleInternalId,
            InternalId,
            userLogin
        ]);
}

export const deleteTotalizerRoleLink = async (operationRoleInternalId: number, totalizerRoleInternalId: number, db: SQLite.SQLiteDatabase) => {
    await db.executeSql('DELETE FROM totalizers_roles_link' +
        ' WHERE totalizer_role_id = ?' +
        '   AND operation_role_id = ?',
        [totalizerRoleInternalId, operationRoleInternalId]);
}

export const deleteInternalTotalizerRole = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM totalizer_roles' +
        ' WHERE internal_id = ?'
        , [internalId,]);
};

export const deleteInternalTotalizerRoleByExternalId = async (userLogin: string, id: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM totalizer_roles' +
        ' WHERE reference = ?' +
        '   AND id = ?'
        , [userLogin,
            id]);
};

export const deleteAllTotalizerRoles = async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM totalizer_roles' +
        ' WHERE reference = ?', [userLogin,]);
}

export const selectAllTotalizerRoles = async (userLogin: string, pageNumber: number | null): Promise<TotalizerRole[]> => {
    const db = await openDatabase();
    
    let results: ResultSet[];
    let query = 'SELECT *' + 
        '  FROM totalizer_roles' +
        ' WHERE reference = ?';
    
    let params = [];
    params.push(userLogin);
    
    query += ' ORDER BY code';
    
    if (pageNumber){
        query += ' LIMIT ?' +
                 ' OFFSET ?';
        params.push(constants.pageSize);
        params.push((pageNumber - 1) * constants.pageSize);
    }
    
    results = await db.executeSql(query, params);
    
    let totalizerRoles: TotalizerRole[] = [];

    for (const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            let totalizerRole = await formatResult(result.rows.item(i));
            totalizerRole.OperationRoles = await selectOperationRoleByTotalizerRoleInternalId(totalizerRole.InternalId);
            totalizerRoles.push(totalizerRole);
        }
    }

    return totalizerRoles;
};

export const selectContAllTotalizerRoles = async (userLogin: string): Promise<number> => {
    const db = await openDatabase();
    
    let query = 'SELECT * ' +
        '  FROM totalizer_roles ' +
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

export const selectTotalizerRoleById = async (userLogin: string, id: number): Promise<TotalizerRole | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT * ' +
        '  FROM totalizer_roles ' +
        ' WHERE reference = ?' +
        '   AND id        = ?'
        , [userLogin, id]);
    
    let totalizer = undefined;
    if (result[0]?.rows.length > 0) {
        totalizer = formatResult(result[0]?.rows?.item(0))
        totalizer.OperationRoles = await selectOperationRoleByOperationInternalId(totalizer.InternalId);
    }
    
    return totalizer;
}

export const selectTotalizerRoleByCode = async (userLogin: string, code: string): Promise<TotalizerRole[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM totalizer_roles ' +
        ' WHERE reference = ?' +
        '   AND code      = ?'
        , [userLogin, code]);

    let totalizerRoles: TotalizerRole[] = [];
    for (const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            let totalizerRole = await formatResult(result.rows.item(i));
            totalizerRole.OperationRoles = await selectOperationRoleByTotalizerRoleInternalId(totalizerRole.InternalId);
            totalizerRoles.push(totalizerRole);
        }
    }
    
    return totalizerRoles;
}

export const selectTotalizerRoleByCodeAndType = async (userLogin: string, code: string, type: number): Promise<TotalizerRole> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT * ' +
        '  FROM totalizer_roles ' +
        ' WHERE reference = ?' +
        '   AND code      = ?' +
        '   AND type      = ?'
        , [userLogin, code, type]);

    let totalizer = undefined;
    if (result[0]?.rows.length > 0) {
        totalizer = formatResult(result[0]?.rows?.item(0))
        totalizer.OperationRoles = await selectOperationRoleByOperationInternalId(totalizer.InternalId);
    }

    return totalizer;
}

export const existsTotalizerRelationshipOperationRole = async (userLogin: string, operationRoleInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT * ' +
        '  FROM totalizers_roles_link' +
        ' WHERE reference = ?' +
        '   AND operation_role_id = ?' +
        ' LIMIT 1'
        , [userLogin, operationRoleInternalId]);

    return result[0]?.rows.length > 0;
}

const formatResult = (item: any): TotalizerRole => {
    const totalizerRole: TotalizerRole = {
        InternalId: item.internal_id,
        Id: item.id,
        Code: item.code,
        Type: item.type,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
        OperationRoles: null
    }

    return totalizerRole;
}