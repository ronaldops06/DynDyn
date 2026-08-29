import React, {useState} from 'react';
import {ScrollView, Text, TouchableOpacity, View} from 'react-native';
import {PageSpecial} from '../../../components/Page';
import {useTheme} from '../../../contexts/ThemeContext';
import {getStyle} from '../../../styles/styles';
import {getPortfolioViewStyle} from './styles';
import Icon from '../../../components/Icon';
import DocumentIcon from '../../../assets/copy.svg';
import PlusIcon from '../../../assets/plus.svg';
import Tag from "../../../components/Tag";
import {constants} from "../../../constants";
import {PortfolioAttribute} from "../../../interfaces/interfaces.tsx";
import Moment from "moment/moment";

const PortfolioView = ({navigation, route}: {navigation: any, route: any}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const portfolioViewStyle = getPortfolioViewStyle(theme);

    const data = route.params?.data;
    
    const [transactionHistory] = useState([
        {
            id: 1,
            day: '12',
            month: 'MAI',
            year: '2020',
            title: 'Aquisição do imóvel',
            type: 'Compra',
            value: '- R$ 755.000,00',
        },
    ]);

    const getPortfolioGroupById = (portfolioGroupId: number) => {
        const activeGroups = Object.values(constants.portfolioGroupType.ativo);
        const liabilityGroups = Object.values(constants.portfolioGroupType.passivo);

        return [...activeGroups, ...liabilityGroups].find(item => item.Id === portfolioGroupId);
    };

    const getPortfolioTypeById = (portfolioTypeId: number) => {
        return Object.values(constants.portfolioType)?.find(item => item.Id === portfolioTypeId)?.Name;
    }

    const getValueFinancialResult = () => {
        return (data.BalanceTotals.Value ? data.BalanceTotals.Value : 0 - (data.AcquisitionCost ? data.AcquisitionCost : 0));
    }

    const getPercentageFinancialResult = () => {
        
        return getValueFinancialResult() * 100 / (data.AcquisitionCost ? data.AcquisitionCost : data.BalanceTotals.Value ?? 1);
    }

    const getValueAttribute = (portfolioAttribute: PortfolioAttribute) => {

        return (
            <>
                {(() => {
                    switch (portfolioAttribute.Attribute.DataType) {
                        case constants.attributeDataType.text.Id:
                            return <Text style={portfolioViewStyle.infoValue}>{portfolioAttribute.ValueText}</Text>;
                        case constants.attributeDataType.number.Id:
                            return <Text style={portfolioViewStyle.infoValue}>{portfolioAttribute.ValueNumber}</Text>;
                        case constants.attributeDataType.boolean.Id:
                            return <Text
                                style={portfolioViewStyle.infoValue}>{(portfolioAttribute.ValueBoolean) ? 'Verdadeiro' : 'Falso'}</Text>;
                        case constants.attributeDataType.date.Id:
                            return <Text style={portfolioViewStyle.infoValue}>{Moment(portfolioAttribute.ValueDate).format('DD/MM/YYYY')}</Text>;
                    }
                })()}
            </>
        );
    }

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleEdit = () => {
        navigation.navigate("PortfolioRegister", {
            isEditing: true, data: data
        });
    };

    const handleMenu = () => {
        // Implementar menu de opções
    };

    const renderHeaderCard = () => {
        return (
            <View style={portfolioViewStyle.headerCard}>
                <View style={portfolioViewStyle.headerIcon}>
                    <Icon name={getPortfolioGroupById(data.Group)?.Icon ?? 'account'} size={32} color={theme.colors.quaternaryIcon}/>
                </View>
                <View style={portfolioViewStyle.headerContent}>
                    <Text style={portfolioViewStyle.headerTitle}>{data.Name}</Text>
                    <Text style={portfolioViewStyle.headerSubtitle}>{getPortfolioGroupById(data.Group)?.Name}</Text>
                    <View style={style.row}>
                    <Tag
                        style={portfolioViewStyle.statusTag}
                        text={data.Status === 1 ? "Aberto" : "Encerrado"}
                        color={theme.colors.tertiaryBaseColor}
                        textColor={data.Status === constants.status.active.Id ? theme.colors.sextenaryTextColor : theme.colors.dangerTextColor}
                    />
                    <Text style={portfolioViewStyle.textLink} onPress={handleEdit}>Editar</Text>
                    </View>
                </View>
            </View>
        );
    };

    const renderValueSection = () => {
        return (
            <View style={portfolioViewStyle.valueSection}>
                <Text style={portfolioViewStyle.sectionLabel}>Valor atual</Text>
                <Text style={portfolioViewStyle.mainValue}>
                    R$ {data.BalanceTotals?.Value?.toFixed(2) ?? "0.00"}
                </Text>
                <Text style={portfolioViewStyle.updateDate}>
                    Atualizado em {Moment(data.DataAlteracao).format('DD/MM/YYYY')}
                </Text>
            </View>
        );
    };

    const renderAcquisitionSection = () => {
        if (!data) return null;
        
        return (
            <View style={portfolioViewStyle.twoColumnRow}>
                <View style={portfolioViewStyle.columnItem}>
                    <Text style={portfolioViewStyle.sectionLabel}>Valor de aquisição</Text>
                    <Text style={portfolioViewStyle.columnValue}>
                        R$ {data.AcquisitionCost?.toFixed(2)}
                    </Text>
                    <Text style={portfolioViewStyle.updateDate}>
                        {Moment(data.DataCriacao).format('DD/MM/YYYY')}
                    </Text>
                </View>
                <View style={portfolioViewStyle.columnItem}>
                    <Text style={portfolioViewStyle.sectionLabel}>Resultado</Text>
                    <Text style={[portfolioViewStyle.columnValue, {color: theme.colors.sextenaryTextColor}]}>
                        R$ {getValueFinancialResult().toFixed(2)}
                    </Text>
                    <Text style={portfolioViewStyle.updateDate}>
                        {getPercentageFinancialResult().toFixed(2)}%
                    </Text>
                </View>
            </View>
        );
    };

    const renderActionItems = () => {
        return (
            <View>
                <TouchableOpacity style={portfolioViewStyle.actionItemContainer}>
                    <View style={portfolioViewStyle.actionItemContent}>
                        <View style={portfolioViewStyle.actionItemIcon}>
                            <DocumentIcon width={20} height={20} fill={theme.colors.quaternaryIcon}/>
                        </View>
                        <View style={portfolioViewStyle.actionItemText}>
                            <Text style={portfolioViewStyle.actionItemTitle}>Documentos</Text>
                            <Text style={portfolioViewStyle.actionItemSubtitle}>
                                Visualize a gerência dos documentos do portfólio
                            </Text>
                        </View>
                    </View>
                    <Icon name="next" size={24} color={theme.colors.quaternaryIcon}/>
                </TouchableOpacity>

                <TouchableOpacity style={portfolioViewStyle.actionItemContainer}>
                    <View style={portfolioViewStyle.actionItemContent}>
                        <View style={portfolioViewStyle.actionItemIcon}>
                            <PlusIcon width={20} height={20} fill={theme.colors.quaternaryIcon}/>
                        </View>
                        <View style={portfolioViewStyle.actionItemText}>
                            <Text style={portfolioViewStyle.actionItemTitle}>Nova transação</Text>
                            <Text style={portfolioViewStyle.actionItemSubtitle}>
                                Adicionar compra, venda, manutenção e outras
                            </Text>
                        </View>
                    </View>
                    <Icon name="next" size={24} color={theme.colors.quaternaryIcon}/>
                </TouchableOpacity>
            </View>
        );
    };

    const renderGeneralInfo = () => {
        return (
            <View style={portfolioViewStyle.box}>
                <Text style={portfolioViewStyle.sectionHeader}>Informações gerais</Text>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name={getPortfolioGroupById(data.Group)?.Icon ?? 'account'} size={20}
                              color={theme.colors.quaternaryIcon}/>
                        <Text style={portfolioViewStyle.infoLabelText}>Tipo</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{getPortfolioGroupById(data.Group)?.Name}</Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="rule" size={20} color={theme.colors.quaternaryIcon}/>
                        <Text style={portfolioViewStyle.infoLabelText}>Natureza</Text>
                    </View>
                    <Text style={[portfolioViewStyle.infoValue, {color: theme.colors.sextenaryTextColor}]}>
                        {getPortfolioTypeById(data.Type)}
                    </Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="date" size={20} color={theme.colors.quaternaryIcon}/>
                        <Text style={portfolioViewStyle.infoLabelText}>Data de abertura</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{Moment(data.DataCriacao).format('DD/MM/YYYY')}</Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="date" size={20} color={theme.colors.quaternaryIcon}/>
                        <Text style={portfolioViewStyle.infoLabelText}>Data de encerramento</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{data.EndDate ? Moment(data.EndDate).format('DD/MM/YYYY') : '—'}</Text>
                </View>
                <View style={[portfolioViewStyle.infoRow, {borderBottomWidth: 0}]}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="text" size={20} color={theme.colors.quaternaryIcon}/>
                        <Text style={portfolioViewStyle.infoLabelText}>Descrição</Text>
                    </View>
                    <Text style={[portfolioViewStyle.infoValue, {textAlign: 'left', marginTop: 8}]}>
                        {data.Description}
                    </Text>
                </View>

            </View>
        );
    };

    const renderCustomAttributes = () => {
        return (
            <View style={portfolioViewStyle.box}>
                <Text style={portfolioViewStyle.sectionHeader}>Atributos personalizados</Text>
                {data.Attributes?.length > 0 ? 
                data.Attributes?.map((attr: PortfolioAttribute, index: number) => (
                    <View key={index} style={portfolioViewStyle.infoRow}>
                        <View style={portfolioViewStyle.infoLabel}>
                            <Text style={portfolioViewStyle.infoLabelText}>{attr.Attribute.Name}</Text>
                        </View>
                        getValueAttribute(attr);
                    </View>
                )) :
                    <Text style={portfolioViewStyle.actionItemSubtitle}>
                        Este Patrimônio não possui atributos personalizados.
                    </Text>
                }
            </View>
        );
    };

    const renderTransactionHistory = () => {
        return (
            <View style={portfolioViewStyle.box}>
                <Text style={portfolioViewStyle.sectionHeader}>Histórico de transações</Text>
                {transactionHistory.map((transaction, index) => (
                    <View key={index} style={portfolioViewStyle.transactionHistoryItem}>
                        <View style={portfolioViewStyle.transactionLeft}>
                            <View style={portfolioViewStyle.transactionDate}>
                                <Text style={portfolioViewStyle.transactionDay}>{transaction.day}</Text>
                                <Text style={portfolioViewStyle.transactionMonth}>{transaction.month}</Text>
                                <Text style={portfolioViewStyle.transactionYear}>{transaction.year}</Text>
                            </View>
                            <View style={portfolioViewStyle.transactionDetails}>
                                <Text style={portfolioViewStyle.transactionTitle}>{transaction.title}</Text>
                                <Text style={portfolioViewStyle.transactionType}>{transaction.type}</Text>
                            </View>
                        </View>
                        <View>
                            <Text style={portfolioViewStyle.transactionValue}>{transaction.value}</Text>
                            <Icon name="next" size={20} color={theme.colors.quaternaryIcon}/>
                        </View>
                    </View>
                ))}
                <View style={portfolioViewStyle.viewAllButton}>
                    <Text style={portfolioViewStyle.viewAllText}>Ver todas</Text>
                </View>
            </View>
        );
    };

    return (
        <PageSpecial
            title="Patrimônio"
            helpType="portfolio_view"
            onBackClick={handleBackClick}
        >
            <ScrollView
                contentContainerStyle={portfolioViewStyle.container}
                showsVerticalScrollIndicator={false}
            >
                {renderHeaderCard()}
                {renderValueSection()}
                {renderAcquisitionSection()}
                {renderActionItems()}
                {renderGeneralInfo()}
                {renderCustomAttributes()}
                {renderTransactionHistory()}
            </ScrollView>
        </PageSpecial>
    );
};

export default PortfolioView;
