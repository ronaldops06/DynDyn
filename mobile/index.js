/**
 * @format
 */

import messaging from '@react-native-firebase/messaging';
import {AppRegistry} from 'react-native';
import App from './App';
import {name as appName} from './app.json';
import BackgroundFetch from 'react-native-background-fetch';

import {backgroundFetchHandler, backgroundMessageHandler, onMessageHandler,} from './src/background/handlers';

messaging().onMessage(onMessageHandler);
messaging().setBackgroundMessageHandler(backgroundMessageHandler);

BackgroundFetch.registerHeadlessTask(backgroundFetchHandler);

AppRegistry.registerComponent(appName, () => App);
