import {Account} from "../interfaces/interfaces";
import {openDatabase} from "./database";
import {constants} from "../constants";
import {ResultSet} from "react-native-sqlite-storage";

export const createTableAccounts = async () => {
    const db = await openDatabase();

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS accounts
        (
            internal_id       INTEGER PRIMARY KEY AUTOINCREMENT,
            id                NUMBER,
            name              TEXT,
            status            NUMBER,
            category_id       NUMBER,
            parent_account_id NUMBER,
            data_criacao      TEXT,
            data_alteracao    TEXT
        );
    `);
    
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_accounts_id ON accounts (id);`);
};

export const insertAccount = async (account: Account): Promise<Account> => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Status,
        Category,
        ParentAccount,
        DataCriacao,
        DataAlteracao
    } = account;

    const result = await db.executeSql(
        'INSERT INTO accounts '
        + '( id'
        + ', name'
        + ', status'
        + ', category_id'
        + ', parent_account_id'
        + ', data_criacao'
        + ', data_alteracao'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?)',
        [Id,
            Name,
            Status,
            Category?.InternalId,
            ParentAccount?.InternalId,
            DataCriacao,
            DataAlteracao]
    );

    account.InternalId = result[0].insertId;

    return account;
};

export const updateAccount = async (account: Account) => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Status,
        Category,
        ParentAccount,
        DataCriacao,
        DataAlteracao,
        InternalId
    } = account;

    await db.executeSql(
        'UPDATE accounts '
        + 'SET id = ?'
        + ', name = ?'
        + ', status = ?'
        + ', category_id = ?'
        + ', parent_account_id = ?'
        + ', data_criacao = ?'
        + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [Id,
            Name,
            Status,
            Category?.InternalId,
            ParentAccount?.InternalId,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
    
    return account;
};

export const deleteInternalAccount = async (internalId: number) => {
    const db = await openDatabase();
    
    await db.executeSql(
        'DELETE ' +
        '  FROM accounts ' +
        ' WHERE internal_id = ?'
        , [internalId]);
};

export const selectAllAccounts = async (pageNumber: number | null): Promise<Account[]> => {
    const db = await openDatabase();

    let results: ResultSet[];
    if (pageNumber)
        results = await db.executeSql(queryBase() + ' ORDER BY act.name LIMIT ? OFFSET ? ', [constants.pageSize, (pageNumber - 1) * constants.pageSize]);
    else
        results = await db.executeSql(queryBase() + ' ORDER BY act.name');

    const accounts: Account[] = [];
    for (let j = 0; j < results.length; j++) {
        for (let i = 0; i < results[j].rows.length; i++) {
            accounts.push(await formatResult(results[j].rows.item(i)));
        }
    };

    return accounts;
};

export const selectContAllAccounts = async (): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM accounts'
    );

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectAccountById = async (id: number): Promise<Account | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(queryBase() + ' WHERE act.id = ?', [id]);

    return result[0]?.rows.length > 0 ? await formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const existsAccountRelationshipCategory = async (categoryInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM accounts' +
        ' WHERE category_id = ?' +
        ' LIMIT 1'
        , [categoryInternalId]);

    return result[0]?.rows.length > 0;
}

export const existsAccountRelationshipAccount = async (accountInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM accounts' +
        ' WHERE parent_account_id = ? ' +
        ' LIMIT 1'
        , [ accountInternalId ]);

    return result[0]?.rows.length > 0;
}

const queryBase = () => {
    return 'SELECT act.*'
        + '     , cat.internal_id AS category_internal_id'
        + '     , cat.id AS category_id'
        + '     , cat.name AS category_name'
        + '     , cat.type AS category_type'
        + '     , cat.status AS category_status'
        + '     , cat.data_criacao AS category_data_criacao'
        + '     , cat.data_alteracao AS category_data_alteracao'
        + '     , par_act.internal_id AS parent_account_internal_id'
        + '     , par_act.id AS parent_account_id'
        + '     , par_act.name AS parent_account_name'
        + '     , par_act.status AS parent_account_status'
        + '     , par_act.data_criacao AS parent_account_data_criacao'
        + '     , par_act.data_alteracao AS parent_account_data_alteracao'
        + '     , par_cat.internal_id AS par_cat_internal_id'
        + '     , par_cat.id AS par_cat_id'
        + '     , par_cat.name AS par_cat_name'
        + '     , par_cat.type AS par_cat_type'
        + '     , par_cat.status AS par_cat_status'
        + '     , par_cat.data_criacao AS par_cat_data_criacao'
        + '     , par_cat.data_alteracao AS par_cat_data_alteracao'
        + '  FROM accounts act'
        + '       INNER JOIN categories cat ON act.category_id = cat.internal_id'
        + '       LEFT JOIN accounts par_act ON act.parent_account_id = par_act.internal_id'
        + '       LEFT JOIN categories par_cat ON par_act.category_id = par_cat.internal_id';
}

const formatResult = async (item: any): Promise<Account> => {
    const account: Account = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
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
        ParentAccount: null,
        BalanceTotals: null
    }

    if (item.parent_account_internal_id) {
        account.ParentAccount = {
            InternalId: item.parent_account_internal_id,
            Id: item.parent_account_id,
            Name: item.parent_account_name,
            Status: item.parent_account_status,
            DataCriacao: item.parent_account_data_criacao,
            DataAlteracao: item.parent_account_data_alteracao,
            Category: {
                InternalId: item.par_cat_internal_id,
                Id: item.par_cat_id,
                Name: item.par_cat_name,
                Type: item.par_cat_type,
                Status: item.par_cat_status,
                DataCriacao: item.par_cat_data_criacao,
                DataAlteracao: item.par_cat_data_alteracao,
            },
            ParentAccount: null,
            BalanceTotals: null
        }
    }

    return account;
}