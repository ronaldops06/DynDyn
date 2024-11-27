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
