# Template – 1:N com dependência forte (padrão Attribute + AttributeOption)

Use este template quando o filho **não existe sem o pai**.

```ts
import { Xxx, XxxOption } from "../interfaces/interfaces";
import { openDatabase } from "./database";
import { constants } from "../constants";
import SQLite, { ResultSet } from "react-native-sqlite-storage";

export const createTableXxx = async () => {
  const db = await openDatabase();

  // ===== TABELA PAI =====
  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS xxx (
      internal_id     INTEGER PRIMARY KEY AUTOINCREMENT,
      id              NUMBER,
      name            TEXT,
      description     TEXT,
      // ... outros campos do pai
      status          NUMBER,
      data_criacao    TEXT,
      data_alteracao  TEXT,
      reference       TEXT
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_id ON xxx (id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_reference ON xxx (reference);`);

  // ===== TABELA FILHO =====
  await db.executeSql(`
    CREATE TABLE IF NOT EXISTS xxx_option (
      internal_id     INTEGER PRIMARY KEY AUTOINCREMENT,
      id              NUMBER,
      label           TEXT,
      is_default      NUMBER,
      status          NUMBER,
      xxx_id          NUMBER,          -- FK para internal_id do pai
      data_criacao    TEXT,
      data_alteracao  TEXT,
      reference       TEXT
    );
  `);

  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_option_id ON xxx_option (id);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_option_reference ON xxx_option (reference);`);
  await db.executeSql(`CREATE INDEX IF NOT EXISTS idx_xxx_option_xxx_id ON xxx_option (xxx_id);`);
};

export const insertXxx = async (userLogin: string, entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, Description, /* ... */, Status, DataCriacao, DataAlteracao, Options } = entity;

  const result = await db.executeSql(
    `INSERT INTO xxx
      (id, name, description, /* ... */, status, data_criacao, data_alteracao, reference)
     VALUES (?, ?, ?, /* ... */, ?, ?, ?, ?)`,
    [Id, Name, Description, /* ... */, Status, DataCriacao, DataAlteracao, userLogin]
  );

  entity.InternalId = result[0].insertId;

  // Insere todos os filhos
  for (const item of Options ?? []) {
    await insertXxxOption(userLogin, entity.InternalId, item, db);
  }

  return entity;
};

export const updateXxx = async (userLogin: string, entity: Xxx): Promise<Xxx> => {
  const db = await openDatabase();
  const { Id, Name, Description, /* ... */, Status, DataCriacao, DataAlteracao, InternalId, Options } = entity;

  await db.executeSql(
    `UPDATE xxx SET
      id = ?, name = ?, description = ?, /* ... */,
      status = ?, data_criacao = ?, data_alteracao = ?
     WHERE internal_id = ?`,
    [Id, Name, Description, /* ... */, Status, DataCriacao, DataAlteracao, InternalId]
  );

  // 1. Busca os filhos atuais
  const currentOptions = await selectOptionByXxxInternalId(InternalId);

  // 2. Calcula o que remover, adicionar e atualizar
  const remove = currentOptions.filter(curr => 
    !Options?.some(opt => opt.Id === curr.Id)
  );

  const add = Options?.filter(opt => 
    !currentOptions.some(curr => curr.Id === opt.Id)
  ) ?? [];

  const update = Options?.filter(opt => {
    const old = currentOptions.find(curr => curr.Id === opt.Id);
    return old && new Date(opt.DataAlteracao) > new Date(old.DataAlteracao ?? 0);
  }) ?? [];

  // 3. Executa as operações
  for (const item of remove) {
    await deleteXxxOption(item.InternalId, InternalId, db);
  }
  for (const item of add) {
    await insertXxxOption(userLogin, InternalId, item, db);
  }
  for (const item of update) {
    await updateXxxOption(item, db);
  }

  return entity;
};

export const insertXxxOption = async (
  userLogin: string,
  parentInternalId: number,
  option: XxxOption,
  db: SQLite.SQLiteDatabase
) => {
  const { Id, Label, IsDefault, Status, DataCriacao, DataAlteracao } = option;

  await db.executeSql(
    `INSERT INTO xxx_option
      (id, label, is_default, status, xxx_id, data_criacao, data_alteracao, reference)
     VALUES (?, ?, ?, ?, ?, ?, ?, ?)`,
    [Id, Label, IsDefault, Status, parentInternalId, DataCriacao, DataAlteracao, userLogin]
  );
};

export const updateXxxOption = async (
  option: XxxOption,
  db: SQLite.SQLiteDatabase
) => {
  const { Id, InternalId, Label, IsDefault, Status, DataCriacao, DataAlteracao } = option;

  await db.executeSql(
    `UPDATE xxx_option SET
      id = ?, label = ?, is_default = ?, status = ?,
      data_criacao = ?, data_alteracao = ?
     WHERE internal_id = ?`,
    [Id, Label, IsDefault, Status, DataCriacao, DataAlteracao, InternalId]
  );
};

export const deleteXxxOption = async (
  optionInternalId: number,
  parentInternalId: number,
  db: SQLite.SQLiteDatabase
) => {
  await db.executeSql(
    `DELETE FROM xxx_option WHERE xxx_id = ? AND internal_id = ?`,
    [parentInternalId, optionInternalId]
  );
};

export const selectOptionByXxxInternalId = async (parentInternalId: number): Promise<XxxOption[]> => {
  const db = await openDatabase();
  const results = await db.executeSql(
    `SELECT * FROM xxx_option WHERE xxx_id = ?`,
    [parentInternalId]
  );

  const options: XxxOption[] = [];
  for (const result of results) {
    for (let i = 0; i < result.rows.length; i++) {
      options.push(formatResultOption(result.rows.item(i)));
    }
  }
  return options;
};

export const selectAllXxx = async (
  userLogin: string,
  pageNumber: number | null = null,
  activated: number | null = null
): Promise<Xxx[]> => {
  const db = await openDatabase();
  let query = `SELECT * FROM xxx WHERE reference = ?`;
  const params: any[] = [userLogin];

  if (activated !== null) {
    query += ` AND status = ?`;
    params.push(activated);
  }

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
      item.Options = await selectOptionByXxxInternalId(item.InternalId);
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
    item.Options = await selectOptionByXxxInternalId(item.InternalId);
    return item;
  }
  return undefined;
};

// ... deleteInternalXxx, deleteInternalXxxByExternalId, deleteAllXxx, selectContAllXxx

const formatResult = (item: any): Xxx => ({
  InternalId: item.internal_id,
  Id: item.id,
  Name: item.name,
  Description: item.description,
  // ...
  Status: item.status,
  DataCriacao: item.data_criacao,
  DataAlteracao: item.data_alteracao,
  Options: [],
});

const formatResultXxx = (item: any): XxxXxx => ({
  InternalId: item.internal_id,
  Id: item.id,
  // ...
  Status: item.status,
  DataCriacao: item.data_criacao,
  DataAlteracao: item.data_alteracao,
});