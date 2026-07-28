import {Attribute, AttributeOption, Category, Operation, OperationRole} from "../interfaces/interfaces";
import { openDatabase } from "./database";
import {constants} from "../constants";
import SQLite, {ResultSet} from "react-native-sqlite-storage";
import {deleteOperationRoleLink, insertOperationRoleLink} from "./operation.repository.tsx";
import {selectOperationRoleByOperationInternalId} from "./operation.role.repository.ts";

export const createTableAttribute = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS attributes (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        name         TEXT,
        description  TEXT,
        data_type    NUMBER,
        status       NUMBER,
        data_criacao TEXT,
        data_alteracao TEXT,
        reference    TEXT
      );
    `);

    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attributes_id ' +
        'ON attributes (id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attributes_reference ' +
        'ON attributes (reference);');

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS attribute_option (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        label         TEXT,
        is_default    NUMBER,
        status       NUMBER,
        attribute_id NUMBER,
        data_criacao TEXT,
        data_alteracao TEXT,
        reference    TEXT
      );
    `);

    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attribute_option_id ' +
        'ON attribute_option (id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attribute_option_reference ' +
        'ON attribute_option (reference);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attribute_option_internal_id ' +
        'ON attribute_option (internal_id);');
    await db.executeSql('CREATE INDEX IF NOT EXISTS idx_attribute_option_attibute_id ' +
        'ON attribute_option (attribute_id);');
};

export const insertAttribute = async (userLogin: string, attribute: Attribute): Promise<Attribute> => {
    const db = await openDatabase();
    const { Id, Name, Description, DataType, Status, DataCriacao, DataAlteracao, Options } = attribute;

    const result = await db.executeSql(
      'INSERT INTO attributes '
                + '( id'
                + ', name'
                + ', description'
                + ', data_type'
                + ', status'
                + ', data_criacao'
                + ', data_alteracao'
                + ', reference'
                + ') VALUES (?, ?, ?, ?, ?, ?, ?, ?)',
      [ Id, Name, Description, DataType, Status, DataCriacao, DataAlteracao, userLogin]
    );

    attribute.InternalId = result[0].insertId;
    
    for (const item of Options) {
        await insertAttributeOption(userLogin, attribute.InternalId, item, db);
    }

    return attribute;
};

export const updateAttribute = async (userLogin: string, attribute: Attribute): Promise<Attribute> => {
    const db = await openDatabase();
    
    const { Id, Name, Description, DataType, Status, DataCriacao, DataAlteracao, InternalId, Options } = attribute;

    await db.executeSql(
        'UPDATE attributes '
        + 'SET id = ?'
            + ', name = ?'
            + ', description = ?'
            + ', data_type = ?'
            + ', status = ?'
            + ', data_criacao = ?'
            + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [ Id, Name, Description, DataType, Status, DataCriacao, DataAlteracao, InternalId ]
    );

    //Remove da base interna os que não vierem
    //Remove do que veio os que já existem internamente
    //Atualiza os que veio, que já existe internamente e que forma atualizados 
    let itens = await selectOptionByAttributeInternalId(InternalId);

    let remove = itens.filter(x => !Options?.includes(x));

    let add: AttributeOption[] = [];
    if (Options !== null)
        add = Options.filter(x => !itens.includes(x));

    const mapItens = new Map(itens.map(item => [item.Id, item]));

    const update = Options.filter(item2 => {
        const item1 = mapItens.get(item2.Id);

        return item1 &&
            new Date(item2.DataCriacao) > new Date(item1.DataCriacao ?? new Date());
    });

    for (const item of remove) {
        await deleteAttributeOption(item.InternalId, InternalId, db);
    }

    for (const item of add) {
        await insertAttributeOption(userLogin, InternalId, item, db);
    }
    
    for (const item of update){
        await updateAttributeOption(item, db);
    }

    return attribute;
};

export const insertAttributeOption= async (userLogin: string, attributeInternalId: number, attributeOption: AttributeOption, db: SQLite.SQLiteDatabase) => {
    const {
        Id, Label, IsDefault, Status, DataCriacao, DataAlteracao
    } = attributeOption;

    const result = await db.executeSql(
        'INSERT INTO attribute_option ' +
        '( id' +
        ', label' +
        ', is_default ' +
        ', status' +
        ', attribute_id' +
        ', data_criacao' +
        ', data_alteracao' +
        ', reference' +
        ') VALUES (?, ?, ?, ?, ?, ?, ?, ?)',
        [   Id,
            Label,
            IsDefault,
            Status,
            attributeInternalId,
            DataCriacao,
            DataAlteracao,
            userLogin
        ]);
}

