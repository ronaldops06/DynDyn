import DeviceInfo from "react-native-device-info";
import notifee from "@notifee/react-native";
import messaging, {AuthorizationStatus} from "@react-native-firebase/messaging";

import {postDevice} from "../services/user.api.ts";
import {Device} from "../interfaces/interfaces.tsx";

async function validateUserPermission() {
    const authStatus = await messaging().requestPermission();
    const enabled =
        authStatus === AuthorizationStatus.AUTHORIZED ||
        authStatus === AuthorizationStatus.PROVISIONAL;

    return enabled;
}

export const updateTokenCloudMessaging = async () => {
    validateUserPermission().then(async (granted) => {
        if (granted) {
            const token = await messaging().getToken();
            const uniqueId = await DeviceInfo.getUniqueId();
            
            var deviceDto = {} as Device;
            deviceDto.NotificationToken = token;
            deviceDto.PhisicalDeviceId = uniqueId;

            await postDevice(deviceDto);
        }
    });
}

export async function setupFirebaseListeners() {

    await notifee.requestPermission();

    // Foreground
    const unsubscribeForeground = messaging().onMessage(async remoteMessage => {

        await notifee.displayNotification({
            title: remoteMessage.notification?.title,
            body: remoteMessage.notification?.body,
            android: {
                channelId: 'default',
            },
        });
    });

    // Background (quando o usuário clica na notificação)
    const unsubscribeOpened = messaging().onNotificationOpenedApp(remoteMessage => {
        console.log('Usuário abriu a notificação:', remoteMessage);
    });

    // Quando o app é aberto a partir de uma notificação
    messaging().getInitialNotification().then(remoteMessage => {
        if (remoteMessage) {
            console.log('App aberto a partir de notificação:', remoteMessage);
        }
    });

    // Retorna função para limpar listeners
    return () => {
        unsubscribeForeground();
        unsubscribeOpened();
    };
}

export const createAndroidChannel = async() => {
    await notifee.createChannel({
        id: 'default',
        name: 'Notificações padrão',
    });
}
