import Moment from 'moment';

import {constants} from "../constants";
import {TypesTransaction} from '../enums/enums';
import {Account, Category, Operation, Transaction, TransactionTotals} from "../interfaces/interfaces";
import {openDatabase} from "./database";

export const createTableTransaction = async () => {
    const db = await openDatabase();

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS transactions
        (
            internal_id            INTEGER PRIMARY KEY AUTOINCREMENT,
            id                     NUMBER,
            value                  NUMBER,
            observation            TEXT,
            consolidated           NUMBER,
            installment            NUMBER,
            total_installments     NUMBER,
            account_id             NUMBER,
            destination_account_id NUMBER,
            operation_id           NUMBER,
            parent_transaction_id  NUMBER,
            data_criacao           TEXT,
            data_alteracao         TEXT
        );
    `);
};

export const insertTransaction = async (transaction: Transaction): Promise<Transaction> => {
    const db = await openDatabase();
    const {
        Id,
        Value,
        Observation,
        Consolidated,
        Installment,
        TotalInstallments,
        Account,
        DestinationAccount,
        Operation,
        ParentTransaction,
        DataCriacao,
        DataAlteracao
    } = transaction;

    const result = await db.executeSql(
        'INSERT INTO transactions '
        + '( id'
        + ', value'
        + ', observation'
        + ', consolidated'
        + ', installment'
        + ', total_installments'
        + ', account_id'
        + ', destination_account_id'
        + ', operation_id'
        + ', parent_transaction_id'
        + ', data_criacao'
        + ', data_alteracao'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [Id,
            Value,
            Observation,
            Consolidated ? 1 : 0,
            Installment,
            TotalInstallments,
            Account?.InternalId,
            DestinationAccount?.InternalId,
            Operation?.InternalId,
            ParentTransaction?.InternalId,
            DataCriacao,
            DataAlteracao]
    );

    transaction.InternalId = result[0].insertId;

    return transaction;
};

export const updateTransaction = async (transaction: Transaction): Promise<Transaction> => {
    const db = await openDatabase();
    const {
        Id,
        Value,
        Observation,
        Consolidated,
        Installment,
        TotalInstallments,
        Account,
        DestinationAccount,
        Operation,
        ParentTransaction,
        DataCriacao,
        DataAlteracao,
        InternalId
    } = transaction;

    await db.executeSql(
        'UPDATE transactions '
        + 'SET id = ?'
        + ', value = ?'
        + ', observation = ?'
        + ', consolidated = ?'
        + ', installment = ?'
        + ', total_installments = ?'
        + ', account_id = ?'
        + ', destination_account_id = ?'
        + ', operation_id = ?'
        + ', parent_transaction_id = ?'
        + ', data_criacao = ?'
        + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [Id,
            Value,
            Observation,
            Consolidated ? 1 : 0,
            Installment,
            TotalInstallments,
            Account?.InternalId,
            DestinationAccount?.InternalId,
            Operation?.InternalId,
            ParentTransaction?.InternalId,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
    
    return transaction;
};

export const deleteInternalTransaction = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE'
        + '  FROM transactions'
        + ' WHERE internal_id = ?', [internalId]);
};

export const selectAllTransactions = async (dateInicio: Date, dateFim: Date, pageNumber: number): Promise<Transaction[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        queryBase()
        + ' WHERE strftime(\'%Y-%m-%d %H:%M:%S\', trn.data_criacao) BETWEEN ? AND ?'
        + '  LIMIT ? '
        + '  OFFSET ?',
        [Moment(dateInicio).format('YYYY-MM-DD HH:mm:ss'),
            Moment(dateFim).format('YYYY-MM-DD HH:mm:ss'),
            constants.pageSize,
            (pageNumber - 1) * constants.pageSize
        ]);

    const transactions: Transaction[] = [];
    results.forEach(result => {
        for (let i = 0; i < result.rows.length; i++) {
            transactions.push(formatResult(result.rows.item(i)));
        }
    });

    return transactions;
};

export const selectContAll = async (dateInicio: Date, dateFim: Date): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * '
        + '  FROM transactions '
        + ' WHERE strftime(\'%Y-%m-%d %H:%M:%S\', data_criacao) BETWEEN ? AND ?',
        [Moment(dateInicio).format('YYYY-MM-DD HH:mm:ss'),
            Moment(dateFim).format('YYYY-MM-DD HH:mm:ss'),
        ]);

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectTransactionsTotals = async (dateInicio: Date, dateFim: Date): Promise<TransactionTotals> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT ope.type AS type'
        + '     , SUM(trn.value) AS value'
        + '  FROM transactions trn'
        + '       INNER JOIN operations ope ON ope.internal_id = trn.operation_id'
        + ' WHERE trn.data_criacao BETWEEN ? AND ?'
        + ' GROUP BY ope.type',
        [Moment(dateInicio).format('YYYY-MM-DD HH:mm:ss'),
            Moment(dateFim).format('YYYY-MM-DD HH:mm:ss'),
        ]);

    return formatResultTotals(results[0]?.rows)
}

export const selectTransactionById = async (id: number): Promise<Transaction | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(queryBase() + ' WHERE trn.id = ?', [id]);

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const existsTransactionRelationshipOperation = async (operationInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM transactions' +
        ' WHERE operation_id = ?' +
        ' LIMIT 1'
        , [operationInternalId]);

    return result[0]?.rows.length > 0;
}

export const existsTransactionRelationshipAccount = async (accountInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM transactions' +
        ' WHERE account_id = ?' +
        '    OR destination_account_id = ?' +
        ' LIMIT 1'
        , [ accountInternalId, 
            accountInternalId ]);
    
    return result[0]?.rows.length > 0;
}

const queryBase = () => {
    return 'SELECT trn.*'
        + '     , ope.internal_id AS operation_internal_id'
        + '     , ope.id AS operation_id'
        + '     , ope.name AS operation_name'
        + '     , ope.type AS operation_type'
        + '     , ope.recurrent AS operation_recurrent'
        + '     , ope.salary AS operation_salary'
        + '     , ope.status AS operation_status'
        + '     , ope.data_criacao AS operation_data_criacao'
        + '     , ope.data_alteracao AS operation_data_alteracao'
        + '     , act.internal_id AS account_internal_id'
        + '     , act.id AS account_id'
        + '     , act.name AS account_name'
        + '     , act.status AS account_status'
        + '     , act.data_criacao AS account_data_criacao'
        + '     , act.data_alteracao AS account_data_alteracao'
        + '     , dest_act.internal_id AS dest_act_internal_id'
        + '     , dest_act.id AS dest_act_id'
        + '     , dest_act.name AS dest_act_name'
        + '     , dest_act.status AS dest_act_status'
        + '     , dest_act.data_criacao AS dest_act_data_criacao'
        + '     , dest_act.data_alteracao AS dest_act_data_alteracao'
        + '     , par_trn.internal_id AS par_trn_internal_id'
        + '     , par_trn.id AS par_trn_id'
        + '     , par_trn.value AS par_trn_value'
        + '     , par_trn.observation AS par_trn_observation'
        + '     , par_trn.consolidated AS par_trn_consolidated'
        + '     , par_trn.installment AS par_trn_installment'
        + '     , par_trn.total_installments AS par_trn_total_installments'
        + '     , par_trn.data_criacao AS par_trn_data_criacao'
        + '     , par_trn.data_alteracao AS par_trn_data_alteracao'
        + '  FROM transactions trn'
        + '       INNER JOIN operations ope ON trn.operation_id = ope.internal_id'
        + '       INNER JOIN accounts act ON trn.account_id = act.internal_id'
        + '       LEFT JOIN accounts dest_act ON dest_act.internal_id = trn.destination_account_id'
        + '       LEFT JOIN transactions par_trn ON par_trn.internal_id = trn.parent_transaction_id';
}

const formatResult = (item: any): Transaction => {
    const transaction: Transaction = {
        InternalId: item.internal_id,
        Id: item.id,
        Value: item.value,
        Observation: item.observation,
        Consolidated: (item.consolidated === 1),
        Installment: item.installment,
        TotalInstallments: item.total_installment,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
        Account: {
            InternalId: item.account_internal_id,
            Id: item.account_id,
            Name: item.account_name,
            Status: item.account_status,
            ParentAccount: null,
            Category: {} as Category,
            DataCriacao: item.account_data_criacao,
            DataAlteracao: item.account_data_alteracao
        },
        DestinationAccount: null,
        ParentTransaction: null,
        Operation: {
            InternalId: item.operation_internal_id,
            Id: item.operation_id,
            Name: item.operation_name,
            Type: item.operation_type,
            Recurrent: (item.operation_recurrent === 1),
            Salary: (item.operation_salary === 1),
            Status: item.operation_status,
            Category: {} as Category,
            DataCriacao: item.operation_data_criacao,
            DataAlteracao: item.operation_data_alteracao
        }
    }

    if (item.dest_act_internal_id) {
        transaction.DestinationAccount = {
            InternalId: item.dest_act_internal_id,
            Id: item.dest_act_id,
            Name: item.dest_act_name,
            Status: item.dest_act_status,
            ParentAccount: null,
            Category: {} as Category,
            DataCriacao: item.dest_act_data_criacao,
            DataAlteracao: item.dest_act_data_alteracao
        }
    }

    if (item.par_trn_internal_id) {
        transaction.ParentTransaction = {
            InternalId: item.par_trn_internal_id,
            Id: item.par_trn_id,
            Value: item.par_trn_value,
            Observation: item.par_trn_observation,
            Consolidated: item.par_trn_consolidated === 1 ? true : false,
            Installment: item.par_trn_installment,
            TotalInstallments: item.par_trn_total_installment,
            Account: {} as Account,
            DestinationAccount: null,
            ParentTransaction: null,
            Operation: {} as Operation,
            DataCriacao: item.par_trn_data_criacao,
            DataAlteracao: item.par_trn_data_alteracao,
        }
    }

    return transaction;
}

const formatResultTotals = (rows: any): TransactionTotals => {
    let transactionTotals: TransactionTotals = {
        Tansfer: 0,
        Credit: 0,
        Debit: 0,
    };

    for (let i = 0; i < rows.length; i++) {
        if (rows.item(i).type === TypesTransaction.Transference)
            transactionTotals.Tansfer = rows.item(i).value;
        else if (rows.item(i).type === TypesTransaction.Expense)
            transactionTotals.Debit = rows.item(i).value;
        else if (rows.item(i).type === TypesTransaction.Revenue)
            transactionTotals.Credit = rows.item(i).value;
    }

    return transactionTotals;
}