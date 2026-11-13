import React, {useState} from 'react';
import {SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import Help from "../../components/Help";
import BottomModal from "../../components/BottomModal";
import {useFocusEffect} from "@react-navigation/native";
import MonthYearSelector from "../../components/MonthYearPicker";
import PrevIcon from "../../assets/nav_prev.svg";
import HelpIcon from "../../assets/help_outline.svg";

import * as I from "../../interfaces/interfaces"
import {constants} from "../../constants";

import {loadDashboardBalanceGroupByMonth} from '../../controller/balance.controller'
import {loadDashboardTransactionFromCategory} from "../../controller/transaction.controller";
import WidgetLine from "./WidgetLine";
import WidgetBar from "./WidgetBar";

import {useTheme} from "../../contexts/ThemeContext";
import {getStyle} from "../../styles/styles";
import {getStyleCadastro} from "../../styles/styles.cadastro";
import {getDashboardStyle} from "./styles";

const Dashboard = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const dashboardStyle = getDashboardStyle(theme);

    const [showModalHelp, setShowModalHelp] = useState(false);
    const [balancesPeriod, setBalancesPeriod] = useState<I.DashboardItem[]>([]);
    const [valueCategoryExpense, setValueCategoryExpense] = useState<I.DashboardItem[]>([]);
    const [dateCategoryExpense, setDateCategoryExpense] = useState<Date>(null);
    const [initialDateBalance, setInitialDateBalance] = useState<Date>(null);
    const [endDateSaldo, setEndDateSaldo] = useState<Date>(null);

    useFocusEffect(
        React.useCallback(() => {
            const fetchData = async () => {
                await loadData();
            }
            
            fetchData();
        }, [])
    );

    const handleBackClick = () => {
        navigation.goBack();
    };

    const loadData = async () => {
        if (initialDateBalance === null) {
            const dataAnterior = new Date();
            dataAnterior.setMonth(dataAnterior.getMonth() - 6);
            setInitialDateBalance(dataAnterior);

            let responseBalances = await loadDashboardBalanceGroupByMonth(dataAnterior.getFullYear(), dataAnterior.getMonth());
            setBalancesPeriod(responseBalances);
        }
        
        if (dateCategoryExpense === null) {
            let date = new Date();
            setDateCategoryExpense(date);
            
            let mountDateInicio = new Date(date.getFullYear(), date.getMonth(), 5, 0, 0, 0);
            let mountDateFim = new Date(date.getFullYear(), date.getMonth() + 1, 4, 23, 59, 59);
                        
            let responseTransactions = await loadDashboardTransactionFromCategory(mountDateInicio, mountDateFim, constants.operationType.expense.Id);
            setValueCategoryExpense(responseTransactions);
        }
    }

    const setMonthAndYearInDate = (date: Date, month: number, year: number) => {
        return new Date(year, month, date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds());
    }
        
    const onMonthInitialDateBalance = (month: number) => {
        setInitialDateBalance(setMonthAndYearInDate(initialDateBalance, month, initialDateBalance.getFullYear()));
    }
    
    const onYearInitialDateBalance = (year: number) => {
        setInitialDateBalance(setMonthAndYearInDate(initialDateBalance, initialDateBalance.getMonth(), year));
    }

    const handleApplyFilterBalance = async () => {
        let responseBalances = await loadDashboardBalanceGroupByMonth(initialDateBalance.getFullYear(), initialDateBalance.getMonth());
        setBalancesPeriod(responseBalances);
    }

    const onMonthDateCategoryExpense = (month: number) => {
        setDateCategoryExpense(setMonthAndYearInDate(dateCategoryExpense, month, dateCategoryExpense.getFullYear()));
    }

    const onYearDateCategoryExpense = (year: number) => {
        setDateCategoryExpense(setMonthAndYearInDate(dateCategoryExpense, dateCategoryExpense.getMonth(), year));
    }

    const handleApplyFilterCategoryExpense = async () => {
        let mountDateInicio = new Date(dateCategoryExpense.getFullYear(), dateCategoryExpense.getMonth(), 5, 0, 0, 0);
        let mountDateFim = new Date(dateCategoryExpense.getFullYear(), dateCategoryExpense.getMonth() + 1, 4, 23, 59, 59);

        let responseTransactions = await loadDashboardTransactionFromCategory(mountDateInicio, mountDateFim, constants.operationType.expense.Id);
        setValueCategoryExpense(responseTransactions);
    }
            
    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                    <View style={style.headerScreenActions}>
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => setShowModalHelp(true)}>
                            <HelpIcon width="25" height="25" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                    </View>
                </View>
                <View style={[styleCadastro.viewBodyCadastro, dashboardStyle.areaBody]}>
                    <Text style={[style.textPrimary18, dashboardStyle.textInfoBlock]}>Acompanhamento de saldos</Text>
                    <WidgetLine
                        description="Saldos"
                        data={balancesPeriod}
                        renderFilters={
                            <MonthYearSelector
                                text="Período Inicial"
                                month={initialDateBalance?.getMonth() ?? 0}
                                year={initialDateBalance?.getFullYear() ?? 2000}
                                onMonthChange={onMonthInitialDateBalance}
                                onYearChange={onYearInitialDateBalance}
                            />
                        }
                        onCloseFilters={handleApplyFilterBalance}
                    />
                    <WidgetBar 
                        description="Despesas por categoria" 
                        data={valueCategoryExpense}
                        renderFilters={
                            <MonthYearSelector
                                text="Mês de Referência"
                                month={dateCategoryExpense?.getMonth() ?? 0}
                                year={dateCategoryExpense?.getFullYear() ?? 2000}
                                onMonthChange={onMonthDateCategoryExpense}
                                onYearChange={onYearDateCategoryExpense}
                            />
                        }
                        onCloseFilters={handleApplyFilterCategoryExpense}
                    />

                </View>
                <BottomModal show={showModalHelp} setShow={setShowModalHelp}>
                    <Help helpType="dashboard"/>
                </BottomModal>
            </ScrollView>
        </SafeAreaView>
    );
}

export default Dashboard;