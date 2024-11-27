import {Account, Category} from "../interfaces/interfaces";
import { openDatabase } from "./database";
import {constants} from "../constants";
import {ResultSet} from "react-native-sqlite-storage";

export const createTableCategory = async () => {
    const db = await openDatabase();

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS categories (
        internal_id  INTEGER PRIMARY KEY AUTOINCREMENT,
        id           NUMBER,
        name         TEXT,
        type         NUMBER,
        status       NUMBER,
        data_criacao TEXT,
        data_alteracao TEXT
      );
    `);
};

export const insertCategory = async (category: Category): Promise<Category> => {
    
    const db = await openDatabase();
    const { Id,
            Name, 
            Type, 
            Status,
            DataCriacao, 
            DataAlteracao } = category;

    const result = await db.executeSql(
      'INSERT INTO categories '
                + '( id'
                + ', name'
                + ', type'
                + ', status'
                + ', data_criacao'
                + ', data_alteracao'
                + ') VALUES (?, ?, ?, ?, ?, ?)',
      [ Id,
        Name, 
        Type, 
        Status, 
        DataCriacao, 
        DataAlteracao ]
    );

    category.InternalId = result[0].insertId;

    return category;
};

export const updateCategory = async (category: Category) => {
    const db = await openDatabase();
    const { Id,
            Name, 
            Type, 
            Status,
            DataCriacao,
            DataAlteracao,
            InternalId
            } = category;

    await db.executeSql(
        'UPDATE categories '
        + 'SET id = ?'
            + ', name = ?'
            + ', type = ?'
            + ', status = ?'
            + ', data_criacao = ?'
            + ', data_alteracao = ?'
        + 'WHERE internal_id = ?',
        [ Id,
        Name, 
        Type, 
        Status,
        DataCriacao, 
        DataAlteracao,
        InternalId
        ]
    );
};

export const deleteCategory = async (internalId: number) => {
    const db = await openDatabase();
    await db.executeSql('DELETE FROM categories WHERE internal_id = ?', [internalId]);
};

export const selectAllCategories = async (type: number, pageNumber: number | null): Promise<Category[]> => {
    const db = await openDatabase();
    
    let results: ResultSet[];
    if (pageNumber)
        results = await db.executeSql('SELECT * FROM categories WHERE type = ? LIMIT ? OFFSET ?', [type, constants.pageSize, (pageNumber - 1) * constants.pageSize]);
    else
        results = await db.executeSql('SELECT * FROM categories WHERE type = ?', [type]);
    
    const categories: Category[] = [];
    results.forEach(result => {
        for (let i = 0; i < result.rows.length; i++) {
            categories.push(formatResult(result.rows.item(i)));
        }
    });

    return categories;
};

export const selectContAllCategories = async (type: number): Promise<number> => {
    const db = await openDatabase();

    const results = await db.executeSql('SELECT * FROM categories WHERE type = ?', [type]);

    let count: number = 0;
    results.forEach(result => {
        count += result.rows.length;
    });

    return count
};

export const selectCategoryById = async (id: number): Promise<Category | undefined> => {
    const db = await openDatabase();

    const result = await db.executeSql('SELECT * FROM categories WHERE id = ?', [id]);

    return result[0]?.rows.length > 0 ? formatResult(result[0]?.rows?.item(0)) : undefined;
}  

const formatResult = (item: any): Category => {
    const category: Category = {
        InternalId: item.internal_id,
        Id: item.id,
        Name: item.name,
        Type: item.type,
        Status: item.status,
        DataCriacao: item.data_criacao,
        DataAlteracao: item.data_alteracao,
    }

    return category;
}