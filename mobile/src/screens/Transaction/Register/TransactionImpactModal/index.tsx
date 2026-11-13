import React, {useEffect, useState} from "react";
import {ActivityIndicator, Alert, ScrollView, Text, View} from "react-native";
import CurrencyInput from "react-native-currency-input";
import BottomModal from "../../../../components/BottomModal";
import Button from "../../../../components/Button";
import Select from "../../../../components/Select";
import {SelectItemRow} from "../../../../components/Select/SelectItem";

import * as I from "../../../../interfaces/interfaces";
import {constants} from "../../../../constants";
import {loadAllPortfolioInternalExcludingInternalIds} from "../../../../controller/portfolio.controller";
import {getDate} from "../../../../utils";

import {useTheme} from "../../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../../styles/styles.cadastro";
import {getTransactionImpactModalStyle} from "./styles";

interface TransactionImpactModalProps {
    show: boolean;
    setShow: (value: boolean) => void;
    addTransactionImpact: (item: I.TransactionImpact) => void;
    transactionImpact?: I.TransactionImpact;
    isEditing: boolean;
    impactType: number;
    impacts: I.TransactionImpact[];
    operationType?: number | null;
}

const TransactionImpactModal = (props: TransactionImpactModalProps) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const transactionImpactModalStyle = getTransactionImpactModalStyle(theme);

    const [loading, setLoading] = useState(false);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [portfolioId, setPortfolioId] = useState(0);
    const [portfolioAttributeInternalId, setPortfolioAttributeInternalId] = useState(0);
    const [operationType, setOperationType] = useState(constants.transactionImpactOperationType.addition.Id);
    const [value, setValue] = useState(0);

    useEffect(() => {
        if (!props.show) return;

        clearEditing();
        loadDataScreen();
        loadPortfolios();
    }, [props.show]);

    const loadPortfolios = async () => {
        setLoading(true);
        try {
            const excludedInternalIds = props.impacts.map(item => item.Portfolio.InternalId);
            const response = await loadAllPortfolioInternalExcludingInternalIds(null, excludedInternalIds, null);
            setPortfolios(response.data ?? []);
        } catch (error) {
            Alert.alert(
                "Erro",
                error instanceof Error ? error.message : "Não foi possível carregar os patrimônios."
            );
        } finally {
            setLoading(false);
        }
    };

    const loadDataScreen = () => {
        if (!props.isEditing || !props.transactionImpact) return;

        setPortfolioId(props.transactionImpact.Portfolio.Id);
        setPortfolioAttributeInternalId(props.transactionImpact.PortfolioAttribute.InternalId);
        setOperationType(props.transactionImpact.OperationType);
        setValue(props.transactionImpact.Value);
    };

    const getPortfolioAttributes = (): I.PortfolioAttribute[] => {
        const portfolio = portfolios.find(item => item.Id === portfolioId);
        return (portfolio?.Attributes ?? []).filter(item =>
            (item.Status === constants.status.active.Id &&
                item.Attribute.DataType === constants.attributeDataType.number.Id) ||
            item.InternalId === props.transactionImpact?.PortfolioAttribute.InternalId
        );
    };

    const handlePortfolioChange = (id: number) => {
        setPortfolioId(id);
        setPortfolioAttributeInternalId(0);
    };

    const handleApply = () => {
        const portfolio = portfolios.find(item => item.Id === portfolioId);
        const portfolioAttribute = getPortfolioAttributes().find(
            item => item.InternalId === portfolioAttributeInternalId
        );

        if (props.impactType === constants.transactionImpactType.incidence.Id) {
            if (!portfolio || !portfolioAttribute) {
                Alert.alert("Atenção!", "Selecione um patrimônio e um campo.");
                return;
            }
        }

        if (value <= 0) {
            Alert.alert("Atenção!", "Informe um valor maior que zero.");
            return;
        }

        const now = getDate();
        props.addTransactionImpact({
            ...props.transactionImpact,
            InternalId: props.transactionImpact?.InternalId ?? 0,
            Id: props.transactionImpact?.Id ?? 0,
            Value: value,
            Portfolio: portfolio,
            PortfolioAttribute: portfolioAttribute,
            ImpactType: props.impactType,
            OperationType: props.operationType.Id ?? operationType
        });
        props.setShow(false);
    };

    const clearEditing = () => {
        setPortfolioId(0);
        setPortfolioAttributeInternalId(0);
        setOperationType(constants.transactionImpactOperationType.addition.Id);
        setValue(0);
    };

    const operationTypes: SelectItemRow[] = [
        constants.transactionImpactOperationType.addition,
        constants.transactionImpactOperationType.subtraction,
    ];
    const portfolioAttributes: SelectItemRow[] = getPortfolioAttributes().map(item => ({
        Id: item.InternalId,
        Name: item.Attribute.Name,
    }));

    return (
        <BottomModal show={props.show} setShow={props.setShow}>
            <ScrollView style={transactionImpactModalStyle.areaFields}>
                {loading ? (
                    <ActivityIndicator color={theme.colors.primaryBaseColor}/>
                ) : (
                    <>
                        <View style={styleCadastro.areaField}>
                            <Select
                                label="Patrimônio"
                                value={portfolioId}
                                setValue={handlePortfolioChange}
                                data={portfolios}
                            />
                        </View>
                        {props.impactType === constants.transactionImpactType.incidence.Id &&
                            <>
                                <View style={styleCadastro.areaField}>
                                    <Select
                                        label="Campo"
                                        value={portfolioAttributeInternalId}
                                        setValue={setPortfolioAttributeInternalId}
                                        data={portfolioAttributes}
                                    />
                                </View>
                                <View style={styleCadastro.areaField}>
                                    <Select
                                        label="Operação"
                                        value={operationType}
                                        setValue={setOperationType}
                                        data={operationTypes}
                                    />
                                </View>
                            </>
                        }
                        <View style={transactionImpactModalStyle.areaValue}>
                            <Text style={transactionImpactModalStyle.labelValue}>Valor do impacto</Text>
                            <CurrencyInput
                                style={transactionImpactModalStyle.inputValue}
                                value={value}
                                onChangeValue={(newValue: number | null) => setValue(newValue ?? 0)}
                                prefix="R$"
                                delimiter="."
                                separator=","
                                precision={2}
                                keyboardType="numeric"
                            />
                        </View>
                    </>
                )}
            </ScrollView>
            <View style={transactionImpactModalStyle.areaButtonApply}>
                <Button
                    label={props.isEditing ? "Salvar" : "Concluir"}
                    onPress={handleApply}
                    loading={loading}
                    disabled={loading}
                />
            </View>
        </BottomModal>
    );
};

export default TransactionImpactModal;
