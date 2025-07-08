import {openDatabase} from "./database.ts";
import {Balance, BalanceTotals, DashboardItem} from "../interfaces/interfaces.tsx";
import Moment from "moment/moment";
import {constants} from "../constants";
import {ResultSet, Transaction} from "react-native-sqlite-storage";

export const createTableBalance = async () => {
    const db = await openDatabase();

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS balances
        (
            internal_id          INTEGER PRIMARY KEY AUTOINCREMENT,
            id                   NUMBER,
            value                NUMBER,
            valuation            NUMBER,
            dividends            NUMBER,
            income               NUMBER,
            percentage_valuation NUMBER,
            percentage_income    NUMBER,
            credit               NUMBER,
            debit                NUMBER,
            salary_credit        NUMBER,
            salary_debit         NUMBER,
            inflow               NUMBER,
            outflow              NUMBER,
            portfolio_id           NUMBER,
            month                NUMBER,
            year                 NUMBER,
            data_criacao         TEXT,
            data_alteracao       TEXT
        );
    `);

    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_balances_portfolio_id ON balances (portfolio_id);`);
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_balances_id ON balances (id);`);
};

/**
 * Esse método executa todos os comandos (insert e update) usando uma única transaction de banco,
 *  essa abordagem melhora significativamente a performance (em até 10x).
 * @param {Balance[]} balances - Array de saldos a serem salvos (atualizado ou inserido) 
 */
export const saveBalances = async (balances: Balance[]): Promise<void> => {
    const db = await openDatabase();
    
    db.transaction((tx) => {
        for (let i = 0; i < balances.length; i++) {
            tx.executeSql(
                'SELECT bal.internal_id ' +
                '  FROM balances bal ' +
                ' WHERE bal.id = ?', 
                [balances[i].Id],
                (_, result) => {
                    let internalBalanceId : number = 0;
                    
                    if (result.rows.length > 0)
                        internalBalanceId = result.rows.item(0).internal_id;
                    
                    executeCommand(internalBalanceId, balances[i], tx);
                },
                (_, error) => {
                    console.log('Erro ao buscar os balances', error);
                });
        }
    });
};

const executeCommand = (internalBalanceId: number, balance: Balance, tx: Transaction) => {
    
    if (internalBalanceId === null || internalBalanceId === 0) {
        tx.executeSql(
            getCommandInsert(),
            getParameters(balance, constants.acao.insert),
            (_, result) => {},
            (_, error) => {
                console.log('Erro no insert de Balance', error);
            }
        );
    } else {
        balance.InternalId = internalBalanceId
        tx.executeSql(
            getCommandUpdate(),
            getParameters(balance, constants.acao.update),
            (_, result) => {},
            (_, error) => {
                console.log('Erro no update de Balance', error);
            }
        );
    }
}

export const insertBalance = async (balance: Balance, transaction: Transaction): Promise<Balance> => {
    const db = await openDatabase();

    const result = await db.executeSql(getCommandInsert(), getParameters(balance, constants.acao.insert));

    balance.InternalId = result[0].insertId;

    return balance;
};

export const updateBalance = async (balance: Balance): Promise<Balance> => {
    const db = await openDatabase();

    var result = await db.executeSql(getCommandUpdate(), getParameters(balance, constants.acao.update));
    console.log(result);

    return balance;
};

export const getCommandInsert = () => {
    return 'INSERT INTO balances '
        + '( id'
        + ', value'
        + ', valuation'
        + ', dividends'
        + ', income'
        + ', percentage_valuation'
        + ', percentage_income'
        + ', credit'
        + ', debit'
        + ', salary_credit'
        + ', salary_debit'
        + ', inflow'
        + ', outflow'
        + ', portfolio_id'
        + ', month'
        + ', year'
        + ', data_criacao'
        + ', data_alteracao'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)';
};

export const getCommandUpdate = () => {
    return 'UPDATE balances '
        + ' SET id = ?'
        + '   , value = ?'
        + '   , valuation = ?'
        + '   , dividends = ?'
        + '   , income = ?'
        + '   , percentage_valuation = ?'
        + '   , percentage_income = ?'
        + '   , credit = ?'
        + '   , debit = ?'
        + '   , salary_credit = ?'
        + '   , salary_debit = ?'
        + '   , inflow = ?'
        + '   , outflow = ?'
        + '   , portfolio_id = ?'
        + '   , month = ?'
        + '   , year = ?'
        + '   , data_criacao = ?'
        + '   , data_alteracao = ?'
        + ' WHERE internal_id = ?';
}

export const getParameters = (balance: Balance, acao: string): any[] => {
    const {
        Id,
        Value,
        Valuation,
        Dividends,
        Income,
        PercentageValuation,
        PercentageIncome,
        Credit,
        Debit,
        SalaryCredit,
        SalaryDebit,
        Inflow,
        Outflow,
        Portfolio,
        Month,
        Year,
        DataCriacao,
        DataAlteracao,
        InternalId
    } = balance;

    let parameters = [Id,
        Value,
        Valuation,
        Dividends,
        Income,
        PercentageValuation,
        PercentageIncome,
        Credit,
        Debit,
        SalaryCredit,
        SalaryDebit,
        Inflow,
        Outflow,
        Portfolio.InternalId,
        Month,
        Year,
        DataCriacao,
        DataAlteracao];
    
    if (acao === constants.acao.update) {
        parameters.push(InternalId);
    }
    
    return parameters;
}

export const deleteInternalBalance = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE'
        + '  FROM balances'
        + ' WHERE internal_id = ?', [internalId]);
};

export const selectAllBalances = async (pageNumber: number | null): Promise<Balance[]> => {
    const db = await openDatabase();

    let results: ResultSet[];
    if (pageNumber)
        results = await db.executeSql(queryBase() + ' ORDER BY bal.month, bal.year LIMIT ? OFFSET ?', [constants.pageSize, (pageNumber - 1) * constants.pageSize]);
    else
        results = await db.executeSql(queryBase() + ' ORDER BY bal.month, bal.year');

    const balances: Balance[] = [];
    results.forEach(result => {
        for (let i = 0; i < result.rows.length; i++) {
            balances.push(formatResult(result.rows.item(i)));
        }
    });

    return balances;
};

export const selectContAllBalances = async (): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM balances ');

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectTotalsByTreePortfolio = async (internalPortfolioId: number): Promise<BalanceTotals | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'WITH RECURSIVE portfolio_hierarchy AS (' +
        '   SELECT internal_id, parent_portfolio_id' +
        '     FROM portfolios' +
        '    WHERE internal_id = ?' +
        '    UNION ALL' +
        '   SELECT act.internal_id, act.parent_portfolio_id' +
        '     FROM portfolios act' +
        '    INNER JOIN portfolio_hierarchy act_hrc ON act.parent_portfolio_id = act_hrc.internal_id' +
        ')' +
        '   SELECT SUM(blc.value) AS value' +
        '     FROM portfolio_hierarchy act' +
        '    INNER JOIN balances blc ON act.internal_id = blc.portfolio_id'
        , [internalPortfolioId]
    );

    return result[0]?.rows.length > 0 ? formatResultTotals(result[0]?.rows?.item(0)) : undefined;
};

export const selectBalanceById = async (id: number): Promise<Balance | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(queryBase() + ' WHERE bal.id = ?', [id]);
    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const selectBalanceByBalanceMonthAndYear = async (internalPortfolioId: number, month: number, year: number): Promise<Balance | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(
        queryBase() +
        ' WHERE bal.month      = ?' +
        '   AND bal.year       = ?' +
        '   AND bal.portfolio_id = ?',
        [month,
            year,
            internalPortfolioId]);

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const selectDashboardBalanceGroupByMonth = async (year: number, month: number): Promise<DashboardItem[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'WITH RECURSIVE portfolio_hierarchy AS (' +
        '   SELECT internal_id, parent_portfolio_id' +
        '     FROM portfolios' +
        '    WHERE parent_portfolio_id IS NULL' +
        '    UNION ALL' +
        '   SELECT act.internal_id, act.parent_portfolio_id' +
        '     FROM portfolios act' +
        '    INNER JOIN portfolio_hierarchy act_hrc ON act.parent_portfolio_id = act_hrc.internal_id' +
        ')' +
        '   SELECT blc.year' +
        '        , blc.month' +
        '        , SUM(blc.value) AS value' +
        '     FROM portfolio_hierarchy act' +
        '    INNER JOIN balances blc ON act.internal_id = blc.portfolio_id' +
        '    WHERE ((blc.year > ?)' +
        '       OR (blc.year  = ? AND blc.month >= ?))' +
        '    GROUP BY blc.year' +
        '        , blc.month' +
        '    ORDER BY blc.year' +
        '        , blc.month;'
        , [ year
          , month]
    );

    const dashboardItens: DashboardItem[] = [];
    results.forEach(result => {
        for (let i = 0; i < result.rows.length; i++) {
            dashboardItens.push(formatResultDashboard(result.rows.item(i)));
        }
    });
    
    return dashboardItens;
}

const queryBase = () => {
    return 'SELECT bal.*'
        + '     , act.internal_id AS act_internal_id'
        + '     , act.id AS act_id'
        + '     , act.name AS act_name'
        + '     , act.type AS act_type'
        + '     , act.group_portfolio AS act_group'
        + '     , act.status AS act_status'
        + '     , act.data_criacao AS act_data_criacao'
        + '     , act.data_alteracao AS act_data_alteracao'
        + '     , act_cat.internal_id AS act_cat_internal_id'
        + '     , act_cat.id AS act_cat_id'
        + '     , act_cat.name AS act_cat_name'
        + '     , act_cat.type AS act_cat_type'
        + '     , act_cat.status AS act_cat_status'
        + '     , act_cat.data_criacao AS act_cat_data_criacao'
        + '     , act_cat.data_alteracao AS act_cat_data_alteracao'
        + '  FROM balances bal'
        + '       INNER JOIN portfolios act ON bal.portfolio_id = act.internal_id'
        + '       INNER JOIN categories act_cat ON act.category_id = act_cat.internal_id';
};

const formatResult = (item: any): Balance => {
    const balance: Balance = {
        InternalId: item.internal_id,
        Id: item.id,
        Value: item.value,
        Valuation: item.valuation,
        Dividends: item.dividends,
        Income: item.income,
        PercentageValuation: item.percentage_valuation,
        PercentageIncome: item.percentage_income,
        Credit: item.credit,
        Debit: item.debit,
        SalaryCredit: item.salary_credit,
        SalaryDebit: item.salary_debit,
        Inflow: item.inflow,
        Outflow: item.outflow,
        Portfolio: {
            InternalId: item.act_internal_id,
            Id: item.act_id,
            Name: item.act_name,
            Type: item.act_type,
            Group: item.act_group,
            Status: item.act_status,
            ParentPortfolio: null,
            Category: {
                InternalId: item.act_cat_internal_id,
                Id: item.act_cat_id,
                Name: item.act_cat_name,
                Type: item.act_cat_type,
                Status: item.act_cat_status,
                DataCriacao: item.act_cat_data_criacao,
                DataAlteracao: item.act_cat_data_alteracao
            },
            DataCriacao: item.act_data_criacao,
            DataAlteracao: item.act_data_alteracao,
            BalanceTotals: null
        },
        Month: item.month,
        Year: item.year,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao
    };

    return balance;
};

const formatResultTotals = (row: any): BalanceTotals => {
    const balanceTotals: BalanceTotals = {
        Value: row.value
    };

    return balanceTotals
}

const formatResultDashboard = (row: any): DashboardItem => {
    const dashboardItem: DashboardItem = {
        Label: `${row.month}\\${row.year}`,
        Value: row.value,
    }
    
    return dashboardItem;
}