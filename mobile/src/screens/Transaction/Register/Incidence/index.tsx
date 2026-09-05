import React, {useCallback, useEffect, useState} from 'react';
import {Alert, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import {useFocusEffect} from "@react-navigation/native";

import {PageSpecial} from "../../../../components/Page";
import {useTheme} from '../../../../contexts/ThemeContext';
import {getStyle} from '../../../../styles/styles';
import {getIncidenceStyle} from './styles';
import {validateSuccess} from "../../../../utils.ts";

import PlusIcon from '../../../assets/plus.svg';
import ChevronRightIcon from '../../../assets/chevron_right.svg';
import * as I from '../../../../interfaces/interfaces';

interface AbatimentoItem {
  id: string;
  conta: string;
  tipo: string;
  status: string;
  valor: number;
}

interface ImpactoItem {
  id: string;
  ativo: string;
  tipo: string;
  saldoGeral: number;
  operacao: string;
  valor: number;
}

const TransactionCreate = ({navigation, route}: {navigation: any, route: any}) => {
  const {theme} = useTheme();
  const style = getStyle(theme);
  const incidenceStyle = getIncidenceStyle(theme);

  const [loading, setLoading] = useState(false);
  const [transactionData, setTransactionData] = useState<any>(null);
  const [abatimentos, setAbatimentos] = useState<AbatimentoItem[]>([]);
  const [impactos, setImpactos] = useState<ImpactoItem[]>([]);
  const [totalAbatimentos, setTotalAbatimentos] = useState(0);
  const [resumoBalanceado, setResumoBalanceado] = useState(true);

  useFocusEffect(
    useCallback(() => {
      if (route.params?.data) {
        setTransactionData(route.params.data);
        loadTransactionDetails();
      }
    }, [route.params?.data])
  );

  useEffect(() => {
    if (route.params?.data) {
      setTransactionData(route.params.data);
      loadTransactionDetails();
    }
  }, []);

  const loadTransactionDetails = async () => {
    setLoading(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 500));
    } catch (error) {
      console.error('Error loading transaction details:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleBackClick = () => {
    navigation.goBack();
  };

  const handleAddAbatimento = () => {
    Alert.alert("Adicionar Abatimento", "Funcionalidade a ser implementada");
  };

  const handleAddImpacto = () => {
    Alert.alert("Adicionar Impacto", "Funcionalidade a ser implementada");
  };

  const handleReuseConfiguration = () => {
    Alert.alert("Reutilizar", "Usar configuração da última transação?", [
      { text: "Cancelar", style: "cancel" },
      { text: "Usar configuração", onPress: () => {} }
    ]);
  };

  const handleViewOtherTransactions = () => {
    Alert.alert("Ver Transações", "Ver outras transações desta operação");
  };

  const handleCancel = () => {
    navigation.goBack();
  };

  const handleSave = async () => {
    setLoading(true);
    try {
      if (!resumoBalanceado) {
        Alert.alert("Atenção!", "Os abatimentos devem ser iguais ao valor da transação");
        setLoading(false);
        return;
      }
      setLoading(false);
      let response: I.Response = {} as I.Response;
      response.success = true;
      validateSuccess(response, navigation, 'TransactionHome');
    } catch (error) {
      setLoading(false);
      Alert.alert("Erro", "Erro ao salvar transação");
    }
  };

  return (
    <PageSpecial
      title="Registrar Incidência"
      helpType="transaction_incidence"
      onBackClick={handleBackClick}
    >
      <View style={incidenceStyle.container}>
        {transactionData && (
          <View style={incidenceStyle.transactionCard}>
            <View style={incidenceStyle.cardHeader}>
              <View style={incidenceStyle.cardIconContainer}>
                <Text style={incidenceStyle.cardIcon}>📋</Text>
              </View>
              <View style={incidenceStyle.cardContent}>
                <Text style={incidenceStyle.cardTitle}>{transactionData.name || "Amortização de Financiamento"}</Text>
                <Text style={incidenceStyle.cardDate}>{transactionData.date || "24/05/2025"}</Text>
              </View>
              <Text style={incidenceStyle.cardValue}>{transactionData.value || "R$ 500,00"}</Text>
            </View>
          </View>
        )}

        <View style={incidenceStyle.reuseSection}>
          <View style={incidenceStyle.reuseContent}>
            <Text style={incidenceStyle.reuseTitle}>Reutilizar configuração da última transação desta operação?</Text>
            <Text style={incidenceStyle.reuseSubtext}>Usada em 18/05/2025 no valor de R$ 500,00</Text>
          </View>
          <TouchableOpacity 
            style={incidenceStyle.reuseButton}
            onPress={handleReuseConfiguration}
          >
            <Text style={incidenceStyle.reuseButtonText}>Usar configuração</Text>
          </TouchableOpacity>
        </View>

        <TouchableOpacity
          style={incidenceStyle.otherTransactionsLink}
          onPress={handleViewOtherTransactions}
        >
          <Text style={incidenceStyle.otherTransactionsText}>Ver outras transações desta operação</Text>
          <ChevronRightIcon width="20" height="20" fill={theme.colors.primaryBaseColor} />
        </TouchableOpacity>

        <View style={incidenceStyle.section}>
          <View style={incidenceStyle.sectionHeader}>
            <View>
              <Text style={incidenceStyle.sectionTitle}>Abatimento da transação</Text>
              <Text style={incidenceStyle.sectionSubtitle}>Defina de quais contas/patrimônios serão abatidos esta transação</Text>
            </View>
          </View>

          <TouchableOpacity
            style={incidenceStyle.addButton}
            onPress={handleAddAbatimento}
          >
            <PlusIcon width="20" height="20" fill={theme.colors.primaryBaseColor} />
            <Text style={incidenceStyle.addButtonText}>Adicionar abatimento</Text>
          </TouchableOpacity>

          <ScrollView style={incidenceStyle.itemsList} nestedScrollEnabled>
            {abatimentos.length > 0 ? (
              abatimentos.map((item) => (
                <View key={item.id} style={incidenceStyle.abatimentoCard}>
                  <View style={incidenceStyle.abatimentoInfo}>
                    <Text style={incidenceStyle.abatimentoMain}>{item.conta}</Text>
                    <Text style={incidenceStyle.abatimentoSecondary}>{item.tipo}</Text>
                  </View>
                  <View style={incidenceStyle.abatimentoRight}>
                    <View style={[incidenceStyle.statusBadge, {backgroundColor: theme.colors.quintenaryBaseColor}]}>
                      <Text style={incidenceStyle.statusText}>{item.status}</Text>
                    </View>
                    <Text style={incidenceStyle.abatimentoValue}>{`R$ ${item.valor.toFixed(2)}`}</Text>
                  </View>
                </View>
              ))
            ) : (
              <Text style={incidenceStyle.emptyText}>Nenhum abatimento adicionado</Text>
            )}
          </ScrollView>
        </View>

        <View style={incidenceStyle.resumoSection}>
          <Text style={incidenceStyle.resumoTitle}>Resumo dos abatimentos</Text>
          <Text style={incidenceStyle.resumoSubtitle}>O total de abatimentos deve ser igual ao valor da transação.</Text>
          
          <View style={incidenceStyle.resumoContent}>
            <View style={incidenceStyle.resumoItem}>
              <Text style={incidenceStyle.resumoLabel}>Valor da transação</Text>
              <Text style={[incidenceStyle.resumoValue, {color: theme.colors.tertiaryMonetaryColor}]}>
                {`R$ ${transactionData?.value?.replace('R$ ', '') || '500,00'}`}
              </Text>
            </View>

            <Text style={incidenceStyle.resumoEquals}>=</Text>

            <View style={incidenceStyle.resumoItem}>
              <Text style={incidenceStyle.resumoLabel}>Total de abatimentos</Text>
              <Text style={[incidenceStyle.resumoValue, {
                color: resumoBalanceado ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor
              }]}>
                {`R$ ${totalAbatimentos.toFixed(2)}`}
              </Text>
            </View>

            <View style={[
              incidenceStyle.resumoStatus,
              {backgroundColor: resumoBalanceado ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor}
            ]}>
              <Text style={incidenceStyle.resumoStatusText}>
                {resumoBalanceado ? "Balanceado" : "Desbalanceado"}
              </Text>
            </View>
          </View>
        </View>

        <View style={incidenceStyle.section}>
          <View style={incidenceStyle.sectionHeader}>
            <View>
              <Text style={incidenceStyle.sectionTitle}>Impacto da transação</Text>
              <Text style={incidenceStyle.sectionSubtitle}>Defina como este valor afetará seus portfólios e campos</Text>
            </View>
          </View>

          <TouchableOpacity
            style={incidenceStyle.addButton}
            onPress={handleAddImpacto}
          >
            <PlusIcon width="20" height="20" fill={theme.colors.primaryBaseColor} />
            <Text style={incidenceStyle.addButtonText}>Adicionar impacto</Text>
          </TouchableOpacity>

          <ScrollView style={incidenceStyle.itemsList} nestedScrollEnabled>
            {impactos.length > 0 ? (
              impactos.map((item) => (
                <View key={item.id} style={incidenceStyle.impactoCard}>
                  <View style={incidenceStyle.impactoInfo}>
                    <Text style={incidenceStyle.impactoMain}>{item.ativo}</Text>
                    <View style={incidenceStyle.impactoDetails}>
                      <Text style={incidenceStyle.impactoSecondary}>{item.tipo}</Text>
                      <Text style={incidenceStyle.impactoSecondary}>
                        {`Saldo total do portfólio: ${item.saldoGeral}`}
                      </Text>
                    </View>
                  </View>
                  <View style={incidenceStyle.impactoRight}>
                    <Text style={[
                      incidenceStyle.impactoValue,
                      {color: item.operacao === 'Somar' ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor}
                    ]}>
                      {item.operacao === 'Somar' ? '↑' : '↓'} {item.operacao}
                    </Text>
                    <Text style={incidenceStyle.impactoAmount}>{`R$ ${item.valor.toFixed(2)}`}</Text>
                  </View>
                </View>
              ))
            ) : (
              <Text style={incidenceStyle.emptyText}>Nenhum impacto adicionado</Text>
            )}
          </ScrollView>
        </View>

        <View style={incidenceStyle.buttonsContainer}>
          <TouchableOpacity
            style={[incidenceStyle.button, incidenceStyle.buttonCancel]}
            onPress={handleCancel}
            disabled={loading}
          >
            <Text style={incidenceStyle.buttonCancelText}>Cancelar</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[incidenceStyle.button, incidenceStyle.buttonSave]}
            onPress={handleSave}
            disabled={loading}
          >
            <Text style={incidenceStyle.buttonSaveText}>
              {loading ? "Salvando..." : "Salvar transação"}
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    </PageSpecial>
  );
};

export default TransactionCreate;
