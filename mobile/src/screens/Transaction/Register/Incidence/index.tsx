import React, {useCallback, useEffect, useState} from 'react';
import {Alert, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import {useFocusEffect} from "@react-navigation/native";
import {PageSpecial} from "../../../../components/Page";
import ChevronRightIcon from '../../../../assets/history.svg';
import Moment from "moment/moment";

import * as I from '../../../../interfaces/interfaces';
import {formatDateTime, toLocalDate, validateSuccess} from "../../../../utils";

import AuxiliaryButton from "../../../../components/AuxiliaryButton";
import Box from "../../../../components/Box";
import Tag from "../../../../components/Tag";
import Button from "../../../../components/Button";

import TransactionImpactModal from "../TransactionImpactModal";

import {useTheme} from '../../../../contexts/ThemeContext';
import {getStyle} from '../../../../styles/styles';
import {getIncidenceStyle} from './styles';
import {Transaction, TransactionImpact} from "../../../../interfaces/interfaces";
import {constants} from "../../../../constants";
import SwipeItem from "../../../../components/SwipeItem";
import {loadTransactionsByOperationInternalId} from "../../../../controller/transaction.controller";
import {CustomAlert} from "../../../../components/CustomAlert";

const TransactionIncidence = ({navigation, route}: { navigation: any, route: any }) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const incidenceStyle = getIncidenceStyle(theme);

    const [loading, setLoading] = useState(false);
    const [transactionData, setTransactionData] = useState<Transaction>(null);
    const [lastTransaction, setLastTransaction] = useState<Transaction>(null);
    const [deductions, setDeductions] = useState<TransactionImpact[]>([]);
    const [impacts, setImpacts] = useState<TransactionImpact[]>([]);
    const [totalAbatimentos, setTotalAbatimentos] = useState(0);
    const [resumoBalanceado, setResumoBalanceado] = useState(false);
    const [showModalImpact, setShowModalIncidence] = useState(false);
    const [isEditingImpact, setIsEditingIncidence] = useState(false);
    const [transactionImpactSelected, setTransactionIncidenceSelected] = useState<TransactionImpact>();
    const [impactTypeSelected, setImpactTypeSelected] = useState<number>();
    const [operationTypeSelected, setOperationTypeSelected] = useState<number>();

    useFocusEffect(
        useCallback(() => {
            if (route.params?.data) {
                setTransactionData(route.params.data);
                loadTransactionDetails(route.params.data);
            }
        }, [route.params?.data])
    );

    useEffect(() => {
        if (route.params?.data) {
            setTransactionData(route.params.data);
            loadTransactionDetails(route.params.data);
        }
    }, []);

    useEffect(() => {
        if (transactionData) {
            loadTransactionDeduction();
        }
    }, [transactionData]);

    useEffect(() => {
        if (transactionData) {
            if (totalAbatimentos === transactionData.Value) {
                setResumoBalanceado(true);
            } else {
                setResumoBalanceado(false);
            }
        }
    }, [totalAbatimentos]);

    const loadTransactionDetails = async (transaction: Transaction) => {
        setLoading(true);
        try {
            const transactions = await loadTransactionsByOperationInternalId(transaction.Operation.InternalId, true);
            setLastTransaction(transactions[0]);
        } catch (error) {
            console.error('Error loading transaction details:', error);
        } finally {
            setLoading(false);
        }
    };

    const getDeductionTypeByOperationType = (operationType: number) => {
        if (operationType === constants.operationType.revenue.Id) {
            return constants.transactionImpactOperationType.addition;
        } else if (operationType === constants.operationType.expense.Id) {
            return constants.transactionImpactOperationType.subtraction;
        }
    }

    const sum = (prev: number, next: number) => {
        return prev + next;
    }

    const loadTransactionDeduction = () => {
        let deductionsAux: TransactionImpact[] = transactionData?.Impacts?.filter(x => x.ImpactType === constants.transactionImpactType.deduction.Id);
        if (deductionsAux?.length ?? 0 === 0) {
            let deductionAux: TransactionImpact = {
                InternalId: 0,
                Id: 0,
                Value: transactionData.Value,
                Portfolio: transactionData.Portfolio,
                PortfolioAttribute: null,
                ImpactType: constants.transactionImpactType.deduction.Id,
                OperationType: getDeductionTypeByOperationType(transactionData.Operation.Type)
            };
            deductionsAux = [] as TransactionImpact[];
            deductionsAux.push(deductionAux);
        }
        setDeductions(deductionsAux);
        let total = deductionsAux.map(item => item.Value).reduce(sum);
        setTotalAbatimentos(total);
    }

    const getDescriptionOperationType = (operationTypeId: number) => {
        return Object.values(constants.transactionImpactOperationType).find(x => x.Id === operationTypeId).Name;
    }

    const getDescriptionImpactType = (impactTypeId: number) => {
        return Object.values(constants.transactionImpactType).find(x => x.Id === impactTypeId).Name;
    }

    const getPortfolioTypeById = (portfolioTypeId: number) => {
        return Object.values(constants.portfolioType)?.find(item => item.Id === portfolioTypeId)?.Name;
    }

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleAddDeduction = () => {
        setOperationTypeSelected(getDeductionTypeByOperationType(transactionData.Operation.Type));
        setImpactTypeSelected(constants.transactionImpactType.deduction.Id);
        setShowModalIncidence(true);
    };

    const handleAddIncidence = () => {
        setImpactTypeSelected(constants.transactionImpactType.incidence.Id)
        setShowModalIncidence(true);
    };
    
    const handleReuseConfiguration = async () => {
        await CustomAlert("Atenção!",
            "As suas configurações atuais serão perdidas. Deseja continuar?",
            async () => {
                applyReuseConfiguration();
            });
    };
    
    const applyReuseConfiguration = () => {
        let updated = lastTransaction.Impacts?.filter(x => x.ImpactType === constants.transactionImpactType.deduction.Id);
        setDeductions(updated);
        
        if (updated?.length > 0) {
            let total = updated.map(item => item.Value).reduce(sum);
            setTotalAbatimentos(total);
        } else {
            setTotalAbatimentos(0.00);
        }
        
        setImpacts(lastTransaction.Impacts?.filter(x => x.ImpactType === constants.transactionImpactType.incidence.Id));
    }

    const handleViewOtherTransactions = () => {
        Alert.alert("Ver Transações", "Ver outras transações desta operação");
    };

    const handleCancel = () => {
        navigation.goBack();
    };

    const handleClickDeduction = (item: I.TransactionImpact) => {
        setOperationTypeSelected(getDeductionTypeByOperationType(transactionData.Operation.Type));
        setImpactTypeSelected(constants.transactionImpactType.deduction.Id);
        setIsEditingIncidence(true);
        setTransactionIncidenceSelected(item);
        setShowModalIncidence(true);
    }

    const handleClickImpact = (item: I.TransactionImpact) => {
        setImpactTypeSelected(constants.transactionImpactType.deduction.Id);
        setIsEditingIncidence(true);
        setTransactionIncidenceSelected(item);
        setShowModalIncidence(true);
    }

    const saveTransactionDeduction = (item: I.TransactionImpact) => {

        setDeductions(current => {
            const index = current.findIndex(x => x.Portfolio.InternalId === item.Portfolio.InternalId);
            const updated = [...current];

            if (index >= 0) {
                updated.splice(index, 1);
            }

            updated.push(item);
            let total = updated.map(item => item.Value).reduce(sum);
            setTotalAbatimentos(total);
            return updated;
        });

        setOperationTypeSelected(null);
    }

    const saveTransactionImpact = (item: I.TransactionImpact) => {
        setImpacts(current => {
            const index = current.findIndex(x => x.PortfolioAttribute.Attribute.InternalId === item.PortfolioAttribute.Attribute.InternalId);
            const updated = [...current];

            if (index >= 0) {
                updated.splice(index, 1);
            }

            updated.push(item);
            return updated;
        });
    }

    const handleSaveTransactionIncidence = (item: I.TransactionImpact) => {
        if (impactTypeSelected == constants.transactionImpactType.deduction.Id) {
            saveTransactionDeduction(item);
        } else {
            saveTransactionImpact(item);
        }

        setTransactionIncidenceSelected(undefined);
        setIsEditingIncidence(false);
    }

    const deleteTransactionDeduction = (item: I.TransactionImpact) => {
        setDeductions(current => {
            const index = current.findIndex(x => x.Portfolio.InternalId === item.Portfolio.InternalId);

            if (index < 0) {
                return current;
            }

            return current.filter((_, currentIndex) => currentIndex !== index);
        });
    }

    const deleteTransactionImpact = (item: I.TransactionImpact) => {
        setImpacts(current => {
            const index = current.findIndex(x => x.PortfolioAttribute.Attribute.InternalId === item.PortfolioAttribute.Attribute.InternalId);

            if (index < 0) {
                return current;
            }

            return current.filter((_, currentIndex) => currentIndex !== index);
        });
    }

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
                                <Text
                                    style={incidenceStyle.cardTitle}>{transactionData.Operation.Name}</Text>
                                {<Text
                                    style={incidenceStyle.cardDate}>{Moment(transactionData.DataCriacao).format('DD/MM/YYYY  HH:mm:ss')}</Text>}
                            </View>
                            <Text style={incidenceStyle.cardValue}>R$ {transactionData.Value.toFixed(2)}</Text>
                        </View>
                    </View>
                )}

                <View style={incidenceStyle.reuseSection}>
                    {lastTransaction &&
                        <>
                            <View style={incidenceStyle.reuseContent}>
                                <Text style={incidenceStyle.reuseTitle}>Reutilizar configuração da última transação
                                    desta
                                    operação?</Text>

                            </View>
                            <View style={incidenceStyle.reuseContent}>
                                <Text
                                    style={incidenceStyle.reuseSubtext}>{`Usada em ${formatDateTime(lastTransaction.DataCriacao)} no valor de R$ ${lastTransaction.Value.toFixed(2)}`}</Text>
                                <TouchableOpacity
                                    style={incidenceStyle.reuseButton}
                                    onPress={handleReuseConfiguration}
                                >
                                    <Text style={incidenceStyle.reuseButtonText}>Usar configuração</Text>
                                </TouchableOpacity>
                            </View>
                            <View style={incidenceStyle.lineSeparetor}/>
                        </>
                    }
                    <View>
                        <TouchableOpacity
                            style={incidenceStyle.otherTransactionsLink}
                            onPress={handleViewOtherTransactions}
                        >
                            <Text style={incidenceStyle.otherTransactionsText}>Ver outras transações desta
                                operação</Text>
                            <ChevronRightIcon width="20" height="20" fill={theme.colors.primaryBaseColor}/>
                        </TouchableOpacity>
                    </View>
                </View>

                <View style={incidenceStyle.section}>
                    <View style={incidenceStyle.sectionHeader}>
                        <View>
                            <Text style={incidenceStyle.sectionTitle}>Abatimento da transação</Text>
                            <Text style={incidenceStyle.sectionSubtitle}>Defina de quais contas/patrimônios serão
                                abatidos esta transação</Text>
                        </View>
                    </View>

                    <AuxiliaryButton
                        text="Adicionar abatimento"
                        onPress={handleAddDeduction}
                        icon="plus"
                        iconColor={theme.colors.primaryTextColor}
                        type="secondary"
                    />

                    <ScrollView style={incidenceStyle.itemsList} nestedScrollEnabled>
                        {deductions.length > 0 ? (
                            deductions.map((item, index) => (
                                <SwipeItem
                                    key={item.Portfolio.Id + "_" + index.toString()}
                                    onPress={() => handleClickDeduction(item)}
                                    onSwipeLeft={() => deleteTransactionDeduction(item)}
                                >
                                    <View style={incidenceStyle.impactInfo}>
                                        <Text style={incidenceStyle.impactMain}>{item.Portfolio.Name}</Text>
                                    </View>
                                    <View style={incidenceStyle.impactRow}>
                                        <Text
                                            style={incidenceStyle.impactSecondary}>{getPortfolioTypeById(item.Portfolio.Type)}</Text>
                                        <Text style={incidenceStyle.impactAmount}>{`R$ ${item.Value.toFixed(2)}`}</Text>
                                    </View>
                                </SwipeItem>

                            ))
                        ) : (
                            <Text style={incidenceStyle.emptyText}>Nenhum abatimento adicionado</Text>
                        )}
                    </ScrollView>
                </View>

                <Box
                    style={incidenceStyle.resumoBox}
                    title="Resumo dos abatimentos"
                    description="O total de abatimentos deve ser igual ao valor da transação.">

                    <View style={incidenceStyle.resumoContent}>
                        <View style={incidenceStyle.resumoItem}>
                            <Text style={incidenceStyle.resumoLabel}>Valor da transação</Text>
                            <Text style={[incidenceStyle.resumoValue, {color: theme.colors.tertiaryMonetaryColor}]}>
                                {`R$ ${transactionData?.Value.toFixed(2)}`}
                            </Text>
                        </View>

                        <Text style={incidenceStyle.resumoEquals}>=</Text>

                        <View>
                            <Text style={incidenceStyle.resumoLabel}>Total de abatimentos</Text>
                            <Text style={[incidenceStyle.resumoValue, {
                                color: resumoBalanceado ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor
                            }]}>
                                {`R$ ${totalAbatimentos.toFixed(2)}`}
                            </Text>
                        </View>

                        <Tag
                            text={resumoBalanceado ? "Balanceado" : "Desbalanceado"}
                            textColor={resumoBalanceado ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor}
                            color={theme.colors.tertiaryBaseColor}
                        />
                    </View>
                </Box>

                <View style={incidenceStyle.section}>
                    <View style={incidenceStyle.sectionHeader}>
                        <View>
                            <Text style={incidenceStyle.sectionTitle}>Impact da transação</Text>
                            <Text style={incidenceStyle.sectionSubtitle}>Defina como este valor afetará seus portfólios
                                e campos</Text>
                        </View>
                    </View>

                    <AuxiliaryButton
                        text="Adicionar Impacto"
                        onPress={handleAddIncidence}
                        icon="plus"
                        iconColor={theme.colors.primaryTextColor}
                        type="secondary"
                    />

                    <ScrollView style={incidenceStyle.itemsList} nestedScrollEnabled>
                        {impacts.length > 0 ? (
                            impacts.map((item, index) => (
                                <SwipeItem
                                    key={index.toString()}
                                    onPress={() => handleClickImpact(item)}
                                    onSwipeLeft={() => deleteTransactionImpact(item)}
                                >
                                    <View style={incidenceStyle.impactInfo}>
                                        <Text style={incidenceStyle.impactMain}>{item.Portfolio.Name}</Text>
                                        <Text style={[
                                            incidenceStyle.impactValue,
                                            {color: item.OperationType === constants.transactionImpactOperationType.addition.Id ? theme.colors.tertiaryMonetaryColor : theme.colors.dangerTextColor}
                                        ]}>
                                            {item.OperationType === constants.transactionImpactOperationType.addition.Id ? '↑' : '↓'} {getDescriptionOperationType(item.OperationType)}
                                        </Text>
                                    </View>
                                    <View style={incidenceStyle.impactRow}>
                                        <Text
                                            style={incidenceStyle.impactSecondary}>{getPortfolioTypeById(item.Portfolio.Type)}</Text>
                                        <Text
                                            style={incidenceStyle.impactMain}>{item.PortfolioAttribute.Attribute.Name}</Text>
                                        <Text style={incidenceStyle.impactAmount}>{`R$ ${item.Value.toFixed(2)}`}</Text>
                                    </View>
                                </SwipeItem>
                            ))
                        ) : (
                            <Text style={incidenceStyle.emptyText}>Nenhum Impacto adicionado</Text>
                        )}
                    </ScrollView>
                </View>

                <View style={incidenceStyle.buttonsContainer}>
                    <Button
                        label={"Salvar"}
                        onPress={handleSave}
                        loading={loading}
                        disabled={loading}
                    />
                </View>
            </View>
            <TransactionImpactModal
                show={showModalImpact}
                setShow={setShowModalIncidence}
                addTransactionImpact={handleSaveTransactionIncidence}
                transactionImpact={transactionImpactSelected}
                isEditing={isEditingImpact}
                impactType={impactTypeSelected}
                impacts={constants.transactionImpactType.deduction.Id === impactTypeSelected ? deductions : impacts}
                operationType={operationTypeSelected}
            />
        </PageSpecial>
    );
};

export default TransactionIncidence;
