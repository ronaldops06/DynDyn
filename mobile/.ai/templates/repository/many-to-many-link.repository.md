## 2. Repositório Filho(Xxx)

```ts
import { Yyy } from "../interfaces/interfaces";
import { openDatabase } from "./database";
import { constants } from "../constants";
import { ResultSet } from "react-native-sqlite-storage";

export const createTableYyy = async () => {
  const db = await openDatabase();

  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS yyy (
      internal_id     INTEGER PRIMARY KEY AUTOINCREMENT,
      id              NUMBER,
      name            TEXT,
      data_criacao    TEXT,
      data_alteracao  TEXT,
      reference       TEXT
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_yyy_id ON yyy (id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_yyy_reference ON yyy (reference);`);
};

export const insertYyy = async (userLogin: string, entity: Yyy): Promise<Yyy> => {
  const db = await openDatabase();
  const { Id, Name, DataCriacao, DataAlteracao } = entity;

  const result = await db.executeSql(
    `INSERT INTO yyy (id, name, data_criacao, data_alteracao, reference)
     VALUES (?, ?, ?, ?, ?)`,
    [Id, Name, DataCriacao, DataAlteracao, userLogin]
  );

  entity.InternalId = result[0].insertId;
  return entity;
};

export const updateYyy = async (entity: Yyy): Promise<Yyy> => {
  const db = await openDatabase();
  const { Id, Name, DataCriacao, DataAlteracao, InternalId } = entity;

  await db.executeSql(
    `UPDATE yyy SET id=?, name=?, data_criacao=?, data_alteracao=?
     WHERE internal_id = ?`,
    [Id, Name, DataCriacao, DataAlteracao, InternalId]
  );
  return entity;
};

/**
 * Busca todos os Yyy vinculados a um Xxx específico
 * (usada pelo repositório principal)
 */
export const selectYyyByXxxInternalId = async (xxxInternalId: number): Promise<Yyy[]> => {
  const db = await openDatabase();

  const results = await db.executeSql(
    `SELECT yyy.*
     FROM yyy
     INNER JOIN xxx_yyy_link link ON yyy.internal_id = link.yyy_id
     WHERE link.xxx_id = ?`,
    [xxxInternalId]
  );

  const list: Yyy[] = [];
  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      list.push(formatResult(result.rows.item(i)));
    }
  }
  return list;
};

/**
 * Exemplo de outra função de ligação (caso o Yyy se relacione com mais entidades)
 */
export const selectYyyByOutroInternalId = async (outroInternalId: number): Promise<Yyy[]> => {
  const db = await openDatabase();

  const results = await db.executeSql(
    `SELECT yyy.*
     FROM yyy
     INNER JOIN outro_yyy_link link ON yyy.internal_id = link.yyy_id
     WHERE link.outro_id = ?`,
    [outroInternalId]
  );

  const list: Yyy[] = [];
  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      list.push(formatResult(result.rows.item(i)));
    }
  }
  return list;
};

export const deleteInternalYyy = async (internalId: number) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM yyy WHERE internal_id = ?`, [internalId]);
};

export const deleteInternalYyyByExternalId = async (userLogin: string, id: number) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM yyy WHERE reference = ? AND id = ?`, [userLogin, id]);
};

export const deleteAllYyy = async (userLogin: string) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM yyy WHERE reference = ?`, [userLogin]);
};

export const selectAllYyy = async (
  userLogin: string,
  pageNumber: number | null = null
): Promise<Yyy[]> => {
  const db = await openDatabase();
  let query = `SELECT * FROM yyy WHERE reference = ?`;
  const params: any[] = [userLogin];

  query += ` ORDER BY name`;

  if (pageNumber) {
    query += ` LIMIT ? OFFSET ?`;
    params.push(constants.pageSize, (pageNumber - 1) * constants.pageSize);
  }

  const results = await db.executeSql(query, params);
  const list: Yyy[] = [];

  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      list.push(formatResult(result.rows.item(i)));
    }
  }
  return list;
};

export const selectYyyById = async (userLogin: string, id: number): Promise<Yyy | undefined> => {
  const db = await openDatabase();
  const result = await db.executeSql(
    `SELECT * FROM yyy WHERE reference = ? AND id = ?`,
    [userLogin, id]
  );
  return result[0]?.rows.length > 0 ? formatResult(result[0].rows.item(0)) : undefined;
};

export const selectContAllYyy = async (userLogin: string): Promise<number> => {
  const db = await openDatabase();
  const results = await db.executeSql(`SELECT * FROM yyy WHERE reference = ?`, [userLogin]);
  let count = 0;
  results.forEach(r => count += r.rows.length);
  return count;
};

const formatResult = (item: any): Yyy => ({
  InternalId: item.internal_id,
  Id: item.id,
  Name: item.name,
  DataCriacao: item.data_criacao,
  DataAlteracao: item.data_alteracao,
});
```