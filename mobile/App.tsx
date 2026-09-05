import React, { useEffect } from 'react';

import {ErrorBoundary} from "./src/components/ErrorBoundary";
import UserContextProvider from './src/contexts/UserContext';
import { NavigationContainer } from '@react-navigation/native';
import {ThemeContextProvider} from './src/contexts/ThemeContext';
import MainStack from './src/stacks/MainStack';
import { createTablePortfolios } from './src/repository/portfolio.repository';
import { createTableCategory } from './src/repository/category.repository';
import { createTableAttribute } from './src/repository/attribute.repository';
import { createTableOperation } from './src/repository/operation.repository';
import { createTableSynchronization } from './src/repository/synchronization.repository';
import { createTableTransaction } from './src/repository/transaction.repository';
import { createTableBalance } from "./src/repository/balance.repository";
import {createAndroidChannel, setupFirebaseListeners} from "./src/controller/firebase.controller";
import {initBackgroundFetch} from "./src/background/background.fetch";
import crashlytics from '@react-native-firebase/crashlytics';
import {createTableOperationRole} from "./src/repository/operation.role.repository";
import {createTableTotalizerRole} from "./src/repository/totalizer.role.repository";

const App = () => {
  
  useEffect(() => {

    let isMounted = true;
    
    const initializeApp = async () => {
      try {
        await crashlytics().setCrashlyticsCollectionEnabled(!__DEV__);

        const defaultHandler = ErrorUtils.getGlobalHandler?.();
        ErrorUtils.setGlobalHandler((error, isFatal) => {
          crashlytics().recordError(error);
          defaultHandler?.(error, isFatal);
        });

        const cleanup = await setupFirebaseListeners();

        // 🔥 Banco em sequência
        await createTableCategory();
        await createTableAttribute();
        await createTablePortfolios();
        await createTableOperationRole();
        await createTableOperation();
        await createTableTransaction();
        await createTableBalance();
        await createTableTotalizerRole();
        await createTableSynchronization();

        // 🔥 Paralelo
        await Promise.all([
          createAndroidChannel(),
          initBackgroundFetch(),
        ]);

        return cleanup;

      } catch (error) {
        crashlytics().recordError(error);
      }
    };

    let cleanupFn: any;

    initializeApp().then((cleanup) => {
      if (isMounted && typeof cleanup === 'function') {
        cleanupFn = cleanup;
      }
    });

    return () => {
      isMounted = false;
      if (typeof cleanupFn === 'function') {
        cleanupFn();
      }
    };

  }, []);
  
  return (
      <ErrorBoundary>
        <UserContextProvider>
          <NavigationContainer>
            <ThemeContextProvider>
              <MainStack />
            </ThemeContextProvider>
          </NavigationContainer>
        </UserContextProvider>
      </ErrorBoundary>
  )
}

export default App;
