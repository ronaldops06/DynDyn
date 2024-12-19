import {useNavigation} from '@react-navigation/core';
import {StackNavigationProp} from '@react-navigation/stack';
import React, {useEffect, useState} from 'react';
import {ActivityIndicator, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import _ from 'lodash';

import {CustomAlert} from '../../components/CustomAlert';
import TransactionItem from '../../components/TransactionItem';
import {TypesTransaction} from '../../enums/enums';
import * as I from '../../interfaces/interfaces';
import {RootStackParamList} from '../RootStackPrams';

import NavNextIcon from '../../assets/nav_next.svg';
import NavPrevIcon from '../../assets/nav_prev.svg';
import PlusIcon from '../../assets/plus.svg';
import {
    loadAllTransactionsInternal,
    loadAndPersistAll,
    loadTotalsTransactions
} from '../../controller/transaction.controller';
import {style} from '../../styles/styles';
import {transactionStyle} from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Transaction'>;

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

const Transaction = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [loading, setLoading] = useState(false);
    const [transactions, setTransactions] = useState<I.Transaction[]>([]);
    const [transactionTotals, setTransactionTotals] = useState<I.TransactionTotals>();
    const [selectedYear, setSelectedYear] = useState(0);
    const [selectedMonth, setSelectedMonth] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [typeSelected, setTypeSelected] = useState(-1);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);

    const appendTransactions = (data: I.Transaction[]) => {
        let transactionsNew = transactions;
        if (data.length > 0) {
            data.map((item, key) => {
                transactionsNew.push(item);
            });
            setTransactions(transactionsNew);
        }
    };

    const loadTransactions = async (loadTotals: boolean = true) => {
        setLoading(true);

        if (selectedYear != 0) {

            let mountDateInicio = new Date(selectedYear, selectedMonth, 5, 0, 0, 0);
            let mountDateFim = new Date(selectedYear, selectedMonth + 1, 4, 23, 59, 59);

            let responseTransactions = null;

            if (isLoadInternal)
                responseTransactions = await loadAllTransactionsInternal(mountDateInicio, mountDateFim, pageNumber);
            else
                responseTransactions = await loadAndPersistAll(mountDateInicio, mountDateFim, pageNumber, navigation);

            setTotalPages(responseTransactions?.totalPages ?? 1);
            appendTransactions(responseTransactions?.data ?? []);

            if (loadTotals) {
                let responseTotals = await loadTotalsTransactions(mountDateInicio, mountDateFim);
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

    useEffect(() => {
        // Executado no goBack da tela seguinte
        navigation.addListener('focus', () => {
            setIsLoadInternal(true);
            setTransactions([]);
        });
    }, [navigation]);
    
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
        if (transactions.length === 0) {
            setPageNumber(1);
            loadTransactions();
        }
    }, [transactions])

    useEffect(() => {
        setIsLoadInternal(true);
        loadTransactions(false);
    }, [pageNumber]);

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

    const handleTouchEnd = () => {
        setTimeout(setIsScrolling, 2000, false);
    }

    const handleTransactionItemClick = (data: I.Transaction) => {
        if (!isScrolling)
            navigation.navigate("TransactionCreate", {isEditing: true, data: data});
    }

    const onSwipeLeft = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta transação terá o status alterado. Deseja continuar?", console.log("implementar"));
    }

    const onSwipeRight = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta transação será excluída. Deseja continuar?", console.log("implementar"));
    }

    const handleNewClick = () => {
        navigation.navigate("TransactionCreate");
    }

    const setDate = (date: Date) => {
        setSelectedYear(date.getFullYear());
        setSelectedMonth(date.getMonth());
    }

    const reloadPage = () => {
        if (pageNumber <= totalPages) {
            setPageNumber(pageNumber + 1);
        }
    }

    function isEndScroll(event: any) {
        let mHeight = event.nativeEvent.layoutMeasurement.height;
        let cSize = event.nativeEvent.contentSize.height;
        let Y = event.nativeEvent.contentOffset.y;

        if (Math.ceil(mHeight + Y) >= cSize) return true;
        return false;
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsulta}>
                <View style={transactionStyle.viewSelectDate}>
                    <TouchableOpacity onPress={handleLeftDateClick} style={transactionStyle.buttonPrev}>
                        <NavPrevIcon width="35" height="35" fill="#F5F5F5"/>
                    </TouchableOpacity>
                    <View style={transactionStyle.viewDateTitle}>
                        <Text style={transactionStyle.textDateTitle}>{months[selectedMonth]} {selectedYear}</Text>
                    </View>
                    <TouchableOpacity onPress={handleRightDateClick} style={transactionStyle.buttonNext}>
                        <NavNextIcon width="35" height="35" fill="#F5F5F5"/>
                    </TouchableOpacity>
                </View>
            </View>
            <View style={style.viewBodyConsulta}>
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
                    </View>
                </View>
                <ScrollView style={transactionStyle.scroll}
                            onScroll={(event) => {
                                if (isEndScroll(event)) {
                                    if (!loading) {
                                        reloadPage();
                                    }
                                }
                            }}
                            onTouchStart={(event) => setIsScrolling(false)}
                            onTouchMove={(event) => setIsScrolling(true)}
                            onTouchEnd={(event) => handleTouchEnd}
                >
                    <View style={transactionStyle.viewList}>
                        {loading &&
                            <ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8"/>
                        }
                        {transactions != null && transactions.filter((item) => {
                            return typeSelected != -1 ? item.Operation.Type == typeSelected : item;
                        }).map((item, key) => (
                            <TransactionItem
                                key={key}
                                data={item}
                                onPress={handleTransactionItemClick}
                                onSwipeLeft={onSwipeLeft}
                                onSwipeRight={onSwipeRight}/>
                        ))
                        }
                    </View>
                </ScrollView>
                <TouchableOpacity
                    style={transactionStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill="#6E8BB8"/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Transaction;