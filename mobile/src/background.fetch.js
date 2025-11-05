import BackgroundFetch from 'react-native-background-fetch';
import {loadAllTrash} from "./controller/synchronization.controller";
import {log} from "./logger";

export async function initBackgroundFetch() {
    await BackgroundFetch.configure(
        {
            minimumFetchInterval: 15, // minutos
            stopOnTerminate: false,   // continua mesmo se o app for encerrado
            startOnBoot: true,        // inicia com o sistema
            enableHeadless: true,     // permite rodar sem interface
        },
        async (taskId) => {
            console.log('[BackgroundFetch] Rodando sincronização:', taskId);
            await loadAllTrash();
            BackgroundFetch.finish(taskId);
        },
        (error) => log('[BackgroundFetch] Erro:', error)
    );

    await BackgroundFetch.start();
}