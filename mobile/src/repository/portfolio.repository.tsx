import {AttributeOption, Portfolio, PortfolioAttribute} from "../interfaces/interfaces";
import {addColumnIfNotExists, openDatabase} from "./database";
import {constants} from "../constants";
import SQLite, {ResultSet} from "react-native-sqlite-storage";

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
    
    await addColumnIfNotExists(db, 'portfolios', 'description', 'TEXT');
    await addColumnIfNotExists(db, 'portfolios', 'currency_code', 'TEXT');
    await addColumnIfNotExists(db, 'portfolios', 'acquisition_cost', 'NUMBER');
    await addColumnIfNotExists(db, 'portfolios', 'end_date', 'TEXT');
    
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolios_id ' +
        'ON portfolios (id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolios_reference ' +
        'ON portfolios (reference);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolios_parent_portfolio_id ' +
        'ON portfolios (parent_portfolio_id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolios_category_id ' +
        'ON portfolios (category_id);');

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS portfolio_attribute (
        internal_id         INTEGER PRIMARY KEY AUTOINCREMENT,
        id                  NUMBER,
        value_text          TEXT,
        value_number        NUMBER,
        value_boolean       NUMBER,
        value_date          TEXT,
        attribute_option_id NUMBER,
        action_type         NUMBER,
        status              NUMBER,
        attribute_id        NUMBER,
        portfolio_id        NUMBER,
        data_criacao        TEXT,
        data_alteracao      TEXT,
        reference           TEXT
      );
    `);

    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_id ' +
        'ON portfolio_attribute (id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_reference ' +
        'ON portfolio_attribute (reference);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_internal_id ' +
        'ON portfolio_attribute (internal_id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_attibute_id ' +
        'ON portfolio_attribute (attribute_id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_portfolio_id ' +
        'ON portfolio_attribute (portfolio_id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_portfolio_attribute_attribute_option_id ' +
        'ON portfolio_attribute (attribute_option_id);');
};

export const insertPortfolio = async (userLogin: string, portfolio: Portfolio): Promise<Portfolio> => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Description,
        CurrencyCode,
        AcquisitionCost,
        EndDate,
        Type,
        Group,
        Status,
        Category,
        ParentPortfolio,
        DataCriacao,
        DataAlteracao,
        Attributes
    } = portfolio;

    const result = await db.executeSql(
        'INSERT INTO portfolios '
        + '( id'
        + ', name'
        + ', description'
        + ', currency_code'
        + ', acquisition_cost'
        + ', end_date'
        + ', type'
        + ', group_portfolio'
        + ', status'
        + ', category_id'
        + ', parent_portfolio_id'
        + ', data_criacao'
        + ', data_alteracao'
        + ', reference'
        + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [Id, 
            Name,
            Description,
            CurrencyCode,
            AcquisitionCost,
            EndDate,
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

    for (const item of Attributes ?? []) {
        await insertPortfolioAttribute(userLogin, portfolio.InternalId, item, db);
    }
    
    return portfolio;
};

export const updatePortfolio = async (userLogin: string, portfolio: Portfolio) => {
    const db = await openDatabase();
    const {
        Id,
        Name,
        Description,
        CurrencyCode,
        AcquisitionCost,
        EndDate,
        Type,
        Group,
        Status,
        Category,
        ParentPortfolio,
        DataCriacao,
        DataAlteracao,
        InternalId,
        Attributes
    } = portfolio;

    await db.executeSql(
        'UPDATE portfolios '
        + 'SET id = ?'
        + ', name = ?'
        + ', description = ?'
        + ', currency_code = ?'
        + ', acquisition_cost = ?'
        + ', end_date = ?'
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
            Description,
            CurrencyCode,
            AcquisitionCost,
            EndDate,
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

    //Remove da base interna os que não vierem
    //Remove do que veio os que já existem internamente
    //Atualiza os que veio, que já existe internamente e que forma atualizados 
    let itens = await selectAttributeByPortfolioInternalId(InternalId);

    let remove = itens.filter(x => !Attributes?.includes(x));

    let add: PortfolioAttribute[] = [];
    if (Attributes !== null && Attributes !== undefined)
        add = Attributes.filter(x => !itens.includes(x));

    const mapItens = new Map(itens.map(item => [item.Id, item]));

    const update = Attributes?.filter(item2 => {
        const item1 = mapItens.get(item2.Id);

        return item1 &&
            new Date(item2.DataCriacao ?? new Date()) > new Date(item1.DataCriacao ?? new Date());
    });

    for (const item of remove) {
        await deletePortfolioAttribute(item.InternalId, InternalId, db);
    }

    for (const item of add) {
        await insertPortfolioAttribute(userLogin, InternalId, item, db);
    }

    for (const item of update ?? []){
        await updatePortfolioAttribute(item, db);
    }
    
    return portfolio;
};

export const insertPortfolioAttribute = async (userLogin: string, portfolioInternalId: number, portfolioAttribute: PortfolioAttribute, db: SQLite.SQLiteDatabase) => {
    const {
        Id, ValueNumber, ValueText, ValueDate, ValueBoolean, AttributeOption, ActionType, Attribute, Status, DataCriacao, DataAlteracao
    } = portfolioAttribute;

    const result = await db.executeSql(
        'INSERT INTO portfolio_attribute ' +
        '( id' +
        ', value_text' +
        ', value_number ' +
        ', value_boolean ' +
        ', value_date ' +
        ', attribute_option_id ' +
        ', action_type ' +
        ', status' +
        ', attribute_id' +
        ', portfolio_id' +
        ', data_criacao' +
        ', data_alteracao' +
        ', reference' +
        ') VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)',
        [   Id,
            ValueText,
            ValueNumber,
            ValueBoolean,
            ValueDate,
            AttributeOption?.InternalId,
            ActionType,
            Status,
            Attribute.InternalId,
            portfolioInternalId,
            DataCriacao,
            DataAlteracao,
            userLogin
        ]);
}

export const updatePortfolioAttribute = async (portfolioAttribute: PortfolioAttribute, db: SQLite.SQLiteDatabase) => {
    const {
        Id, InternalId, ValueNumber, ValueText, ValueDate, ValueBoolean, AttributeOption, ActionType, Attribute, Status, DataCriacao, DataAlteracao
    } = portfolioAttribute;

    await db.executeSql(
        'UPDATE portfolio_attribute ' +
        'SET id = ?' +
        '  , value_text  = ?' +
        '  , value_number = ?' +
        '  , value_boolean = ?' +
        '  , value_date = ?' +
        '  , attribute_option_id = ?' +
        '  , action_type = ?' +
        '  , status = ?' +
        '  , attribute_id = ?' +
        '  , data_criacao = ?' +
        '  , data_alteracao = ?' +
        + 'WHERE internal_id = ?',
        [ Id,
            ValueText,
            ValueNumber,
            ValueBoolean,
            ValueDate,
            AttributeOption?.InternalId,
            ActionType,
            Status,
            Attribute.InternalId,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
};

export const deletePortfolioAttribute = async (portfolioInternalId: number, portfolioAttributeInternalId: number, db: SQLite.SQLiteDatabase) => {
    await db.executeSql('DELETE FROM portfolio_attribute' +
        ' WHERE portfolio_id = ?' +
        '   AND internal_id = ?',
        [portfolioInternalId, portfolioAttributeInternalId]);
}

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

export const selectAllPortfolios = async (userLogin: string, groupsPortfolios:number[] | null, pageNumber: number | null, activated: boolean | null): Promise<Portfolio[]> => {
    const db = await openDatabase();
    
    let query = queryBase();
    
    let params = [];
    params.push(userLogin);
    
    if (activated !== null) {
        query += ' AND act.status = ?';
        params.push(activated);
    }

    if (groupsPortfolios !== null && groupsPortfolios.length > 0) {
        const placeholders = groupsPortfolios.map(() => '?').join(',');
        query += ` AND act.group_portfolio IN (${placeholders})`;
        params.push(...groupsPortfolios);
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
            let portfolio = await formatResult(results[j].rows.item(i));
            portfolio.Attributes = await selectAttributeByPortfolioInternalId(portfolio.InternalId);
            portfolios.push(portfolio);
        }
    };
    
    return portfolios;
};

export const selectContAllPortfolios = async (userLogin: string, groupsPortfolios:number[] | null): Promise<number> => {
    const db = await openDatabase();
    
    let query = 'SELECT * ' +
        '  FROM portfolios' +
        ' WHERE reference = ?';
    
    let params = [];
    params.push(userLogin);

    if (groupsPortfolios !== null && groupsPortfolios.length > 0) {
        const placeholders = groupsPortfolios.map(() => '?').join(',');
        query += ` AND group_portfolio IN (${placeholders})`;
        params.push(...groupsPortfolios);
    }

    const results = await db.executeSql(query, params);

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectPortfolioById = async (userLogin: string, id: number): Promise<Portfolio | undefined> => {
    const db = await openDatabase();
    const result = await db.executeSql(queryBase() + ' AND act.id = ?', [userLogin, id]);

    let portfolio = undefined;
    if (result[0]?.rows.length > 0) {
        portfolio = formatResult(result[0]?.rows?.item(0));
        portfolio.Attributes = await selectAttributeByPortfolioInternalId(portfolio.InternalId);
    }
        
    return portfolio;
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
        + '     , par_act.description AS parent_portfolio_description'
        + '     , par_act.currency_code AS parent_portfolio_currency_code'
        + '     , par_act.acquisition_cost AS parent_portfolio_acquisition_cost'
        + '     , par_act.end_date AS parent_portfolio_end_date'
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

const formatResult = (item: any): Portfolio => {
    const portfolio: Portfolio = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Description: item.description,
        CurrencyCode: item.currency_code,
        AcquisitionCost: item.acquisition_cost,
        EndDate: item.end_date,
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
            Description: item.parent_portfolio_description,
            CurrencyCode: item.parent_portfolio_currency_code,
            AcquisitionCost: item.parent_portfolio_acquisition_cost,
            EndDate: item.parent_portfolio_end_date,
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

export const selectAttributeByPortfolioInternalId = async (portfolioInternalId: number): Promise<PortfolioAttribute[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT port_attr.id port_attr_id' +
        '     , port_attr.internal_id port_attr_internal_id' +
        '     , port_attr.value_text port_attr_value_text' +
        '     , port_attr.value_number port_attr_value_number' +
        '     , port_attr.value_date port_attr_value_date' +
        '     , port_attr.value_boolean port_attr_value_boolean' +
        '     , port_attr.action_type port_attr_action_type' +
        '     , port_attr.status port_attr_status' +
        '     , port_attr.data_criacao port_attr_data_criacao' +
        '     , port_attr.data_alteracao port_attr_data_alteracao' +
        '     , port_attr.reference port_attr_reference' +
        '     , attr.id attr_id' +
        '     , attr.internal_id attr_internal_id' +
        '     , attr.name attr_name' +
        '     , attr.description attr_description' +
        '     , attr.data_type attr_data_type' +
        '     , attr.status attr_status' +
        '     , attr.data_criacao attr_data_criacao' +
        '     , attr.data_alteracao attr_data_alteracao' +
        '     , attr.data_alteracao attr_data_alteracao' +
        '     , attr_opt.id attr_opt_id' +
        '     , attr_opt.internal_id attr_opt_internal_id' +
        '     , attr_opt.label attr_label' +
        '     , attr_opt.is_default attr_opt_is_default' +
        '     , attr_opt.status attr_opt_status' +
        '     , attr_opt.data_criacao attr_opt_data_criacao' +
        '     , attr_opt.data_alteracao attr_opt_data_alteracao' +
        '  FROM portfolio_attribute port_attr' +
        '       INNER JOIN attributes attr ON attr.internal_id = port_attr.attribute_id' +
        '       LEFT JOIN attribute_option attr_opt ON attr_opt.internal_id = port_attr.attribute_option_id' +
        ' WHERE port_attr.portfolio_id = ?'
        , [portfolioInternalId,]);

    const portfolioAttributes: PortfolioAttribute[] = [];
    for(const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            portfolioAttributes.push(formatResultAttribute(result.rows.item(i)));
        }
    }

    return portfolioAttributes;
}

const formatResultAttribute = (item: any): PortfolioAttribute => {
    const attribute: PortfolioAttribute = {
        InternalId: item.internal_id,
        Id: item.id,
        ValueText: item.value_text,
        ValueNumber: item.value_number,
        ValueDate: item.value_date,
        ValueBoolean: item.value_boolean,
        AttributeOption: {
            InternalId: item.attr_opt_internal_id,
            Id: item.attr_opt_id,
            Label: item.attr_label,
            IsDefault: item.attr_opt_is_default,
            Status: item.attr_opt_status,
            DataCriacao: item.attr_opt_data_criacao,
            DataAlteracao: item.attr_opt_data_alteracao,
            tempId: ""
        },
        ActionType: item.action_type,
        Attribute: {
            InternalId: item.attr_internal_id,
            Id: item.attr_id,
            Name: item.attr_name,
            Description: item.attr_description,
            DataType: item.attr_data_type,
            Status: item.attr_status,
            DataCriacao: item.attr_data_criacao,
            DataAlteracao: item.attr_data_alteracao,
            Options: null
        },
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao
    };

    return attribute;
}