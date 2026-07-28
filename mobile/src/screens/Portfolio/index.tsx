import React, {useEffect, useState} from "react";
import AccountIcon from "../../assets/account.svg";
import ExpandIcon from "../../assets/expand.svg";
import ExpandLessIcon from "../../assets/expand_less.svg";
import {ScrollView, Text, TouchableOpacity, View} from "react-native";

import * as I from "../../interfaces/interfaces.tsx";
import {PageProcess, PageSpecial} from "../../components/Page";
import {constants as pageConstants} from "../../components/Page/constants";

import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getStyle} from "../../styles/styles.ts";
import {constants} from "../../constants";
import {getPortfolioStyle} from "./styles";
import Icon from "../../components/Icon";
import AuxiliaryButton from "../../components/AuxiliaryButton";
import PortfolioView from "./View";

const Portfolio = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const portfolioStyle = getPortfolioStyle(theme);

    let portfolios: I.Portfolio[] = [{
        Id: 1,
        InternalId: 1,
        Name: "CDB",
        Type: 1,
        Group: 2,
        Status: 1,
        Category: {
            Id: 1,
            InternalId: 1,
            Name: "Investimentos",
            Type: 1,
            Status: 1,
            DataCriacao: "",
            DataAlteracao: ""
        },
        ParentPortfolio: null,
        DataCriacao: "",
        DataAlteracao: "",
        BalanceTotals: {
            Value: 15000
        }
    },
        {
            Id: 2,
            InternalId: 2,
            Name: "Apartamento 01",
            Type: 2,
            Group: 52,
            Status: 1,
            Category: {
                Id: 1,
                InternalId: 1,
                Name: "Investimentos",
                Type: 1,
                Status: 1,
                DataCriacao: "",
                DataAlteracao: ""
            },
            ParentPortfolio: null,
            DataCriacao: "",
            DataAlteracao: "",
            BalanceTotals: {
                Value: 380000
            }
        },
        {
            Id: 3,
            InternalId: 3,
            Name: "Garagem 01",
            Type: 2,
            Group: 52,
            Status: 1,
            Category: {
                Id: 1,
                InternalId: 1,
                Name: "Investimentos",
                Type: 1,
                Status: 1,
                DataCriacao: "",
                DataAlteracao: ""
            },
            ParentPortfolio: null,
            DataCriacao: "",
            DataAlteracao: "",
            BalanceTotals: {
                Value: 10500
            }
        }];

    const [portfolioType, setPortfolioType] = useState(constants.portfolioType.ativo);
    const [groupTypes, setGroupTypes] = useState<I.SymbolStringView[]>([]);

    useEffect(() => {
        if (groupTypes.length === 0)
            getGroupTypes();
    }, [])

    const getGroupTypes = () => {
        let keys: I.SymbolStringView[] = Object.values(constants.portfolioGroupType.ativo);
        keys.push(...Object.values(constants.portfolioGroupType.passivo));

        setGroupTypes(keys);
    }

    const getTotal = (type: number) => {
        return portfolios?.filter(x => x.Type === type).reduce((acc, item) => acc + Number(item.BalanceTotals.Value), 0);
    }

    const getTotalPercentage = (type: number) => {
        let total = portfolios?.reduce((acc, item) => acc + Number(item.BalanceTotals.Value), 0);
        let totalType = getTotal(type);

        return (totalType * 100) / total;
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

    const handleClickIsVisible = (groupType: I.SymbolStringView) => {
        groupType.IsVisible = !groupType.IsVisible;

        setGroupTypes((prevGroupType) =>
            prevGroupType.map((item) =>
                item.Id === groupType.Id ? groupType : item
            )
        );
    }

    const _renderGroup = () => {
        if (!portfolios) return null;
        return (
            groupTypes.filter(x => x.Type === portfolioType.Id).map(type => {
                let portfoliosGroup = portfolios.filter(x => x.Group === type.Id && x.Type === portfolioType.Id);
                let totalValue = portfoliosGroup?.reduce((acc, item) => acc + Number(item.BalanceTotals.Value), 0);
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
                            <View onTouchEndCapture={() => handleItemClick(item)} key={portfolioGroup} style={portfolioStyle.portfolioGroupItens}>
                                <View key={`${portfolioGroup}-${item.Id}`} style={style.row}>
                                    <Text style={style.textPrimary16}>{item.Name}</Text>
                                    <View style={style.row}>
                                        <Text style={style.textPrimary16}>R$ {item.BalanceTotals.Value.toFixed(2)}</Text>
                                        <Icon name="next" size="24" color={theme.colors.quaternaryIcon}/>
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
                <ScrollView
                    style={portfolioStyle.scroll}>
                    {_renderGroup()}
                </ScrollView>
            </View>
        </PageSpecial>
    )
}

export default Portfolio;