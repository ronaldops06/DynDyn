import React, {useEffect, useState} from "react";
import {ActivityIndicator, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from "react-native";
import {DashboardItem, Response, User} from "../../interfaces/interfaces.tsx"
import {VictoryAxis, VictoryChart, VictoryLine, VictoryTheme} from 'victory-native';

import UserIcon from "../../assets/user.svg";
import VisibilityIcon from "../../assets/visibility.svg";
import VisibilityOffIcon from "../../assets/visibility_off.svg";
import AccountIcon from '../../assets/account.svg';
import CashRegisterIcon from '../../assets/cash-register.svg';
import CategoryIcon from '../../assets/category.svg';
import HistoryIcon from '../../assets/history.svg';
import DashboardIcon from '../../assets/dashboard.svg';

import {getUserByStorage, validateLogin} from "../../utils.ts";
import {loadAllBalance, loadDashboardBalanceGroupByMonth} from '../../controller/balance.controller.tsx'
import {loadAllPortfolio, loadAllPortfolioInternal} from "../../controller/portfolio.controller.tsx";

import {useTheme} from '../../contexts/ThemeContext';
import {getStyle} from "../../styles/styles.ts";
import {getHomeStyle} from "./styles";
import {constants} from "../../constants";
import {selectTransactionsTotals} from "../../repository/transaction.repository.tsx";
import {loadTotalsTransactions} from "../../controller/transaction.controller.tsx";
import * as I from "../../interfaces/interfaces.tsx";

const Home = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const homeStyle = getHomeStyle(theme);

    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState<User>({} as User);
    const [balanceTotal, setBalanceTotal] = useState<number>(0);
    const [transactionTotals, setTransactionTotals] = useState<I.TransactionTotals>({} as I.TransactionTotals);
    const [showValue, setShowValue] = useState(false);
    const [balancesPeriod, setBalancesPeriod] = useState<DashboardItem[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            let user = await getUserByStorage()
            setUser(user);

            await loadBalance();
        };

        fetchData();
    }, []);

    const loadBalance = async () => {
        setLoading(true);

        //Carrega os portfólios somente para que possa buscar os saldos posteriormente, pois saldo depende de portfolio
        let responsePortfolios = await loadAllPortfolio(1, null);
        validateLogin(responsePortfolios, navigation);

        let response = await loadAllBalance(null);

        //Carrega as contas novamente para pegar os saldos atualizados, na primeira página
        responsePortfolios = await loadAllPortfolioInternal(null, null);

        const total = sumBalanceTotal(responsePortfolios);
        setBalanceTotal(total);

        //TODO: Futuramente o período deverá vir de configuração e não ser setado diretamente no código
        let date = new Date();
        let mountDateInicio = new Date(date.getFullYear(), date.getMonth(), 5, 0, 0, 0);
        let mountDateFim = new Date(date.getFullYear(), date.getMonth() + 1, 4, 23, 59, 59);
        let responseTotalsTransactions = await loadTotalsTransactions(mountDateInicio, mountDateFim, false, false);
        setTransactionTotals(responseTotalsTransactions);

        const dataAnterior = new Date();
        dataAnterior.setMonth(dataAnterior.getMonth() - 6);

        let responseBalances = await loadDashboardBalanceGroupByMonth(dataAnterior.getFullYear(), dataAnterior.getMonth());
        setBalancesPeriod(responseBalances);

        setLoading(false);
    };

    const sumBalanceTotal = (response: Response): number => {
        return response.data.reduce((soma, item) => {
            return item.ParentPortfolio === null ? soma + item.BalanceTotals.Value : soma
        }, 0);
    };

    const handleIconUserClick = () => {
        navigation.navigate("UserAccount");
    }

    const renderValueTotal = () => {
        return showValue ? `R$ ${balanceTotal.toFixed(2)}` : <View style={homeStyle.valueHidden}/>;
    };

    const renderValueTotalProjection = (value: number) => {
        return showValue ? `R$ ${value.toFixed(2)}` : <View style={homeStyle.valueProjectionHidden}/>;
    };

    const handleShowValueClick = () => {
        setShowValue(!showValue);
    };

    const goTo = (screenName: string, param: any = null) => {
        if (param !== null) {
            navigation.navigate(screenName, param);
        } else {
            navigation.navigate(screenName);
        }
    };

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={style.viewHeaderConsultaReduced}>
                    <View style={style.titleScreen}>
                        <View style={homeStyle.textUserArea}>
                            <Text style={homeStyle.textUser}>Olá, {user.Name}</Text>
                        </View>
                        <TouchableOpacity style={homeStyle.iconUser} onPress={handleIconUserClick}>
                            <UserIcon width="32" height="32" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                    </View>
                </View>
                <View style={style.viewBodyConsultaLarger}>
                    <View style={homeStyle.areaBalance}>
                        <Text style={homeStyle.balanceLabelItem}>Seu Saldo Atual</Text>
                        <View style={homeStyle.balanceRow}>
                            {loading
                                ? <ActivityIndicator size="small" color={theme.colors.primaryTextColor}/>
                                : <Text
                                    style={[homeStyle.balanceTextItem, balanceTotal < 0 ? homeStyle.balanceTextNegative : homeStyle.balanceTextPositive]}>{renderValueTotal()}</Text>
                            }
                            <TouchableOpacity onPress={handleShowValueClick}>{showValue ?
                                <VisibilityOffIcon width={30} fill={theme.colors.primaryIconDashboard}/> :
                                <VisibilityIcon width={30} fill={theme.colors.primaryIconDashboard}/>}
                            </TouchableOpacity>
                        </View>
                        <Text style={homeStyle.balanceTextInfo}>Valores pendentes e saldo projetado após
                            consolidação</Text>
                        <View style={homeStyle.balanceRow}>
                            <View style={homeStyle.balanceCard}>
                                <Text style={homeStyle.balanceLabelCardItem}>Receita</Text>
                                {!loading
                                    ? <Text
                                        style={[homeStyle.balanceTextItemSecondary, homeStyle.balanceTextPositive]}>{renderValueTotalProjection(transactionTotals?.Credit ?? 0)}</Text>
                                    : null
                                }
                            </View>
                            <View style={homeStyle.balanceCard}>
                                <Text style={homeStyle.balanceLabelCardItem}>Despesa</Text>
                                {!loading
                                    ? <Text
                                        style={[homeStyle.balanceTextItemSecondary, homeStyle.balanceTextNegative]}>{renderValueTotalProjection(transactionTotals?.Debit ?? 0)}</Text>
                                    : null
                                }
                            </View>
                            <View style={homeStyle.balanceCard}>
                                <Text style={homeStyle.balanceLabelCardItem}>Projeção</Text>
                                {!loading
                                    ? <Text
                                        style={[homeStyle.balanceTextItemSecondary, 
                                            (balanceTotal + transactionTotals?.Credit - transactionTotals?.Debit) >= 0 
                                                ? homeStyle.balanceTextPositive 
                                                : homeStyle.balanceTextNegative]}>
                                        {renderValueTotalProjection(balanceTotal + transactionTotals?.Credit - transactionTotals?.Debit)}
                                      </Text>
                                    : null
                                }
                            </View>
                        </View>
                    </View>
                    <View style={homeStyle.areaFeatures}>
                        <View style={homeStyle.featureRow}>
                            <TouchableOpacity
                                style={homeStyle.featureItem}
                                onPress={() => goTo('Transaction')}>
                                <CashRegisterIcon width="28" height="28" fill={theme.colors.primaryIconDashboard}/>
                                <Text style={homeStyle.featureTextItem}>Transação</Text>
                                <Text style={homeStyle.featureSubTextItem}>Gerencie suas receitas e despesas</Text>
                            </TouchableOpacity>
                            <TouchableOpacity
                                style={homeStyle.featureItem}
                                onPress={() => {
                                }}>
                                <DashboardIcon width="28" height="28" fill={theme.colors.primaryIconDashboard}/>
                                <Text style={homeStyle.featureTextItem}>Dashboard</Text>
                                <Text style={homeStyle.featureSubTextItem}>Acompanhe seu patrimônio</Text>
                            </TouchableOpacity>
                        </View>
                        <View style={homeStyle.featureRow}>
                            <TouchableOpacity
                                style={homeStyle.auxiliaryItem}
                                onPress={() => goTo('Category')}>
                                <View style={homeStyle.auxiliaryIconItem}>
                                    <CategoryIcon width="32" height="32" fill={theme.colors.primaryIconDashboard}/>
                                </View>
                                <Text style={homeStyle.auxiliaryTextItem}>Categoria</Text>
                            </TouchableOpacity>
                            <TouchableOpacity
                                style={homeStyle.auxiliaryItem}
                                onPress={() => goTo('Account', {actionNavigation: constants.actionNavigation.reload})}>
                                <View style={homeStyle.auxiliaryIconItem}>
                                    <AccountIcon width="32" height="32" fill={theme.colors.primaryIconDashboard}/>
                                </View>
                                <Text style={homeStyle.auxiliaryTextItem}>Conta</Text>
                            </TouchableOpacity>
                            <TouchableOpacity
                                style={homeStyle.auxiliaryItem}
                                onPress={() => goTo('Operation', {actionNavigation: constants.actionNavigation.reload})}>
                                <View style={homeStyle.auxiliaryIconItem}>
                                    <HistoryIcon width="32" height="32" fill={theme.colors.primaryIconDashboard}/>
                                </View>
                                <Text style={homeStyle.auxiliaryTextItem}>Operação</Text>
                            </TouchableOpacity>
                        </View>
                    </View>
                    <View style={homeStyle.areaChart}>
                        <VictoryChart theme={VictoryTheme.material}
                                      domainPadding={{x: 30, y: [20, 20]}}>
                            <VictoryAxis
                                style={{
                                    tickLabels: {fontSize: 12, padding: 5},
                                }}
                            />
                            <VictoryAxis
                                dependentAxis
                                tickFormat={(x) => `${x}`}
                                style={{
                                    tickLabels: {fontSize: 12, padding: 5},
                                }}
                            />

                            <VictoryLine
                                data={balancesPeriod}
                                x="Label"
                                y="Value"
                                style={{
                                    data: {stroke: theme.colors.secondaryMonetaryColor, strokeWidth: 3},
                                }}
                                interpolation="monotoneX"
                            />
                        </VictoryChart>
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default Home;