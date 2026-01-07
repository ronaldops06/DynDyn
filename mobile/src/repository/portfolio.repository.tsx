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
            data_alteracao    TEXT,
            reference         TEXT
        );
    `);
    
    await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_portfolios_id ON portfolios (id);`);
};

export const insertPortfolio = async (userLogin: string, portfolio: Portfolio): Promise<Portfolio> => {
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
        + ', reference'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [Id, 
            Name,
            Type,
            Group,
            Status,
            Category?.InternalId,
            ParentPortfolio?.InternalId,
            DataCriacao,
            DataAlteracao,
            userLogin]
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

export const deleteInternalPortfolioByExternalId = async (userLogin: string, id: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM portfolios' +
        ' WHERE reference = ?' +
        '   AND id = ?'
        , [userLogin,
            id]);
};

export const deleteInternalPortfolio = async (internalId: number) => {
    const db = await openDatabase();
    
    await db.executeSql(
        'DELETE ' +
        '  FROM portfolios ' +
        ' WHERE internal_id = ?'
        , [internalId]);
};

export const deleteAllPortfolios = async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM portfolios' +
        ' WHERE reference = ?', [userLogin]);
}

export const selectAllPortfolios = async (userLogin: string, pageNumber: number | null, activated: boolean | null): Promise<Portfolio[]> => {
    const db = await openDatabase();
    
    let query = queryBase();
    
    let params = [];
    params.push(userLogin);
    
    if (activated !== null) {
        query += ' AND act.status = ?';
        params.push(activated);
    }
    
    let results: ResultSet[];
    if (pageNumber) {
        query += ' ORDER BY act.name LIMIT ? OFFSET ? ';
        params.push(constants.pageSize);
        params.push((pageNumber - 1) * constants.pageSize);
        
        results = await db.executeSql(query, params);
    } else {
        query += ' ORDER BY act.name';
        results = await db.executeSql(query, params);
    }

    const portfolios: Portfolio[] = [];

    for (let j = 0; j < results.length; j++) {
        for (let i = 0; i < results[j].rows.length; i++) {
            portfolios.push(await formatResult(results[j].rows.item(i)));
        }
    };

    return portfolios;
};

export const selectContAllPortfolios = async (userLogin: string): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM portfolios' +
        ' WHERE reference = ?',
        [userLogin,]
    );

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectPortfolioById = async (userLogin: string, id: number): Promise<Portfolio | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(queryBase() + ' AND act.id = ?', [userLogin, id]);

    return result[0]?.rows.length > 0 ? await formatResult(result[0]?.rows?.item(0)) : undefined;
}

export const existsPortfolioRelationshipCategory = async (userLogin: string, categoryInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM portfolios' +
        ' WHERE reference = ?' +
        '   AND category_id = ?' +
        ' LIMIT 1'
        , [userLogin, categoryInternalId]);

    return result[0]?.rows.length > 0;
}

export const existsPortfolioRelationshipPortfolio = async (userLogin: string, portfolioInternalId: number): Promise<boolean> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT *' +
        ' FROM portfolios' +
        ' WHERE reference = ?' +
        '   AND parent_portfolio_id = ? ' +
        ' LIMIT 1'
        , [userLogin, portfolioInternalId]);

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
        + '       LEFT JOIN categories par_cat ON par_act.category_id = par_cat.internal_id'
        + ' WHERE act.reference = ?';
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