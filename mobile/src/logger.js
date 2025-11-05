import AsyncStorage from '@react-native-async-storage/async-storage';

const STORAGE_KEY = 'sagemoney_logs';
const isDev = __DEV__; 

export async function log(message, data = null) {
    try {
        console.log('LOG', message);
        const timestamp = new Date().toISOString();
        const entry = { timestamp, message, data };
        
        if (isDev) {
            console.log(`[LOG - ${timestamp}]`, message, data ?? '');
        }
        
        const existing = JSON.parse(await AsyncStorage.getItem(STORAGE_KEY)) || [];
        existing.push(entry);

        // Mantém apenas os últimos 200 logs (evita lotar o storage)
        const trimmed = existing.slice(-200);

        await AsyncStorage.setItem(STORAGE_KEY, JSON.stringify(trimmed));
    } catch (error) {
        console.error('Erro ao salvar log:', error);
    }
}

export async function getLogs() {
    try {
        const logs = JSON.parse(await AsyncStorage.getItem(STORAGE_KEY)) || [];
        return logs;
    } catch (error) {
        console.error('Erro ao ler logs:', error);
        return [];
    }
}

export async function clearLogs() {
    try {
        await AsyncStorage.removeItem(STORAGE_KEY);
        if (isDev) console.log('[LOG] Logs limpos');
    } catch (error) {
        console.error('Erro ao limpar logs:', error);
    }
}
