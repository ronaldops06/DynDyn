import { Operation } from "../interfaces/interfaces";
import { openDatabase } from "./database";

export const createTableOperation = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS operations (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        name         TEXT,
        type         NUMBER,
        recurrent    NUMBER,
        salary       NUMBER,
        status       NUMBER,
        category_id  NUMBER,
        data_criacao TEXT,
        data_alteracao TEXT
      );
    `);
};

export const insertOperation = async (operation: Operation): Promise<Operation> => {
    const db = await openDatabase();
    const { Id,
            Name, 
            Type,
            Recurrent,
            Salary,
            Status,
            Category, 
            DataCriacao, 
            DataAlteracao } = operation;

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
                + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)',
      [ Id,
        Name,  
        Type,
        Recurrent,
        Salary,
        Status,
        Category?.InternalId, 
        DataCriacao, 
        DataAlteracao ]
    );

    operation.InternalId = result[0].insertId;

    return operation;
};

export const updateOperation = async (operation: Operation) => {
const db = await openDatabase();
const { Id,
        Name,
        Type,
        Recurrent,
        Salary,
        Status,
        Category, 
        DataCriacao,
        DataAlteracao,
        InternalId
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
    [ Id,
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
};

export const deleteOperation = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(`DELETE FROM operations WHERE internal_id = ?`, [internalId]);
};

export const selectAllOperations = async (): Promise<Operation[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(queryBase());
    
    const operations: Operation[] = [];
    results.forEach(result => {
        for (let i = 0; i < result.rows.length; i++) {
            operations.push(formatResult(result.rows.item(i)));
        }
    });

    return operations;
  };

export const selectOperationById = async (id: number): Promise<Operation | undefined> => {
    const db = await openDatabase();
  
    const result = await db.executeSql(queryBase() +' WHERE ope.id = ?', [id]);

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}

const queryBase = () => {
    return 'SELECT ope.*'
         + '     , cat.internal_id AS category_internalId'
         + '     , cat.id AS category_id'
         + '     , cat.name AS category_name'
         + '     , cat.type AS category_type'
         + '     , cat.status AS category_status'
         + '     , cat.data_criacao AS category_data_criacao'
         + '     , cat.data_alteracao AS category_data_alteracao'
         + '  FROM operations ope'
         + '       INNER JOIN categories cat ON ope.category_id = cat.internal_id';
}

const formatResult = (item: any): Operation => {
    const operation: Operation = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Type: item.type,
        Recurrent: item.recurrent,
        Salary: item.salary,
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
        Category: {
            InternalId: item.category_internalId,
            Id: item.category_id,
            Name: item.category_name,
            Type: item.category_type,
            Status: item.category_status,
            DataCriacao: item.category_data_criacao,
            DataAlteracao: item.category_data_alteracao,
        }}

    return operation;
}