# Template – Repositório Simples

```ts
import { Xxx } from "../interfaces/interfaces";
import { openDatabase } from "./database";
import { constants } from "../constants";
import { ResultSet } from "react-native-sqlite-storage";

export const createTableXxx = async () => {
  const db = await openDatabase();

  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS xxx (
      internal_id     INTEGER PRIMARY KEY AUTOINCREMENT,
      id              NUMBER,
      name            TEXT,
      // outros campos...
      status          NUMBER,
      data_criacao    TEXT,
      data_alteracao  TEXT,
      reference       TEXT
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_id ON xxx (id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_reference ON xxx (reference);`);
};

export const insertXxx = async (userLogin: string, entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, /* ... */, DataCriacao, DataAlteracao } = entity;

  const result = await db.executeSql(
    `INSERT INTO xxx (id, name, /* ... */, data_criacao, data_alteracao, reference)
     VALUES (?, ?, /* ... */, ?, ?, ?)`,
    [Id, Name, /* ... */, DataCriacao, DataAlteracao, userLogin]
  );

  entity.InternalId = result[0].insertId;
  return entity;
};

export const updateXxx = async (entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, /* ... */, DataCriacao, DataAlteracao, InternalId } = entity;

  await db.executeSql(
    `UPDATE xxx SET id=?, name=?, /* ... */, data_criacao=?, data_alteracao=?
     WHERE internal_id = ?`,
    [Id, Name, /* ... */, DataCriacao, DataAlteracao, InternalId]
  );
  return entity;
};

export const deleteInternalXxx = async (internalId: number) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM xxx WHERE internal_id = ?`, [internalId]);
};

export const deleteInternalXxxByExternalId = async (userLogin: string, id: number) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM xxx WHERE reference = ? AND id = ?`, [userLogin, id]);
};

export const deleteAllXxx = async (userLogin: string) => {
  const db = await openDatabase();
  await db.executeSql(`DELETE FROM xxx WHERE reference = ?`, [userLogin]);
};

export const selectAllXxx = async (
  userLogin: string,
  pageNumber: number | null = null,
  // outros filtros...
): Promise<Xxx[]> => {
  const db = await openDatabase();
  let query = `SELECT * FROM xxx WHERE reference = ?`;
  const params: any[] = [userLogin];

  // filtros opcionais aqui...

  query += ` ORDER BY name`;

  if (pageNumber) {
    query += ` LIMIT ? OFFSET ?`;
    params.push(constants.pageSize, (pageNumber - 1) * constants.pageSize);
  }

  const results = await db.executeSql(query, params);
  const list: Xxx[] = [];

  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      list.push(formatResult(result.rows.item(i)));
    }
  }
  return list;
};

export const selectContAllXxx = async (userLogin: string): Promise<number> => {
  const db = await openDatabase();
  const results = await db.executeSql(`SELECT * FROM xxx WHERE reference = ?`, [userLogin]);
  let count = 0;
  results.forEach(r => count += r.rows.length);
  return count;
};

export const selectXxxById = async (userLogin: string, id: number): Promise<Xxx | undefined> => {
  const db = await openDatabase();
  const result = await db.executeSql(
    `SELECT * FROM xxx WHERE reference = ? AND id = ?`,
    [userLogin, id]
  );
  return result[0]?.rows.length > 0 ? formatResult(result[0].rows.item(0)) : undefined;
};

const formatResult = (item: any): Xxx => ({
  InternalId: item.internal_id,
  Id: item.id,
  Name: item.name,
  // ...
  DataCriacao: item.data_criacao,
  DataAlteracao: item.data_alteracao,
});