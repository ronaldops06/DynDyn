import React, {useState} from 'react';
import {SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getStyle} from "../../styles/styles.ts";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import PrevIcon from "../../assets/nav_prev.svg";
import HelpIcon from "../../assets/help_outline.svg";
import Help from "../../components/Help";
import CustomModal from "../../components/CustomModal";
import {VictoryAxis, VictoryBar, VictoryChart, VictoryLine, VictoryTheme, VictoryTooltip} from 'victory-native';
import { loadDashboardBalanceGroupByMonth} from '../../controller/balance.controller.tsx'
import * as I from "../../interfaces/interfaces.tsx"
import {useFocusEffect} from "@react-navigation/native";
import {loadDashboardTransactionFromCategory} from "../../controller/transaction.controller.tsx";
import {dashboardStyle} from "./styles";
import {constants} from "../../constants";

const Dashboard = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);

    const [showModalHelp, setShowModalHelp] = useState(false);
    const [balancesPeriod, setBalancesPeriod] = useState<I.DashboardItem[]>([]);
    const [valueCategoryExpense, setValueCategoryExpense] = useState<I.DashboardItem[]>([]);
    const [valueCategoryRevenue, setValueCategoryRevenue] = useState<I.DashboardItem[]>([]);

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
    
    const _renderWidgetBar = (description: string, data: I.DashboardItem[]) => {
        
        if (!data) return;
        
        return(
            <View>
                <Text style={style.textPrimary18}>{description}</Text>
                <VictoryChart
                    style={{
                        background: { fill: theme.colors.secondaryBaseColor },
                    }}
                    padding={{ top: 40, bottom: 180, left: 50, right: 50 }}
                    theme={VictoryTheme.material}
                    domainPadding={{ x: 30 }}>
                    <VictoryAxis
                        tickValues={data?.sort((a, b) => b?.Value - a?.Value).map(d => d?.Label)}
                        style={{
                            tickLabels: {
                                fontSize: 14,
                                padding: 5,
                                angle: -90,
                                fill: theme.colors.primaryTextColor,
                                textAnchor: 'end',
                                verticalAnchor: 'start',
                                dy: -10,
                            }
                        }}

                    />
                    <VictoryAxis dependentAxis/>

                    <VictoryBar
                        data={data?.sort((a, b) => b?.Value - a?.Value)}
                        x="Label"
                        y="Value"
                        labels={({ datum }) =>
                            typeof datum?.Value === 'number'
                                ? `R$ ${datum?.Value?.toFixed(2)?.toString()}`
                                : ''
                        }
                        barWidth={10}
                        alignment="middle"
                        style={{
                            data: {fill: theme.colors.secondaryMonetaryColor},
                            labels: {
                                fill: theme.colors.primaryMonetaryColor,
                                fontSize: 14,
                            },
                        }}
                        animate={{
                            duration: 600,
                            easing: 'quadInOut',
                        }}
                        labelComponent={<VictoryTooltip/>}
                    />
                </VictoryChart>
            </View>
        );
    }
    
    const loadData = async () => {
        const dataAnterior = new Date();
        dataAnterior.setMonth(dataAnterior.getMonth() - 6);

        let responseBalances = await loadDashboardBalanceGroupByMonth(dataAnterior.getFullYear(), dataAnterior.getMonth());
        setBalancesPeriod(responseBalances);

        let date = new Date();
        let mountDateInicio = new Date(date.getFullYear(), date.getMonth(), 5, 0, 0, 0);
        let mountDateFim = new Date(date.getFullYear(), date.getMonth() + 1, 4, 23, 59, 59);
        let responseTransactions = await loadDashboardTransactionFromCategory(mountDateInicio, mountDateFim, constants.operationType.expense.Id);
        setValueCategoryExpense(responseTransactions);
        
        responseTransactions = await loadDashboardTransactionFromCategory(mountDateInicio, mountDateFim, constants.operationType.revenue.Id);
        setValueCategoryRevenue(responseTransactions);
    }
    
    return(
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
                    <Text style={style.textPrimary18}>Saldo dos últimos 6 meses</Text>
                    <VictoryChart theme={VictoryTheme.material}
                                  domainPadding={{x: 30, y: [20, 20]}}>
                        <VictoryAxis
                            style={{
                                tickLabels: {
                                    fontSize: 14,
                                    padding: 5,
                                    angle: -90,
                                    fill: theme.colors.primaryTextColor,
                                    textAnchor: 'end',
                                    verticalAnchor: 'start',
                                    dy: -10,
                                }
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
                    {_renderWidgetBar("Despesas por categoria no mês atual", valueCategoryExpense)}
                    
                </View>
                <CustomModal show={showModalHelp} setShow={setShowModalHelp}>
                    <Help helpType="dashboard"/>
                </CustomModal>
            </ScrollView>
        </SafeAreaView>
    );
}

export default Dashboard;