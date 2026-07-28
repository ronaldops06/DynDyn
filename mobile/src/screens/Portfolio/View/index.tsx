import React, { useState } from 'react';
import { View, Text, ScrollView, TouchableOpacity } from 'react-native';
import { PageSpecial } from '../../../components/Page';
import { useTheme } from '../../../contexts/ThemeContext';
import { getStyle } from '../../../styles/styles';
import { getPortfolioViewStyle } from './styles';
import Icon from '../../../components/Icon';
import HomeIcon from '../../../assets/home.svg';
import DocumentIcon from '../../../assets/copy.svg';
import PlusIcon from '../../../assets/plus.svg';
import Tag from "../../../components/Tag";
import {getDescriptionStatus} from "../../../utils.ts";
import {constants} from "../../../constants";

const PortfolioView = ({ navigation, route }) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const portfolioViewStyle = getPortfolioViewStyle(theme);

    const data = route.params?.data;

    // Mock data - substitua com dados reais quando implementar integração
    const [portfolioData] = useState({
        id: 1,
        name: 'Imóvel Avenida Brasil',
        type: 'Imóvel',
        status: 'ABERTO',
        statusColor: theme.colors.sextenaryTextColor,
        currentValue: 850000.00,
        acquisitionValue: 755000.00,
        result: 95000.00,
        resultPercentage: 12.6,
        updateDate: '24/05/2024',
        propertyType: 'Imóvel',
        nature: 'Ativo',
        acquisitionDate: '12/05/2020',
        saleDate: null,
        description: 'Apartamento residencial localizado na Av. Brasil, 1234.',
    });

    const [customAttributes] = useState([
        { icon: 'rule', label: 'Tipo do imóvel', value: 'Apartamento' },
        { icon: 'rule', label: 'Área útil (m²)', value: '85 m²' },
        { icon: 'home', label: 'Localização', value: 'São Paulo - SP' },
        { icon: 'rule', label: 'Andar', value: '8º' },
        { icon: 'plus', label: 'Número de quartos', value: '3' },
        { icon: 'rule', label: 'Vagas de garagem', value: '2' },
        { icon: 'rule', label: 'IPTU anual', value: 'R$ 2.400,00' },
        { icon: 'rule', label: 'Condomínio mensal', value: 'R$ 850,00' },
    ]);

    const [financialSummary] = useState({
        currentBalance: 850000.00,
        totalInvested: 755000.00,
        appreciation: 95000.00,
        profitability: 12.6,
    });

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
        return Object.values(constants.portfolioType).find(item => item.Id === portfolioTypeId).Name;
    }

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleEdit = () => {
        // Implementar lógica de edição
    };

    const handleMenu = () => {
        // Implementar menu de opções
    };

    const renderHeaderCard = () => {
        return (
            <View style={portfolioViewStyle.headerCard}>
                <View style={portfolioViewStyle.headerIcon}>
                    <Icon name={getPortfolioGroupById(data.Group).Icon} size={32} color={theme.colors.quaternaryIcon} />
                </View>
                <View style={portfolioViewStyle.headerContent}>
                    <Text style={portfolioViewStyle.headerTitle}>{data.Name}</Text>
                    <Text style={portfolioViewStyle.headerSubtitle}>{getPortfolioGroupById(data.Group).Name}</Text>
                    <Tag 
                        style={portfolioViewStyle.statusTag}
                        text={data.Status === 1 ? "Aberto" : "Encerrado"}
                        color={theme.colors.tertiaryBaseColor}
                        textColor={data.Status === constants.status.active.Id ? theme.colors.sextenaryTextColor : theme.colors.dangerTextColor}
                    />
                </View>
            </View>
        );
    };

    const renderValueSection = () => {
        return (
            <View style={portfolioViewStyle.valueSection}>
                <Text style={portfolioViewStyle.sectionLabel}>Valor atual</Text>
                <Text style={portfolioViewStyle.mainValue}>
                    R$ {data.BalanceTotals.Value.toFixed(2)}
                </Text>
                <Text style={portfolioViewStyle.updateDate}>
                    Atualizado em {portfolioData.updateDate}
                </Text>
            </View>
        );
    };

    const renderAcquisitionSection = () => {
        return (
            <View style={portfolioViewStyle.twoColumnRow}>
                <View style={portfolioViewStyle.columnItem}>
                    <Text style={portfolioViewStyle.sectionLabel}>Valor de aquisição</Text>
                    <Text style={portfolioViewStyle.columnValue}>
                        R$ {portfolioData.acquisitionValue.toFixed(2)}
                    </Text>
                    <Text style={portfolioViewStyle.updateDate}>
                        {portfolioData.acquisitionDate}
                    </Text>
                </View>
                <View style={portfolioViewStyle.columnItem}>
                    <Text style={portfolioViewStyle.sectionLabel}>Resultado</Text>
                    <Text style={[portfolioViewStyle.columnValue, { color: theme.colors.sextenaryTextColor }]}>
                        R$ {portfolioData.result.toFixed(2)}
                    </Text>
                    <Text style={portfolioViewStyle.updateDate}>
                        {portfolioData.resultPercentage.toFixed(2)}%
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
                            <DocumentIcon width={20} height={20} fill={theme.colors.quaternaryIcon} />
                        </View>
                        <View style={portfolioViewStyle.actionItemText}>
                            <Text style={portfolioViewStyle.actionItemTitle}>Documentos</Text>
                            <Text style={portfolioViewStyle.actionItemSubtitle}>
                                Visualize a gerência dos documentos do portfólio
                            </Text>
                        </View>
                    </View>
                    <Icon name="next" size={24} color={theme.colors.quaternaryIcon} />
                </TouchableOpacity>

                <TouchableOpacity style={portfolioViewStyle.actionItemContainer}>
                    <View style={portfolioViewStyle.actionItemContent}>
                        <View style={portfolioViewStyle.actionItemIcon}>
                            <PlusIcon width={20} height={20} fill={theme.colors.quaternaryIcon} />
                        </View>
                        <View style={portfolioViewStyle.actionItemText}>
                            <Text style={portfolioViewStyle.actionItemTitle}>Nova transação</Text>
                            <Text style={portfolioViewStyle.actionItemSubtitle}>
                                Adicionar compra, venda, manutenção e outras
                            </Text>
                        </View>
                    </View>
                    <Icon name="next" size={24} color={theme.colors.quaternaryIcon} />
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
                        <Icon name={getPortfolioGroupById(data.Group).Icon} size={20} color={theme.colors.quaternaryIcon} />
                        <Text style={portfolioViewStyle.infoLabelText}>Tipo</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{getPortfolioGroupById(data.Group).Name}</Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="rule" size={20} color={theme.colors.quaternaryIcon} />
                        <Text style={portfolioViewStyle.infoLabelText}>Natureza</Text>
                    </View>
                    <Text style={[portfolioViewStyle.infoValue, { color: theme.colors.sextenaryTextColor }]}>
                        {getPortfolioTypeById(data.Type)}
                    </Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="date" size={20} color={theme.colors.quaternaryIcon} />
                        <Text style={portfolioViewStyle.infoLabelText}>Data de abertura</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{portfolioData.acquisitionDate}</Text>
                </View>
                <View style={portfolioViewStyle.infoRow}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="date" size={20} color={theme.colors.quaternaryIcon} />
                        <Text style={portfolioViewStyle.infoLabelText}>Data de encerramento</Text>
                    </View>
                    <Text style={portfolioViewStyle.infoValue}>{portfolioData.saleDate ?? '—'}</Text>
                </View>
                <View style={[portfolioViewStyle.infoRow, { borderBottomWidth: 0 }]}>
                    <View style={portfolioViewStyle.infoLabel}>
                        <Icon name="text" size={20} color={theme.colors.quaternaryIcon} />
                        <Text style={portfolioViewStyle.infoLabelText}>Descrição</Text>
                    </View>
                    <Text style={[portfolioViewStyle.infoValue, { textAlign: 'left', marginTop: 8 }]}>
                        {portfolioData.description}
                    </Text>
                </View>
                
            </View>
        );
    };

    const renderCustomAttributes = () => {
        return (
            <View style={portfolioViewStyle.box}>
                <Text style={portfolioViewStyle.sectionHeader}>Atributos personalizados</Text>
                {customAttributes.map((attr, index) => (
                    <View key={index} style={portfolioViewStyle.infoRow}>
                        <View style={portfolioViewStyle.infoLabel}>
                            <Text style={portfolioViewStyle.infoLabelText}>{attr.label}</Text>
                        </View>
                        <Text style={portfolioViewStyle.infoValue}>{attr.value}</Text>
                    </View>
                ))}
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
                            <Icon name="next" size={20} color={theme.colors.quaternaryIcon} />
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
