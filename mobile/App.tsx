import { NavigationContainer } from '@react-navigation/native';
import React, { useEffect } from 'react';

import UserContextProvider from './src/contexts/UserContext';
import {ThemeContextProvider} from './src/contexts/ThemeContext';
import { createTablePortfolios } from './src/repository/portfolio.repository';
import { createTableCategory } from './src/repository/category.repository';
import { createTableOperation } from './src/repository/operation.repository';
import { createTableSynchronization } from './src/repository/synchronization.repository';
import { createTableTransaction } from './src/repository/transaction.repository';
import MainStack from './src/stacks/MainStack';
import {createTableBalance} from "./src/repository/balance.repository.tsx";

const App = () => {
  
  useEffect(() => {
    createTableCategory();
    createTablePortfolios();
    createTableOperation();
    createTableTransaction();
    createTableBalance();
    createTableSynchronization();
  }, []);

  return (
    <UserContextProvider>
      <NavigationContainer>
        <ThemeContextProvider>
          <MainStack />
        </ThemeContextProvider>
      </NavigationContainer>
    </UserContextProvider>
  )
}

export default App;
