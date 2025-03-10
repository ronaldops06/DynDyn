import * as I from '../interfaces/interfaces';
import {loadSynchronizationByCreationsDateAndOperation, setLastSynchronization} from "./synchronization.controller.tsx";
import {constants} from "../constants";
import Moment from "moment";
import {loadInternalAccount} from "./account.controller.tsx";
import {
    deleteInternalBalance,
    insertBalance,
    saveBalances,
    selectAllBalances,
    selectBalanceByBalanceMonthAndYear,
    selectContAllBalances,
    updateBalance
} from "../repository/balance.repository.tsx";
import {deleteBalance, getBalances, postBalance, putBalance} from "../services/balance.api.ts";
import {Alert} from "react-native";
import moment from "moment/moment";

export const loadAllBalanceInternal = async (pageNumber: Number | null): Promise<I.Response> => {
    let response = {} as I.Response;

    response.isLogged = true;
    response.data = await selectAllBalances(pageNumber as number);
    response.totalPages = await selectContAllBalances();

    return response;
}

export const loadAllBalance = async (pageNumber: Number | null): Promise<I.Response> => {
    let synchronization = await loadSynchronizationByCreationsDateAndOperation(null, null, constants.operations.balance);

    let params = `LastSyncDate=${Moment(synchronization.ExecutionDate).format('YYYY-MM-DD HH:mm:ss')}`;

    let response = await getBalances(params);

    if (response && !response.isLogged)
        return response;

    var balances = response?.data ?? [];
    
    //Armazena as constas em memória e só irá buscar no banco se não existir nela (array). Isso melhora a performance.
    var accounts = [] as I.Account[];
    for (const item of balances) {        
        if (!accounts.some(x => x.Id === item.Account.Id))
            accounts.push(await loadInternalAccount(item.Account));
        
        item.Account = accounts.find(x => x.Id === item.Account.Id);
    }

    await saveBalances(balances);
    
    await setLastSynchronization(synchronization);
    return response ?? {} as I.Response;
}

export const createBalance = async (balance: I.Balance): Promise<I.Response> => {
    let response = await postBalance(balance);

    if (response && !response.isLogged)
        return response;

    populateInternalFields(balance, response);

    if (!response.isConnected) {
        balance = await insertBalance(balance, null);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        balance = await insertBalance(response.data, null);
    }

    return response;
}

export const alterBalance = async (balance: I.Balance): Promise<I.Response> => {
    let response = await putBalance(balance);
    
    if (response && !response.isLogged)
        return response;

    populateInternalFields(balance, response);

    if (!response.isConnected) {
        balance = await updateBalance(balance, null);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data !== null) {
        balance = await updateBalance(response.data, null);
    }

    return response;
}

const populateInternalFields = (balance: I.Balance, response: I.Response) => {
    if (balance.InternalId)
        response.data.InternalId = balance.InternalId;

    if (balance.Account !== null)
        response.data.Account.InternalId = balance.Account.InternalId;
}

export const excludeBalance = async (balanceId: number, balanceInternalId: number): Promise<I.Response> => {
    let response = await deleteBalance(balanceId);

    if (response && !response.isLogged)
        return response;

    if (!response.isConnected) {
        await deleteInternalBalance(balanceInternalId);
        Alert.alert("Atenção!", "Sem conexão com a internet, os dados foram salvos e será feita uma nova tentativa de envio assim que a conexão for restabelecida.");
        //TO-DO: Guardar o registro em uma fila de envio
    } else if (response.data) {
        await deleteInternalBalance(balanceInternalId);
    }

    return response;
}

/**
 * Valida se algum dos campos alterados requer o recálculo do saldo
 * @param sourceTransaction - Objeto com a transação origem
 * @param destTransaction - Objeto com a transação destino (alterada)
 * @return - Resultado da validação: se "true" indica que é necessário refazer o cálculo; se "false" indica que não é necessário refazer o cálculo.
 */
export const validateNeedRecalculate = (sourceTransaction: I.Transaction | null, destTransaction: I.Transaction | null): boolean => {
    if (sourceTransaction === null || destTransaction === null)
        return true;
    
    if (sourceTransaction.Value !== destTransaction.Value)
        return true;
    
    if (sourceTransaction.Operation.Id !== destTransaction.Operation.Id)
        return true;
    
    if (sourceTransaction.Operation.Salary !== destTransaction.Operation.Salary)
        return true;

    if (sourceTransaction.Account.Id !== destTransaction.Account.Id)
        return true;

    if (sourceTransaction.DestinationAccount?.Id !== destTransaction.DestinationAccount?.Id)
        return true;

    if (sourceTransaction.DataCriacao !== destTransaction.DataCriacao)
        return true;
    
    return false;
}

