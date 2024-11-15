import { NavigationContainer } from '@react-navigation/native';
import React, { useEffect } from 'react';

import UserContextProvider from './src/contexts/UserContext';
import { createTableAccounts } from './src/repository/account.repository';
import { createTableCategory } from './src/repository/category.repository';
import { createTableOperation } from './src/repository/operation.repository';
import { createTableSynchronization } from './src/repository/synchronization.repository';
import { createTableTransaction } from './src/repository/transaction.repository';
import MainStack from './src/stacks/MainStack';

const App = () => {

  useEffect(() => {
    createTableCategory();
    createTableAccounts();
    createTableOperation();
    createTableTransaction();
    createTableSynchronization();
  }, []);

  return (
    <UserContextProvider>
      <NavigationContainer>
        <MainStack />
      </NavigationContainer>
    </UserContextProvider>
  )
}

export default App;
