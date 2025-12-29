import React, {useEffect, useRef, useState} from 'react';
import {Alert, Text, TouchableOpacity, View} from 'react-native';
import {useFocusEffect} from '@react-navigation/native';
import _ from 'lodash';
import {TypesTransaction} from '../../enums/enums';
import * as I from '../../interfaces/interfaces';

import NavNextIcon from '../../assets/nav_next.svg';
import NavPrevIcon from '../../assets/nav_prev.svg';
import CurrencyExchangeIcon from '../../assets/currency_exchange.svg';
import {
    alterTransaction,
    excludeTransaction,
    executeRecurringTransaction,
    loadAllTransactionsInternal,
    loadAndPersistAll,
    loadTotalsTransactions
} from '../../controller/transaction.controller';
import CustomScroll from "../../components/CustomScroll";
import {validateLogin} from "../../utils.ts";
import {constants} from "../../constants";
import {constants as pageConstants} from "../../components/Page/constants";
import TransactionItem from "./TransactionItem";
import CashRegisterIcon from "../../assets/cash-register.svg";

import {useTheme} from '../../contexts/ThemeContext';
import {getStyle} from '../../styles/styles';
import {getTransactionStyle} from './styles';
import {PageProcess} from "../../components/Page";

const months = [
    'Janeiro',
    'Fevereiro',
    'Março',
    'Abril',
    'Maio',
    'Junho',
    'Julho',
    'Agosto',
    'Setembro',
    'Outubro',
    'Novembro',
    'Dezembro'
];

