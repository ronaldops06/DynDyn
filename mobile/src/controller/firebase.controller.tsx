import messaging from '@react-native-firebase/messaging';
import {postDevice} from "../services/user.api.ts";
import {Device, NotificationOperation} from "../interfaces/interfaces.tsx";
import DeviceInfo from "react-native-device-info";
import {constants} from "../constants";
import {processNotificationsCategory} from "./category.controller.tsx";
import {processNotificationsOperation} from "./operation.controller.tsx";
import {processNotificationsTransaction} from "./transaction.controller.tsx";
import {processNotificationsBalance} from "./balance.controller.tsx";
import {processNotificationsPortfolio} from "./portfolio.controller.tsx";

async function requestUserPermission() {
    const authStatus = await messaging().requestPermission({
        alert: true,
        badge: true,
        sound: true,
    });
    const enabled =
        authStatus === messaging.AuthorizationStatus.AUTHORIZED ||
        authStatus === messaging.AuthorizationStatus.PROVISIONAL;

    return enabled;
}

export const updateTokenCloudMessaging = async () => {
    requestUserPermission().then(async (granted) => {
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

    // Solicita permissão (iOS + Android 13+)
    await messaging().requestPermission();

    // Foreground
    const unsubscribeForeground = messaging().onMessage(async remoteMessage => {
        let body = JSON.parse(JSON.parse(remoteMessage.notification?.body ?? '')) as NotificationOperation;
        
        if (body && body.Reference === constants.operations.category) {
            await processNotificationsCategory(body.Operation, body.Id);
        } else if (body && body.Reference === constants.operations.operation) {
            await processNotificationsOperation(body.Reference, body.Id);
        } else if (body && body.Reference === constants.operations.portfolio) {
            await processNotificationsPortfolio(body.Reference, body.Id);
        } else if (body && body.Reference === constants.operations.balance) {
            await processNotificationsBalance(body.Reference, body.Id);
        } else if (body && body.Reference === constants.operations.transaction) {
            await processNotificationsTransaction(body.Reference, body.Id);
        }
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