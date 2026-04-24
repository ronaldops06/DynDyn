import notifee from '@notifee/react-native';
import BackgroundFetch from 'react-native-background-fetch';
import {loadAllTrash} from "../controller/synchronization.controller.tsx";
import {logInFile} from "../logger.tsx";

const createDefaultChannel = async () => {
    return notifee.createChannel({
        id: 'default',
        name: 'Notificações padrão',
    });
};

export const handleNotification = async (data: any) => {
    try {
        const channelId = await createDefaultChannel();

        await notifee.displayNotification({
            title: data?.title,
            body: data?.body,
            android: { channelId },
        });
    } catch (error) {
        await logInFile(error)
        console.error('Notification error', error);
    }
};

export const onMessageHandler = async (remoteMessage: any) => {
    await handleNotification(remoteMessage?.data);
};

export const backgroundMessageHandler = async (remoteMessage: any) => {
    await handleNotification(remoteMessage?.data);
};

export const backgroundFetchHandler = async (event) => {
    try {
        await loadAllTrash();
    } catch (error) {
        await logInFile(error)
    } finally {
        BackgroundFetch.finish(event.taskId);
    }
};
