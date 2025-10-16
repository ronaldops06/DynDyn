import DeviceInfo from "react-native-device-info";
import notifee from "@notifee/react-native";
import messaging, {AuthorizationStatus} from "@react-native-firebase/messaging";

import {postDevice} from "../services/user.api.ts";
import {Device, NotificationOperation} from "../interfaces/interfaces.tsx";
import {constants} from "../constants";
import {processNotificationsCategory} from "./category.controller.tsx";
import {processNotificationsOperation} from "./operation.controller.tsx";
import {processNotificationsTransaction} from "./transaction.controller.tsx";
import {processNotificationsBalance} from "./balance.controller.tsx";
import {processNotificationsPortfolio} from "./portfolio.controller.tsx";

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

        if (remoteMessage.notification?.title === "Excluded Entity"){
            let body = JSON.parse(JSON.parse(remoteMessage.notification?.body ?? '')) as NotificationOperation;
            executeExcludeEntity(body);
        } else {
            await notifee.displayNotification({
                title: remoteMessage.notification?.title,
                body: remoteMessage.notification?.body,
                android: {
                    channelId: 'default',
                },
            });
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

const executeExcludeEntity = async (body: NotificationOperation) => {
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
};

export const createAndroidChannel = async() => {
    await notifee.createChannel({
        id: 'default',
        name: 'Notificações padrão',
    });
}