const Transaction = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const transactionStyle = getTransactionStyle(theme);

    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [transactions, setTransactions] = useState<I.Transaction[]>([]);
    const [transactionsGroup, setTransactionsGroup] = useState<I.TransactionsGroup[]>([]);
    const [transactionTotals, setTransactionTotals] = useState<I.TransactionTotals>({} as I.TransactionTotals);
    const [selectedYear, setSelectedYear] = useState(0);
    const [selectedMonth, setSelectedMonth] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [typeSelected, setTypeSelected] = useState(-1);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [isSelectionMode, setIsSelectionMode] = useState(false);
    const [showModalHelp, setShowModalHelp] = useState(false);

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setTransactions([]);
            }
        }, [route.params?.actionNavigation])
    );

    const appendTransactions = (data: I.Transaction[]) => {
        let transactionsNew = transactions;
        if (data.length > 0) {
            data.map((item, key) => {
                transactionsNew.push(item);
            });
            setTransactions(transactionsNew);
        }
    };

    const loadTransactions = async (page: number, loadTotals: boolean = true) => {
        setLoading(true);

        if (selectedYear != 0) {

            let mountDateInicio = new Date(selectedYear, selectedMonth, 5, 0, 0, 0);
            let mountDateFim = new Date(selectedYear, selectedMonth + 1, 4, 23, 59, 59);

            let responseTransactions = null;

            if (isLoadInternal) {
                responseTransactions = await loadAllTransactionsInternal(mountDateInicio, mountDateFim, page);
            } else {
                responseTransactions = await loadAndPersistAll(mountDateInicio, mountDateFim, page);
                validateLogin(responseTransactions, navigation);
            }

            setTotalPages(responseTransactions?.totalPages ?? 1);
            appendTransactions(responseTransactions?.data ?? []);

            if (loadTotals) {
                let responseTotals = await loadTotalsTransactions(mountDateInicio, mountDateFim, false);
                setTransactionTotals(responseTotals);
            }
        }
        setLoading(false);
        setIsLoadInternal(false);
    };

    useEffect(() => {
        let today = new Date();
        setDate(today);
    }, []);

    /*Se clicar várias vezes na troca de datas essa lógica faz com que não seja efetuado a busca em todas as trocas de 
    datas, o "debounce" faz com que aguarde para executar a função e se for chamada novamente enquanto o tempo não acabou
    cancela a chamada anterior e começa a aguardar novamente.*/
    const updateTransactions = _.debounce(() => {
        setTransactions([]);
    }, 500);

    useEffect(() => {
        updateTransactions();

        return () => updateTransactions.cancel();
    }, [selectedYear, selectedMonth]);

    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
        } else {
            if (transactions.length === 0) {
                setPageNumber(1);
                loadTransactions(1);
            } else {
                getTransactionsGroupByDate();
            }
        }
    }, [transactions])

    useEffect(() => {
        if (transactions?.length !== 0) {
            setIsLoadInternal(true);
            loadTransactions(pageNumber, false);
        }
    }, [pageNumber]);

    const getTransactionsGroupByDate = () => {
        let transactionsGroup: I.TransactionsGroup[] = [] as I.TransactionsGroup[];

        transactions.map((item) => {
            const formattedDate = new Date(item.DataCriacao).toISOString().split("T")[0];

            const index = transactionsGroup.findIndex(x => x.date === formattedDate);
            if (index >= 0) {
                transactionsGroup[index].transactions.push(item);
            } else {
                transactionsGroup.push({date: formattedDate, transactions: [item]});
            }
        });

        setTransactionsGroup(transactionsGroup);
    }

    const sorting = (transactions: I.Transaction[]): I.Transaction[] => {

        let unconsolidated = [...transactions.filter(x => !x.Consolidated ?? false).sort((a, b) => new Date(a.DataCriacao) - new Date(b.DataCriacao))] ?? [];

        let consolidated = [...transactions.filter(x => x.Consolidated).sort((a, b) => new Date(b.DataCriacao) - new Date(b.DataCriacao))] ?? [];

        return [...unconsolidated, ...consolidated];
    }

    const handleLeftDateClick = () => {
        let mountDate = new Date(selectedYear, selectedMonth, 1);
        mountDate.setMonth(mountDate.getMonth() - 1);
        setDate(mountDate);
    };

    const handleRightDateClick = () => {
        let mountDate = new Date(selectedYear, selectedMonth, 1);
        mountDate.setMonth(mountDate.getMonth() + 1);
        setDate(mountDate);
    };

    const handleRecurringAndInstallmentPaymentsClick = async () => {

        Alert.alert("Atenção!",
            "Serão geradas as transações recorrentes e as transações de parcelas para o mês atual. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let mountDateInicio = new Date(selectedYear, selectedMonth, 5, 0, 0, 0);
                        var response = await executeRecurringTransaction(mountDateInicio);

                        validateLogin(response, navigation);

                        setTransactions([]);
                    }
                }
            ],
            {cancelable: false}
        );
    };

    const handleTransactionItemClick = (data: I.Transaction) => {
        if (!isScrolling) {
            if (isSelectionMode)
                validateAndAddSelectedItem(data);
            else
                navigation.navigate("Transaction", {
                    screen: 'TransactionCreate',
                    params: {isEditing: true, data: data}
                });
        }
    }

    const handleTransactionLongItemClick = (data: I.Transaction) => {
        if (!isScrolling) {
            if (!isSelectionMode) {
                setTransactionTotals(prev => ({
                    ...prev,
                    Credit: 0,
                    Debit: 0
                }));
            }

            setIsSelectionMode(true);
            validateAndAddSelectedItem(data);
        }
    }

    const validateAndAddSelectedItem = (data: I.Transaction) => {

        data.IsSelectedItem = !data.IsSelectedItem;
        setTransactions((prevTransactions) =>
            prevTransactions.map((item) =>
                item.Id === data.Id ? data : item
            )
        );

        let valor = data.IsSelectedItem ? data.Value : data.Value * -1;

        setTransactionTotals(prev => ({
            ...prev,
            Credit: prev.Credit + (data.Operation.Type === constants.operationType.revenue.Id ? valor : 0),
            Debit: prev.Debit + (data.Operation.Type === constants.operationType.expense.Id ? valor : 0)
        }));
    }

    const onSwipeLeft = (data: I.Transaction) => {
        Alert.alert("Atenção!",
            "Esta transação terá o status alterado. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {

                        var prevtransactions = {...data};

                        data.Consolidated = !data.Consolidated;
                        let response = await alterTransaction(prevtransactions, data);
                        validateLogin(response, navigation);

                        //Atualiza a transação alterada para que fique certa na tela e não seja necessário recarregar
                        setTransactions((prevTransactions) =>
                            prevTransactions.map((item) =>
                                item.Id === data.Id ? data : item
                            )
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const onSwipeRight = (data: I.Transaction) => {
        Alert.alert("Atenção!",
            "Esta transação será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeTransaction(data);
                        validateLogin(response, navigation);

                        if (response.success) {
                            setIsLoadInternal(true);
                            setTransactions([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const handleNewClick = () => {
        navigation.navigate("Transaction", {
            screen: 'TransactionCreate',
            params: {isEditing: false, data: null}
        });
    }

    const handleDeselectClick = () => {
        setTransactions((prevTransactions) =>
            prevTransactions.map((item) => ({
                    ...item, IsSelectedItem: false
                })
            )
        );

        setTransactionTotals(prev => ({
            ...prev,
            Credit: prev?.CreditTotal ?? 0,
            Debit: prev?.DebitTotal ?? 0
        }));

        setIsSelectionMode(false);
    }

    const setDate = (date: Date) => {
        setSelectedYear(date.getFullYear());
        setSelectedMonth(date.getMonth());
    }

    return (
        <PageProcess
            headerType={pageConstants.headerType.process}
            bodyType={pageConstants.bodyType.process}
            title={"Transações"}
            helpType={"transaction"}
            iconTitle={<CashRegisterIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>}
            isSelectionMode={isSelectionMode}
            onDeselectClick={handleDeselectClick}
            onNewClick={handleNewClick}
            titleActions={
                <TouchableOpacity style={style.titleScreenMoreInfo}
                                  onPress={handleRecurringAndInstallmentPaymentsClick}>
                    <CurrencyExchangeIcon width="24" height="24" fill={theme.colors.primaryIcon}/>
                </TouchableOpacity>
            }
            headerContent={
                <View style={transactionStyle.viewSelectDate}>
                    <TouchableOpacity onPress={handleLeftDateClick} disabled={loading || isSelectionMode}
                                      style={transactionStyle.buttonPrev}>
                        <NavPrevIcon width="35" height="35" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                    <View style={transactionStyle.viewDateTitle}>
                        <Text style={transactionStyle.textDateTitle}>{months[selectedMonth]} {selectedYear}</Text>
                    </View>
                    <TouchableOpacity onPress={handleRightDateClick} disabled={loading || isSelectionMode}
                                      style={transactionStyle.buttonNext}>
                        <NavNextIcon width="35" height="35" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                </View>
            }>
            <>
                <View style={transactionStyle.viewTotais}>
                    <View style={transactionStyle.cardTotais}
                          onTouchEndCapture={() => setTypeSelected(TypesTransaction.Revenue)}>
                        <Text
                            style={[transactionStyle.textLabelTotais, transactionStyle.textLabelReceita]}>Receita</Text>
                        <Text
                            style={[transactionStyle.textTotais, transactionStyle.textReceita]}>R$ {transactionTotals?.Credit?.toFixed(2)}</Text>
                    </View>
                    <View style={transactionStyle.cardTotais}
                          onTouchEndCapture={() => setTypeSelected(TypesTransaction.Expense)}>
                        <Text
                            style={[transactionStyle.textLabelTotais, transactionStyle.textLabelDespesa]}>Despesa</Text>
                        <Text
                            style={[transactionStyle.textTotais, transactionStyle.textDespesa]}>R$ {transactionTotals?.Debit?.toFixed(2)}</Text>
                    </View>
                    <View style={transactionStyle.cardTotais}
                          onTouchEndCapture={() => setTypeSelected(-1)}>
                        <Text style={[transactionStyle.textLabelTotais, transactionStyle.textLabelSaldo]}>Saldo</Text>
                        <Text
                            style={[transactionStyle.textTotais, transactionStyle.textSaldo]}>R$ {((transactionTotals?.Credit ?? 0) - (transactionTotals?.Debit ?? 0)).toFixed(2)}</Text>
                        <Text
                            style={[transactionStyle.textPersentTotais, transactionStyle.textSaldo]}>{(transactionTotals?.Credit ? ((transactionTotals?.Credit ?? 0) - (transactionTotals?.Debit ?? 0)) * 100 / (transactionTotals?.Credit ?? 0) : 0).toFixed(2)}%</Text>
                    </View>
                </View>
                <CustomScroll
                    data={sorting(transactions).filter((item) => {
                        return typeSelected != -1 ? item.Operation.Type == typeSelected : item;
                    })}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    styles={transactionStyle.scroll}
                    renderItem={({item}) => (
                        <TransactionItem
                            data={item}
                            isSelectionMode={isSelectionMode}
                            onPress={handleTransactionItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}
                            onLongPress={handleTransactionLongItemClick}/>
                    )}
                />
            </>
        </PageProcess>
    );
}

export default Transaction;