export const updateAttributeOption = async (attributeOption: AttributeOption, db: SQLite.SQLiteDatabase) => {
    const {
        Id, InternalId, Label, IsDefault, Status, DataCriacao, DataAlteracao
    } = attributeOption;

    await db.executeSql(
        'UPDATE categories '
        + 'SET id = ?'
        + ', label = ?'
        + ', is_default = ?'
        + ', status = ?'
        + ', data_criacao = ?'
        + ', data_alteracao = ?'
        + ', attribute_id = ?'
        + 'WHERE internal_id = ?',
        [ Id,
            Label,
            IsDefault,
            Status,
            DataCriacao,
            DataAlteracao,
            InternalId
        ]
    );
};

export const deleteAttributeOption = async (attributeOptionInternalId: number, attributeInternalId: number, db: SQLite.SQLiteDatabase) => {
    await db.executeSql('DELETE FROM attribute_option' +
        ' WHERE attribute_id = ?' +
        '   AND internal_id = ?',
        [attributeInternalId, attributeOptionInternalId]);
}

export const deleteInternalAttribute = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM attributes' +
        ' WHERE internal_id = ?'
        , [internalId]);
};

export const deleteInternalAttributeByExternalId = async (userLogin: string, id: number) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE FROM attributes' +
        ' WHERE reference = ?' +
        '   AND id = ?'
        , [userLogin, id]);
};

export const deleteAllAttributes = async (userLogin: string) => {
    const db = await openDatabase();
    await db.executeSql(
        'DELETE' +
        '  FROM attributes' +
        ' WHERE reference = ?', [userLogin]);
}

export const selectAllAttributes = async (userLogin: string, pageNumber: number | null, activated: number | null): Promise<Attribute[]> => {
    const db = await openDatabase();
    
    let results: ResultSet[];
    let query = 'SELECT *' + 
        '  FROM attributes' +
        ' WHERE reference = ?';
    
    let params = [];
    params.push(userLogin);
    
    if (activated !== null){
        query += ' AND status = ?';
        params.push(activated);
    }

    query += ' ORDER BY name';
    
    if (pageNumber){
        query += ' LIMIT ?' +
                 ' OFFSET ?';
        params.push(constants.pageSize);
        params.push((pageNumber - 1) * constants.pageSize);
    }
    
    results = await db.executeSql(query, params);
        
    const attributes: Attribute[] = [];
    for (const result of results){
        for (let i = 0; i < result.rows.length; i++) {
            let attribute = await formatResult(result.rows.item(i));

            attribute.Options = await selectOptionByAttributeInternalId(attribute.InternalId);
            attributes.push(attribute);
        }
    }

    return attributes;
};

export const selectContAllAttributes = async (userLogin: string): Promise<number> => {
    const db = await openDatabase();
    
    let query = 'SELECT * ' +
        '  FROM attributes ' +
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

export const selectAttributeById = async (userLogin: string, id: number): Promise<Attribute | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql(
        'SELECT * ' +
        '  FROM attributes ' +
        ' WHERE reference = ?' +
        '   AND id        = ?'
        , [userLogin, id]);

    let attribute = undefined;
    if (result[0]?.rows.length > 0) {
        attribute = formatResult(result[0]?.rows?.item(0));
        attribute.Options = await selectOptionByAttributeInternalId(attribute.InternalId);
    }
    
    return attribute;
}

const formatResult = (item: any): Attribute => {
    const attribute: Attribute = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Description: item.description,
        DataType: item.data_type,
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
    }

    return attribute;
}

export const selectOptionByAttributeInternalId = async (attributeInternalId: number): Promise<AttributeOption[]> => {
    const db = await openDatabase();

    const results = await db.executeSql(
        'SELECT * ' +
        '  FROM attribute_option' +
        ' WHERE attribute_id = ?'
        , [attributeInternalId,]);

    const attributeOptions: AttributeOption[] = [];
    for(const result of results) {
        for (let i = 0; i < result.rows.length; i++) {
            attributeOptions.push(formatResultOption(result.rows.item(i)));
        }
    }

    return attributeOptions;
}

const formatResultOption = (item: any): AttributeOption => {
    const option: AttributeOption = {
        InternalId: item.internal_id,
        Id: item.id,
        Label: item.label,
        IsDefault: item.is_default,
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
        tempId: ""
    };

    return option;
}