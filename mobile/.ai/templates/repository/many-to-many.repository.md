# Template – N:N (padrão Operation + OperationRole + Link)

Use este template quando existe relacionamento muitos-para-muitos e quando o filho pode existir sem o pai.

Este padrão tem **dois repositórios**:

1. **Repositório Principal (Xxx)** → contém a tabela principal + a tabela de ligação
2. **Repositório do Filho (Yyy)** → é quase um repositório simples, mas com funções extras que buscam via tabela de ligação

---

## 1. Repositório Principal (Xxx) + Tabela de Ligação

```ts
import { Xxx, Yyy } from "../interfaces/interfaces";
import { openDatabase } from "./database";
import { constants } from "../constants";
import SQLite, { ResultSet } from "react-native-sqlite-storage";
import { selectYyyByXxxInternalId } from "./yyy.repository";

export const createTableXxx = async () => {
  const db = await openDatabase();

  // ===== TABELA PRINCIPAL =====
  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS xxx (
      internal_id     INTEGER PRIMARY KEY AUTOINCREMENT,
      id              NUMBER,
      name            TEXT,
      // ... outros campos
      status          NUMBER,
      data_criacao    TEXT,
      data_alteracao  TEXT,
      reference       TEXT
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_id ON xxx (id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_reference ON xxx (reference);`);

  // ===== TABELA DE LIGAÇÃO =====
  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS xxx_yyy_link (
      xxx_id      NUMBER,
      yyy_id      NUMBER,
      reference   TEXT,
      FOREIGN KEY (xxx_id) REFERENCES xxx(internal_id) ON DELETE CASCADE,
      FOREIGN KEY (yyy_id) REFERENCES yyy(internal_id) ON DELETE CASCADE
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_yyy_link_reference ON xxx_yyy_link (reference);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_yyy_link_xxx_id ON xxx_yyy_link (xxx_id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_yyy_link_yyy_id ON xxx_yyy_link (yyy_id);`);
};

export const insertXxx = async (userLogin: string, entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, /* ... */, Status, DataCriacao, DataAlteracao, YyyList } = entity;

  const result = await db.executeSql(
    `INSERT INTO xxx (id, name, /* ... */, status, data_criacao, data_alteracao, reference)
     VALUES (?, ?, /* ... */, ?, ?, ?, ?)`,
    [Id, Name, /* ... */, Status, DataCriacao, DataAlteracao, userLogin]
  );

  entity.InternalId = result[0].insertId;

  for (const item of YyyList ?? []) {
    await insertXxxYyyLink(userLogin, entity.InternalId, item, db);
  }

  return entity;
};

export const updateXxx = async (userLogin: string, entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, /* ... */, Status, DataCriacao, DataAlteracao, InternalId, YyyList } = entity;

  await db.executeSql(
    `UPDATE xxx SET id=?, name=?, /* ... */, status=?, data_criacao=?, data_alteracao=?
     WHERE internal_id = ?`,
    [Id, Name, /* ... */, Status, DataCriacao, DataAlteracao, InternalId]
  );

  const current = await selectYyyByXxxInternalId(InternalId);

  const remove = current.filter(curr =>
    !YyyList?.some(item => item.InternalId === curr.InternalId)
  );

  const add = YyyList?.filter(item =>
    !current.some(curr => curr.InternalId === item.InternalId)
  ) ?? [];

  for (const item of remove) {
    await deleteXxxYyyLink(item.InternalId, InternalId, db);
  }
  for (const item of add) {
    await insertXxxYyyLink(userLogin, InternalId, item, db);
  }

  return entity;
};

export const insertXxxYyyLink = async (
  userLogin: string,
  xxxInternalId: number,
  yyy: Yyy,
  db: SQLite.SQLiteDatabase
) => {
  await db.executeSql(
    `INSERT INTO xxx_yyy_link (xxx_id, yyy_id, reference) VALUES (?, ?, ?)`,
    [xxxInternalId, yyy.InternalId, userLogin]
  );
};

export const deleteXxxYyyLink = async (
  yyyInternalId: number,
  xxxInternalId: number,
  db: SQLite.SQLiteDatabase
) => {
  await db.executeSql(
    `DELETE FROM xxx_yyy_link WHERE xxx_id = ? AND yyy_id = ?`,
    [xxxInternalId, yyyInternalId]
  );
};

export const selectAllXxx = async (
  userLogin: string,
  pageNumber: number | null = null
): Promise<Xxx[]> => {
  const db = await openDatabase();
  let query = `SELECT * FROM xxx WHERE reference = ?`;
  const params: any[] = [userLogin];

  query += ` ORDER BY name`;

  if (pageNumber) {
    query += ` LIMIT ? OFFSET ?`;
    params.push(constants.pageSize, (pageNumber - 1) * constants.pageSize);
  }

  const results = await db.executeSql(query, params);
  const list: Xxx[] = [];

  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      const item = formatResult(result.rows.item(i));
      item.YyyList = await selectYyyByXxxInternalId(item.InternalId);
      list.push(item);
    }
  }
  return list;
};

export const selectXxxById = async (userLogin: string, id: number): Promise<Xxx | undefined> => {
  const db = await openDatabase();
  const result = await db.executeSql(
    `SELECT * FROM xxx WHERE reference = ? AND id = ?`,
    [userLogin, id]
  );

  if (result[0]?.rows.length > 0) {
    const item = formatResult(result[0].rows.item(0));
    item.YyyList = await selectYyyByXxxInternalId(item.InternalId);
    return item;
  }
  return undefined;
};

export const existsXxxRelationshipYyy = async (
  userLogin: string,
  yyyInternalId: number
): Promise<boolean> => {
  const db = await openDatabase();
  const result = await db.executeSql(
    `SELECT xxx.id
     FROM xxx
     INNER JOIN xxx_yyy_link link ON xxx.internal_id = link.xxx_id
     WHERE xxx.reference = ? AND link.yyy_id = ?
     LIMIT 1`,
    [userLogin, yyyInternalId]
  );
  return result[0]?.rows.length > 0;
};

// ... deleteInternalXxx, deleteInternalXxxByExternalId, deleteAllXxx, selectContAllXxx

const formatResult = (item: any): Xxx => ({
  InternalId: item.internal_id,
  Id: item.id,
  Name: item.name,
  // ...
  Status: item.status,
  DataCriacao: item.data_criacao,
  DataAlteracao: item.data_alteracao,
  YyyList: [],
});