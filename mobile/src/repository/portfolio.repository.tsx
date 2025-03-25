import {Portfolio} from "../interfaces/interfaces";
import {openDatabase} from "./database";
import {constants} from "../constants";
import {ResultSet} from "react-native-sqlite-storage";

export const createTablePortfolios = async () => {
    const db = await openDatabase();

    await db.executeSql(`
        CREATE TABLE IF NOT EXISTS portfolios
        (
            internal_id       INTEGER PRIMARY KEY AUTOINCREMENT,
            id                NUMBER,
            name              TEXT,
            type              NUMBER,
            group_portfolio   NUMBER,
            status            NUMBER,
            category_id       NUMBER,
            parent_portfolio_id NUMBER,
            data_criacao      TEXT,
            data_alteracao    TEXT
        );
    `);
    
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_portfolios_id ON portfolios (id);`);
};

export const insertPortfolio = async (portfolio: Portfolio): Promise<Portfolio> => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Type,
        Group,
        Status,
        Category,
        ParentPortfolio,
        DataCriacao,
        DataAlteracao
    } = portfolio;

    const result = await db.executeSql(
        'INSERT INTO portfolios '
        + '( id'
        + ', name'
        + ', type'
        + ', group_portfolio'
        + ', status'
        + ', category_id'
        + ', parent_portfolio_id'
        + ', data_criacao'
        + ', data_alteracao'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [Id, 
            Name,
            Type,
            Group,
            Status,
            Category?.InternalId,
            ParentPortfolio?.InternalId,
            DataCriacao,
            DataAlteracao]
    );

    portfolio.InternalId = result[0].insertId;

    return portfolio;
};

export const updatePortfolio = async (portfolio: Portfolio) => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Type,
        Group,
        Status,
        Category,
        ParentPortfolio,
        DataCriacao,
        DataAlteracao,
        InternalId
    } = portfolio;

    await db.executeSql(
        'UPDATE portfolios '
        + 'SET id = ?'
        + ', name = ?'
        + ', type = ?'
        + ', group_portfolio = ?'
        + ', status = ?'
        + ', category_id = ?'
        + ', parent_portfolio_id = ?'
        + ', data_criacao = ?'
        + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [Id,
            Name,
            Type,
            Group,
            Status,
            Category?.InternalId,
            ParentPortfolio?.InternalId,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
    
    return portfolio;
};

export const deleteInternalPortfolio = async (internalId: number) => {
    const db = await openDatabase();
    
    await db.executeSql(
        'DELETE ' +
        '  FROM portfolios ' +
        ' WHERE internal_id = ?'
        , [internalId]);
};

export const selectAllPortfolios = async (pageNumber: number | null): Promise<Portfolio[]> => {
    const db = await openDatabase();

    let results: ResultSet[];
    if (pageNumber)
        results = await db.executeSql(queryBase() + ' ORDER BY act.name LIMIT ? OFFSET ? ', [constants.pageSize, (pageNumber - 1) * constants.pageSize]);
    else
        results = await db.executeSql(queryBase() + ' ORDER BY act.name');

    const portfolios: Portfolio[] = [];
    for (let j = 0; j < results.length; j++) {
        for (let i = 0; i < results[j].rows.length; i++) {
            portfolios.push(await formatResult(results[j].rows.item(i)));
        }
    };

    return portfolios;
};

export const selectContAllPortfolios = async (): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM portfolios'
    );

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectPortfolioById = async (id: number): Promise<Portfolio | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(queryBase() + ' WHERE act.id = ?', [id]);

    return result[0]?.rows.length > 0 ? await formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const existsPortfolioRelationshipCategory = async (categoryInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM portfolios' +
        ' WHERE category_id = ?' +
        ' LIMIT 1'
        , [categoryInternalId]);

    return result[0]?.rows.length > 0;
}

export const existsPortfolioRelationshipPortfolio = async (portfolioInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM portfolios' +
        ' WHERE parent_portfolio_id = ? ' +
        ' LIMIT 1'
        , [ portfolioInternalId ]);

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
        + '     , par_act.internal_id AS parent_portfolio_internal_id'
        + '     , par_act.id AS parent_portfolio_id'
        + '     , par_act.name AS parent_portfolio_name'
        + '     , par_act.type AS parent_portfolio_type'
        + '     , par_act.group_portfolio AS parent_portfolio_group'
        + '     , par_act.status AS parent_portfolio_status'
        + '     , par_act.data_criacao AS parent_portfolio_data_criacao'
        + '     , par_act.data_alteracao AS parent_portfolio_data_alteracao'
        + '     , par_cat.internal_id AS par_cat_internal_id'
        + '     , par_cat.id AS par_cat_id'
        + '     , par_cat.name AS par_cat_name'
        + '     , par_cat.type AS par_cat_type'
        + '     , par_cat.status AS par_cat_status'
        + '     , par_cat.data_criacao AS par_cat_data_criacao'
        + '     , par_cat.data_alteracao AS par_cat_data_alteracao'
        + '  FROM portfolios act'
        + '       INNER JOIN categories cat ON act.category_id = cat.internal_id'
        + '       LEFT JOIN portfolios par_act ON act.parent_portfolio_id = par_act.internal_id'
        + '       LEFT JOIN categories par_cat ON par_act.category_id = par_cat.internal_id';
}

const formatResult = async (item: any): Promise<Portfolio> => {
    const portfolio: Portfolio = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Type: item.type,
        Group: item.group_portfolio,
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
        ParentPortfolio: null,
        BalanceTotals: null
    }

    if (item.parent_portfolio_internal_id) {
        portfolio.ParentPortfolio = {
            InternalId: item.parent_portfolio_internal_id,
            Id: item.parent_portfolio_id,
            Name: item.parent_portfolio_name,
            Type: item.parent_portfolio_type,
            Group: item.parent_portfolio_group,
            Status: item.parent_portfolio_status,
            DataCriacao: item.parent_portfolio_data_criacao,
            DataAlteracao: item.parent_portfolio_data_alteracao,
            Category: {
                InternalId: item.par_cat_internal_id,
                Id: item.par_cat_id,
                Name: item.par_cat_name,
                Type: item.par_cat_type,
                Status: item.par_cat_status,
                DataCriacao: item.par_cat_data_criacao,
                DataAlteracao: item.par_cat_data_alteracao,
            },
            ParentPortfolio: null,
            BalanceTotals: null
        }
    }

    return portfolio;
}