export const calculateBalanceByTransactionFromUpdate = async (sourceTransaction: I.Transaction | null, destTransaction: I.Transaction | null) => {
    
    if (!validateNeedRecalculate(sourceTransaction, destTransaction))
        return;
    
    let executeSync = true;
    if (sourceTransaction !== null && destTransaction !== null)
        executeSync = false;
    
    if (sourceTransaction !== null) {
        sourceTransaction.Value = sourceTransaction.Value * -1;
        await calculateBalanceByTransaction(sourceTransaction, executeSync);
    }
    
    if (destTransaction !== null)
        await calculateBalanceByTransaction(destTransaction, true);
    
}

export const calculateBalanceByTransaction = async (transaction: I.Transaction, executeSync: boolean) => {

    let calculateBalance: I.CalculateBalance = {} as I.CalculateBalance;
    calculateBalance.Year = moment(transaction.DataCriacao).year();
    calculateBalance.Month = moment(transaction.DataCriacao).month();

    calculateBalance.OperationType = transaction.Operation.Type;
    calculateBalance.Salary = transaction.Operation.Salary;
    calculateBalance.Value = transaction.Value;

    if (transaction.Operation.Type === constants.operationType.revenue.Id) {
        calculateBalance.CalculateBalanceType = constants.calculateBalanceType.inflow;
        calculateBalance.Account = transaction.Account;
        await balanceCalculation(calculateBalance, executeSync);
        
    } else if (transaction.Operation.Type === constants.operationType.expense.Id) {
        calculateBalance.CalculateBalanceType = constants.calculateBalanceType.outflow;
        calculateBalance.Account = transaction.Account;
        await balanceCalculation(calculateBalance, executeSync);
        
    } else if (transaction.Operation.Type === constants.operationType.transfer.Id) {
        calculateBalance.CalculateBalanceType = constants.calculateBalanceType.outflow;
        calculateBalance.Account = transaction.Account;
        
        await balanceCalculation(calculateBalance, executeSync);

        calculateBalance.CalculateBalanceType = constants.calculateBalanceType.inflow;
        calculateBalance.Account = transaction.DestinationAccount ?? {} as I.Account;
        await balanceCalculation(calculateBalance, executeSync);
    }
    
}

export const balanceCalculation = async (calculateBalance: I.CalculateBalance, executeSync: boolean) => {
    var balance = await selectBalanceByBalanceMonthAndYear(calculateBalance.Account.InternalId, calculateBalance.Month, calculateBalance.Year);

    if (balance === undefined) {
        balance = {} as I.Balance;
        balance.Id = 0;
        balance.Value = 0;
        balance.Inflow = 0;
        balance.Outflow = 0;
        balance.SalaryCredit = 0;
        balance.SalaryDebit = 0;
        balance.Credit = 0;
        balance.Debit = 0;
        balance.Dividends = 0;
        balance.Income = 0;
        balance.Valuation = 0;
        balance.Month = calculateBalance.Month;
        balance.Year = calculateBalance.Year;
    }

    balance.Account = calculateBalance.Account;
    
    //Credit: Tudo que entra desconsiderando transferência e salário 
    //Debit: Tudo que sai desconsiderando transferência e salário 
    //SalaryCredit: Tudo que entra referente a salário
    //SalaryDebit: Tudo que sai referente a salário
    //Inflow: Tudo que entra, considerando salário, transferência e todo o resto
    //Outflow: Tudo que sai, considerando salário, transferência e todo o resto
    //Value: Quanto sobra, resultado do cálculo (Inflow - Outflow)
    //Valuation: 

    if (calculateBalance.CalculateBalanceType === constants.calculateBalanceType.inflow) {
        balance.Value += calculateBalance.Value;
        balance.Inflow += calculateBalance.Value;
        
        if (calculateBalance.Salary)
            balance.SalaryCredit += calculateBalance.Value;
        else if (calculateBalance.OperationType !== constants.operationType.transfer.Id)
            balance.Credit += calculateBalance.Value;
    } else if (calculateBalance.CalculateBalanceType === constants.calculateBalanceType.outflow) {
        balance.Value -= calculateBalance.Value;
        balance.Outflow += calculateBalance.Value;

        if (calculateBalance.Salary)
            balance.SalaryDebit += calculateBalance.Value;
        else if (calculateBalance.OperationType !== constants.operationType.transfer.Id)
            balance.Debit += calculateBalance.Value;
    }

    if (balance.InternalId === undefined ) {
        if (executeSync)
            await createBalance(balance);
        else
            await insertBalance(balance, null);
    } else {
        if (executeSync)
            await alterBalance(balance);
        else
            await updateBalance(balance, null);
    }
}