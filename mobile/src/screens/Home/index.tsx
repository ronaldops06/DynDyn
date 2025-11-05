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

const Home = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const homeStyle = getHomeStyle(theme);
    
    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState<User>({} as User);
    const [balanceTotal, setBalanceTotal] = useState<number>(0);
    const [showValue, setShowValue] = useState(false);
    const [ balancesPeriod, setBalancesPeriod] = useState<DashboardItem[]>([]);

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
        let responsePortfolios = await loadAllPortfolio(1);
        validateLogin(responsePortfolios, navigation);

        let response = await loadAllBalance(null);

        //Carrega as contas novamente para pegar os saldos atualizados, na primeira página
        responsePortfolios = await loadAllPortfolioInternal(null);

        const total = sumBalanceTotal(responsePortfolios);
        setBalanceTotal(total);

        const dataAnterior = new Date();
        dataAnterior.setMonth(dataAnterior.getMonth() - 6);
        
        let responseBalances = await loadDashboardBalanceGroupByMonth(dataAnterior.getFullYear(), dataAnterior.getMonth());
        setBalancesPeriod(responseBalances);
        
        setLoading(false);
    };

    const sumBalanceTotal = (response: Response): number => {
        return response.data.reduce((soma, item) => {
            return item.Portfolio?.ParentPortfolio === undefined ? soma + item.BalanceTotals.Value : soma
        }, 0);
    };
    
    const handleIconUserClick = () => {
        navigation.navigate("UserAccount");
    }
    
    const renderValueTotal = () => {
        return showValue ? `R$ ${balanceTotal.toFixed(2)}` : <View style={homeStyle.valueHidden}/>;
    };

    const handleShowValueClick = () => {
        setShowValue(!showValue);
    };

    const goTo = (screenName: string) => {
        navigation.navigate(screenName);
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
                                ? <ActivityIndicator size="large" color={theme.colors.primaryTextColor}/>
                                : <Text
                                    style={[homeStyle.balanceTextItem, balanceTotal < 0 ? homeStyle.balanceTextNegative : homeStyle.balanceTextPositive]}>{renderValueTotal()}</Text>
                            }
                            <TouchableOpacity onPress={handleShowValueClick}>{showValue ?
                                <VisibilityOffIcon width={30} fill={theme.colors.primaryIconDashboard}/> :
                                <VisibilityIcon width={30} fill={theme.colors.primaryIconDashboard}/>}
                            </TouchableOpacity>
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
                                onPress={() => goTo('Account')}>
                                <View style={homeStyle.auxiliaryIconItem}>
                                    <AccountIcon width="32" height="32" fill={theme.colors.primaryIconDashboard}/>
                                </View>
                                <Text style={homeStyle.auxiliaryTextItem}>Conta</Text>
                            </TouchableOpacity>
                            <TouchableOpacity
                                style={homeStyle.auxiliaryItem}
                                onPress={() => goTo('Operation')}>
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
                                    tickLabels: { fontSize: 12, padding: 5 },
                                }}
                            />
                            <VictoryAxis
                                dependentAxis
                                tickFormat={(x) => `${x}`}
                                style={{
                                    tickLabels: { fontSize: 12, padding: 5 },
                                }}
                            />

                            <VictoryLine
                                data={balancesPeriod}
                                x="Label"
                                y="Value"
                                style={{
                                    data: { stroke: theme.colors.secondaryMonetaryColor, strokeWidth: 3 },
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