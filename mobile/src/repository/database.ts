import SQLite, {ResultSet} from 'react-native-sqlite-storage';

SQLite.DEBUG(false);
SQLite.enablePromise(true);

const database_name: string = "sagemoney.db";

let db: SQLite.SQLiteDatabase;

export const openDatabase = async (): Promise<SQLite.SQLiteDatabase> => {
    if (db) {
        return db;
    }

    try {
        db = await SQLite.openDatabase({
            name: database_name,
            location: "default",
            
        });
        console.log("Banco de dados conectado com sucesso!");
        return db;
    } catch (error) {
        console.error("Erro ao conectar ao banco de dados", error);
        throw error;
    }
};

export const addColumnIfNotExists = async (db: SQLite.SQLiteDatabase, table: string, column: string, definition: string) => {
    const [result] = await db.executeSql(
        `PRAGMA table_info(${table})`
    );

    let exists = false;

    for (let i = 0; i < result.rows.length; i++) {
        const row = result.rows.item(i);

        if (row.name === column) {
            exists = true;
            break;
        }
    }

    if (!exists) {
        await db.executeSql(
            `ALTER TABLE ${table} ADD COLUMN ${column} ${definition}`
        );
    }
}