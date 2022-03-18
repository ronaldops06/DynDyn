import React, { useState, useEffect } from 'react';
import { Alert, SafeAreaView, View, ScrollView, ActivityIndicator, Text, TouchableOpacity } from 'react-native';
import { StackNavigationProp } from '@react-navigation/stack';
import { useNavigation } from '@react-navigation/core';
import AsyncStorage from '@react-native-community/async-storage';
import Moment from 'moment';

import { RootStackParamList } from '../RootStackPrams';
import api from '../../services/api';
import * as I from '../../interfaces/interfaces';
import { TypesTransaction } from '../../enums/enums';
import TransactionItem from '../../components/TransactionItem';

import NavPrevIcon from '../../assets/nav_prev.svg';
import NavNextIcon from '../../assets/nav_next.svg';
import PlusIcon from '../../assets/plus.svg';
import { style } from '../../styles/styles';
import { transactionStyle } from './styles';
import { transactionItemStyle } from '../../components/TransactionItem/styles';

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
    
    const [ loading, setLoading ] = useState(false);
    const [ transactions, setTransactions ] = useState<I.Transaction[]>([]);
    const [ transactionTotals, setTransactionTotals ] = useState<I.TransactionTotals>();
    const [ selectedYear, setSelectedYear ] = useState(0);
    const [ selectedMonth, setSelectedMonth ] = useState(0);
    const [ pageNumber, setPageNumber ] = useState(1);
    const [ totalPages, setTotalPages ] = useState(1);
    const [ typeSelected, setTypeSelected ] = useState(-1);    
    
    const appendTransactions = (data: I.Transaction[]) => {
        let transactionsNew = transactions;
        if (data.length > 0){
            data.map((item, key) => {
                transactionsNew.push(item);
            });
            setTransactions(transactionsNew);
        }
    };

    const getTransactions = async () => {
        setLoading(true);
        
        if (selectedYear != 0 ) {
            const token = await AsyncStorage.getItem('token');

            let mountDateInicio = new Date(selectedYear, selectedMonth, 5, 0, 0, 0);
            let mountDateFim= new Date(selectedYear, selectedMonth + 1, 4, 23, 59, 59);
            let params = `dataCriacaoInicio=${Moment(mountDateInicio, 'YYYY-MM-DD').format()}&dataCriacaoFim=${Moment(mountDateFim, 'YYYY-MM-DD').format()}&pageNumber=${pageNumber}`;
            
            await api.get(`Movimento?${params}`, {
                headers: { 'Authorization': 'Bearer ' + token ?? ""}
            }).then(response => {
                var pagination = response.headers.pagination;
                var totalPages = pagination.split('totalPages":')[1].replace('}','');
                
                setTotalPages(Number(totalPages));
                appendTransactions(response.data);                
            }).catch((error) => {
                if (error.response.status == 401){
                    navigation.navigate("SignIn");
                } else {
                    Alert.alert(error.response.data);
                }
            });
        }
        setLoading(false);
    };

    const getTransactionsTotals = async () => {
        setLoading(true);
        
        if (selectedYear != 0 ) {
            const token = await AsyncStorage.getItem('token');

            let mountDateInicio = new Date(selectedYear, selectedMonth, 5, 0, 0, 0);
            let mountDateFim= new Date(selectedYear, selectedMonth + 1, 4, 23, 59, 59);
            let params = `dataCriacaoInicio=${Moment(mountDateInicio, 'YYYY-MM-DD').format()}&dataCriacaoFim=${Moment(mountDateFim, 'YYYY-MM-DD').format()}`;
            
            await api.get(`Movimento/Totais?${params}`, {
                headers: { 'Authorization': 'Bearer ' + token ?? ""}
            }).then(response => {
                setTransactionTotals(response.data);             
            }).catch((error) => {
                if (error.response.status == 401){
                    navigation.navigate("SignIn");
                } else {
                    Alert.alert(error.response.data);
                }
            });
        }
        setLoading(false);
    };

    useEffect(() => {
        let today = new Date();
        setDate(today);
    },[]);

    useEffect(() => {
        // Executado no goBack da tela seguinte
        navigation.addListener('focus', () => {
            setTransactions([]);
            setPageNumber(1);
            getTransactions();
            getTransactionsTotals();
        });
    }, [navigation]);

    useEffect(() => {
        setTransactions([]);
        setPageNumber(1);
        getTransactions();
        getTransactionsTotals();
    }, [selectedYear, selectedMonth]);

    const handleLeftDateClick = () => {
        let mountDate = new Date(selectedYear, selectedMonth, 1);
        mountDate.setMonth( mountDate.getMonth() - 1 );
        setDate(mountDate);
    }

    const handleRightDateClick = () => {
        let mountDate = new Date(selectedYear, selectedMonth, 1);
        mountDate.setMonth( mountDate.getMonth() + 1 );
        setDate(mountDate);
    }

    const handleTransactionItemClick = (data: I.Transaction) => {
        navigation.navigate("TransactionCreate", {isEditing: true, data: data});
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
                            onPress: () => {console.log("implementar")}
                        }
                    ],
                    { cancelable: false }
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
                            onPress: () => {console.log("implementar")}
                        }
                    ],
                    { cancelable: false }
                );
    }

    const handleNewClick = () => {
        navigation.navigate("TransactionCreate");
    }

    const setDate = (date: Date) => {
        setSelectedYear( date.getFullYear() );
        setSelectedMonth( date.getMonth() );
    }

    const reloadPage = () => {
        if (pageNumber <= totalPages){
            setPageNumber(pageNumber + 1);
            getTransactions();
        }
    }

    function isEndScroll(event: any) {
        let mHeight = event.nativeEvent.layoutMeasurement.height;
        let cSize = event.nativeEvent.contentSize.height;
        let Y = event.nativeEvent.contentOffset.y;
 
        if (Math.ceil(mHeight + Y) >= cSize) return true;
        return false;
    }
    
    return(
        <SafeAreaView style={[style.container,style.containerConsulta]}>
            <View style={style.viewHeaderConsulta} >
                <View style={transactionStyle.viewSelectDate}>
                    <TouchableOpacity onPress={handleLeftDateClick} style={transactionStyle.buttonPrev}>
                        <NavPrevIcon width="35" height="35" fill="#F5F5F5" />
                    </TouchableOpacity>
                    <View style={transactionStyle.viewDateTitle}>
                        <Text style={transactionStyle.textDateTitle}>{months[selectedMonth]} {selectedYear}</Text>
                    </View>
                    <TouchableOpacity onPress={handleRightDateClick} style={transactionStyle.buttonNext}>
                        <NavNextIcon width="35" height="35" fill="#F5F5F5" />
                    </TouchableOpacity>
                </View>
            </View>
            <View style={style.viewBodyConsulta}>
                <View style={transactionStyle.viewTotais}>
                    <View style={transactionStyle.cardTotais}
                        onTouchEndCapture={() => setTypeSelected(TypesTransaction.Revenue)}>
                        <Text style={[transactionStyle.textLabelTotais, transactionStyle.textLabelReceita]}>Receita</Text>
                        <Text style={[transactionStyle.textTotais, transactionStyle.textReceita]}>R$ {transactionTotals?.credito.toFixed(2)}</Text>
                    </View>
                    <View style={transactionStyle.cardTotais}
                        onTouchEndCapture={() => setTypeSelected(TypesTransaction.Expense)}>
                        <Text style={[transactionStyle.textLabelTotais, transactionStyle.textLabelDespesa]}>Despesa</Text>
                        <Text style={[transactionStyle.textTotais, transactionStyle.textDespesa]}>R$ {transactionTotals?.debito.toFixed(2)}</Text>
                    </View>
                    <View style={transactionStyle.cardTotais}
                        onTouchEndCapture={() => setTypeSelected(-1)}>
                        <Text style={[transactionStyle.textLabelTotais, transactionStyle.textLabelSaldo]}>Saldo</Text>
                        <Text style={[transactionStyle.textTotais,transactionStyle.textSaldo]}>R$ {((transactionTotals?.credito ?? 0) - (transactionTotals?.debito ?? 0)).toFixed(2)}</Text>
                    </View>
                </View>
                <ScrollView style={transactionStyle.scroll} 
                    onScroll={(event) => {
                        if (isEndScroll(event)) {
                            if (!loading){
                                reloadPage();
                            }
                        }
                    }}>
                    <View style={transactionStyle.viewList}>
                        {loading && 
                            <ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8" />
                        }
                        {transactions != null && transactions.filter((item) => {
                                                    return typeSelected != -1 ? item.operacao.tipo == typeSelected : item;
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
                    <PlusIcon width="35" height="35" fill="#6E8BB8" />
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Transaction;