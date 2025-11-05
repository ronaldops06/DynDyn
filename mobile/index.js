/**
 * @format
 */

import messaging from '@react-native-firebase/messaging';
import { AppRegistry } from 'react-native';
import App from './App';
import { name as appName } from './app.json';
import notifee from "@notifee/react-native";
import BackgroundFetch from 'react-native-background-fetch';
import {loadAllTrash} from "./src/controller/synchronization.controller";

const handleNotification = async (data) => {
    
    await notifee.createChannel({
        id: 'default',
        name: 'Notificações padrão',
    });

    await notifee.displayNotification({
        title: data.title,
        body: data.body,
        android: {
            channelId: 'default',
        },
    });
}

messaging().onMessage(async remoteMessage => {
    await handleNotification(remoteMessage?.data);
});

messaging().setBackgroundMessageHandler(async remoteMessage => {
    await handleNotification(remoteMessage?.data);
});

BackgroundFetch.registerHeadlessTask(async (event) => {
    console.log('[HeadlessTask] Rodando sincronização em background...');
    await loadAllTrash();
    BackgroundFetch.finish(event.taskId);
});

AppRegistry.registerComponent(appName, () => App);
