import React, {useEffect, useRef, useState} from "react";
import {ActivityIndicator, ScrollView, Text, TouchableOpacity, View} from "react-native";
import {useFocusEffect} from "@react-navigation/native";
import ExpandIcon from "../../assets/expand.svg";
import ExpandLessIcon from "../../assets/expand_less.svg";
import {PageSpecial} from "../../components/Page";
import AuxiliaryButton from "../../components/AuxiliaryButton";
import Icon from "../../components/Icon";

import * as I from "../../interfaces/interfaces";
import {constants} from "../../constants";
import {validateLogin} from "../../utils";

import {loadAllPortfolio, loadAllPortfolioInternal} from "../../controller/portfolio.controller";
import {loadAllBalance} from "../../controller/balance.controller";

import {useTheme} from "../../contexts/ThemeContext";
import {getStyle} from "../../styles/styles";
import {getPortfolioStyle} from "./styles";

const Portfolio = ({navigation, route}: { navigation: any, route: any }) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const portfolioStyle = getPortfolioStyle(theme);
    
    const [portfolioType, setPortfolioType] = useState(constants.portfolioType.ativo);
    const [groupTypes, setGroupTypes] = useState<I.SymbolNumberView[]>([]);
    const isFirstRender = useRef(true);
    const [loading, setLoading] = useState(true);
    const [isLoadInternal, setIsLoadInternal] = useState(true);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setPortfolios([]);
            }
        }, [route.params?.actionNavigation])
    );
        
    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }
        
        if (portfolios.length === 0) {
            loadPortfolios();
        }
    }, [portfolios])
    
    useEffect(() => {
        if (groupTypes.length === 0) {
            getGroupTypes();
            setPortfolios([]);
        }
    }, [])
    
    const loadPortfolios = async () => {
        setLoading(true);

        let responsePortfolios = null;

        if (isLoadInternal) {
            responsePortfolios = await loadAllPortfolioInternal(null, null, null);
        } else {
            responsePortfolios = await loadAllPortfolio(null, null, null);
            validateLogin(responsePortfolios, navigation);

            let response = await loadAllBalance(null);

            //Carrega as contas novamente para pegar os saldos atualizados, na primeira página
            responsePortfolios = await loadAllPortfolioInternal(null, null, null);
        }

        setPortfolios(responsePortfolios?.data ?? []);

        setLoading(false);
        setIsLoadInternal(true);
    };

    const getGroupTypes = () => {
        let keys: I.SymbolNumberView[] = Object.values(constants.portfolioGroupType.ativo);
        keys.push(...Object.values(constants.portfolioGroupType.passivo));

        setGroupTypes(keys);
    }

    const getTotal = (type: number) => {
        return portfolios?.filter(x => x.Type === type && x.ParentPortfolio === null).reduce((acc, item) => acc + Number(item.BalanceTotals?.Value), 0);
    }

    const getTotalPercentage = (type: number) => {
        let total = portfolios?.reduce((acc, item) => acc + Number(item.BalanceTotals?.Value), 0);
        let totalType = getTotal(type);

        return (totalType * 100) / total ?? 1;
    }

    const getButtonStyle = (type: number) => {
        return [portfolioStyle.button, (portfolioType.Id == type ?
            portfolioStyle.buttonSelected :
            portfolioStyle.buttonDefault)];
    };

    const getTextButtonStyle = (type: number) => {
        return [portfolioStyle.textButton, (portfolioType.Id == type ?
            portfolioStyle.textButtonSelected :
            portfolioStyle.textButtonDefault)];
    };

    const handleBackClick = () => {
        navigation.goBack();
    };
    
    const handleNewClick = () => {
        navigation.navigate("PortfolioRegister", {
            isEditing: false, data: null
        });
    }

    const handleItemClick = (data: I.Portfolio) => {
        navigation.navigate("PortfolioView", {
            data: data
        });
    }

    const handleClickIsVisible = (groupType: I.SymbolNumberView) => {
        groupType.IsVisible = !groupType.IsVisible;

        setGroupTypes((prevGroupType) =>
            prevGroupType.map((item) =>
                item.Id === groupType.Id ? groupType : item
            )
        );
    }

    const _renderGroup = () => {
        if (portfolios.length === 0) return null;

        return (
            groupTypes.filter(x => x.Type === portfolioType.Id).map(type => {
                let portfoliosGroup = portfolios.filter(x => x.Group === type.Id && x.Type === portfolioType.Id && x.ParentPortfolio === null);
                let totalValue = portfoliosGroup?.reduce((acc, item) => acc + Number(item.BalanceTotals?.Value), 0);

                return (
                    <View key={type.Id} style={portfolioStyle.portfolioGroup}>
                        <View style={portfolioStyle.portfolioGroupHeader}>
                            <View style={portfolioStyle.portfolioGroupHeaderLeft}>
                                <Icon name={type.Icon} size={28} color={theme.colors.quaternaryIcon}/>
                                <Text style={style.textPrimary16}>{type.Name}</Text>
                            </View>
                            <View style={portfolioStyle.portfolioGroupHeaderRigth}>
                                <Text style={style.textPrimary16}>R$ {totalValue.toFixed(2)}</Text>
                                <TouchableOpacity
                                    onPress={() => handleClickIsVisible(type)}
                                    disabled={portfoliosGroup.length === 0}
                                >
                                    {type.IsVisible ?
                                        <ExpandLessIcon width="24" height="24" fill={theme.colors.quaternaryIcon}/> :
                                        <ExpandIcon width="24" height="24" fill={theme.colors.quaternaryIcon}/>
                                    }
                                </TouchableOpacity>
                            </View>
                        </View>
                        {type.IsVisible &&
                            <>
                                {_renderPortfolioByGroup(type.Id)}
                            </>
                        }
                    </View>
                )
            })
        );
    }

    const _renderPortfolioByGroup = (portfolioGroup: number) => {
        let portfoliosRendering = portfolios.filter(x => x.Group === portfolioGroup && x.Type === portfolioType.Id);

        if (portfoliosRendering.length === 0) return null;

        return (
            <>
                {portfoliosRendering.map(item => {
                        return (
                            <View onTouchEndCapture={() => handleItemClick(item)} key={portfolioGroup}
                                  style={portfolioStyle.portfolioGroupItens}>
                                <View key={`${portfolioGroup}-${item.Id}`} style={style.row}>
                                    <Text style={style.textPrimary16}>{item.Name}</Text>
                                    <View style={style.row}>
                                        <Text style={style.textPrimary16}>R$ {item.BalanceTotals?.Value?.toFixed(2) ?? "0.00"}</Text>
                                        <Icon name="next" size={24} color={theme.colors.quaternaryIcon}/>
                                    </View>
                                </View>
                            </View>
                        );
                    }
                )}
            </>
        )
    }

    return (
        <PageSpecial
            title={"Patrimônio"}
            helpType={"portfolio"}
            onBackClick={handleBackClick}
        >
            <View>
                <View style={portfolioStyle.areaCardTotals}>
                    <View style={[portfolioStyle.cardTotal, portfolioStyle.cardTotalLiability]}>
                        <Text style={portfolioStyle.cardTotalLabel}>Total de Ativos</Text>
                        <Text
                            style={portfolioStyle.textTotalLiability}>R$ {getTotal(constants.portfolioType.ativo.Id).toFixed(2)}</Text>
                        <Text
                            style={portfolioStyle.cardTotalLabel}>{getTotalPercentage(constants.portfolioType.ativo.Id).toFixed(2)}%
                            do patrimônio</Text>
                    </View>
                    <View style={[portfolioStyle.cardTotal, portfolioStyle.cardTotalAsset]}>
                        <Text style={portfolioStyle.cardTotalLabel}>Total de Passivos</Text>
                        <Text
                            style={portfolioStyle.textTotalAsset}>R$ {getTotal(constants.portfolioType.passivo.Id).toFixed(2)}</Text>
                        <Text
                            style={portfolioStyle.cardTotalLabel}>{getTotalPercentage(constants.portfolioType.passivo.Id).toFixed(2)}%
                            do patrimônio</Text>
                    </View>
                </View>
                <View>
                    <AuxiliaryButton
                        text="Adicionar Patrimônio"
                        onPress={handleNewClick}
                        icon="plus"
                        iconColor={theme.colors.quaternaryIcon}
                        type="secondary"
                    />
                </View>
                <View style={portfolioStyle.areaButtonSelect}>
                    <TouchableOpacity
                        style={getButtonStyle(constants.portfolioType.ativo.Id)}
                        onPress={() => setPortfolioType(constants.portfolioType.ativo)}>
                        <Text
                            style={getTextButtonStyle(constants.portfolioType.ativo.Id)}>Ativo</Text>
                    </TouchableOpacity>
                    <TouchableOpacity
                        style={getButtonStyle(constants.portfolioType.passivo.Id)}
                        onPress={() => setPortfolioType(constants.portfolioType.passivo)}>
                        <Text
                            style={getTextButtonStyle(constants.portfolioType.passivo.Id)}>Passivo</Text>
                    </TouchableOpacity>
                </View>
                {loading ?
                    <View style={portfolioStyle.loading}>
                        <ActivityIndicator size="large" color={theme.colors.primaryBaseColor}/> 
                    </View>:
                    <ScrollView
                        style={portfolioStyle.scroll}>
                        {!loading && _renderGroup()}
                    </ScrollView>
                }
            </View>
        </PageSpecial>
    )
}

export default Portfolio;