import CheckBox from '@react-native-community/checkbox';
import DateTimePicker from '@react-native-community/datetimepicker';
import Moment from 'moment';
import React, {useCallback, useEffect, useState} from 'react';
import {Alert, Text, TouchableOpacity, View} from 'react-native';
import CurrencyInput from 'react-native-currency-input';

import {PageRegister} from "../../../components/Page";
import ClockIcon from '../../../assets/clock.svg';
import TodayIcon from '../../../assets/today.svg';
import HistoryIcon from '../../../assets/history.svg';
import CalculatorIcon from '../../../assets/calculate.svg';
import CopyIcon from '../../../assets/copy.svg';
import ButtonSelectBar, {ButtonsSelectedProps} from '../../../components/ButtonSelectBar';
import Picker from '../../../components/CustomPicker';
import TextInput from '../../../components/CustomTextInput';
import OperationModal from '../OperationModal';
import {TypesCategory, TypesTransaction} from '../../../enums/enums';
import * as I from '../../../interfaces/interfaces';

import {alterTransaction, createTransaction, excludeTransaction} from '../../../controller/transaction.controller';
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import {loadAllPortfolioInternal} from "../../../controller/portfolio.controller.tsx";

import {constants} from "../../../constants";
import {getCurrentStack, getDate, toLocalDate, validateLogin, validateSuccess} from "../../../utils.ts";

import {useTheme} from '../../../contexts/ThemeContext';
import {getStyleCadastro} from '../../../styles/styles.cadastro';
import {getTransactionCreateStyle} from './styles';
import Calculator from "../../../components/Calculator";
import AuxiliaryButton from "../../../components/AuxiliaryButton";
import {useFocusEffect} from "@react-navigation/native";
import Select from "../../../components/Select";

const TransactionCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const transactionCreateStyle = getTransactionCreateStyle(theme);

    const stepInput: React.RefObject<any> = React.createRef();

    const paramTransactionId = route.params?.data?.Id ?? 0;
    const paramTransactionInternalId = route.params?.data?.InternalId ?? 0;
    const paramIsEditing = route.params?.isEditing ?? false;
    const baseOperation = {} as I.Operation;

    const [stack, setStack] = useState<string>("");
    const [loading, setLoading] = useState(false);
    const [typeSelected, setTypeSelected] = useState(2);
    const [showDate, setShowDate] = useState(false);
    const [mode, setMode] = useState('date');
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [transactionId, setTransactionId]  = useState(0);
    const [transactionInternalId, setTransactionInternalId]  = useState(0);
    const [isEditing, setIsEditing]  = useState(false);
    const [valueValue, setValueValue] = useState(0);
    const [operation, setOperation] = useState<I.Operation>(baseOperation);
    const [valueDescription, setValueDescription] = useState("");
    const [valueDate, setValueDate] = useState("");
    const [valueTime, setValueTime] = useState("");
    const [valueCategory, setValueCategory] = useState(0);
    const [valuePortfolio, setValuePortfolio] = useState(0);
    const [valueDestPortfolio, setValueDestPortfolio] = useState(0);
    const [valueNote, setValueNote] = useState("");
    const [valueConsolidated, setValueConsolidated] = useState(false);
    const [isRecurrent, setIsRecurrent] = useState<boolean>(false);
    const [isPaindInstallments, setIsPaindInstallments] = useState<boolean>(false);
    const [installment, setInstallment] = useState(1);
    const [valueTimes, setValueTimes] = useState(1);
    const [showModal, setShowModal] = useState(false);
    const [showCalculator, setShowCalculator] = useState(false);

    useFocusEffect(
        useCallback(() => {
            if (route.params?.referenceId !== undefined) {
                let reference = route.params?.reference;

                if (reference === constants.operations.category)
                {
                    getCategories();

                    setValueCategory(route.params?.referenceId);
                } else if (reference === constants.operations.portfolio) {
                    getPortifolios();
                    
                    setValuePortfolio(route.params?.referenceId);
                }
            }
        }, [route.params?.actionNavigation])
    );
    
    const getLists = async () => {
        await getCategories();
        await getPortifolios();
    }
    
    const getCategories = async () => {
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Operation, null, true);
        validateLogin(responseCategories, navigation);
        setCategories(responseCategories?.data ?? []);
    }

    const getPortifolios = async () => {
        let groupsPortfolios = [];
        groupsPortfolios.push(constants.portfolioGroupType.ativo.contasBancarias.Id);
        groupsPortfolios.push(constants.portfolioGroupType.passivo.contasBancarias.Id);
        
        let responsePortfolios = await loadAllPortfolioInternal(null, groupsPortfolios, true);
        validateLogin(responsePortfolios, navigation);
        setPortfolios(responsePortfolios?.data ?? []);
    }

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data !== undefined && data !== null) {
            setTypeSelected(data.Operation.Type ?? 0);
            setValueValue(data.Value);
            setOperation(data.Operation);
            setValueDescription(data.Operation.Name);
            setValueDate(Moment(data.DataCriacao).local().format('DD/MM/YYYY'));
            setValueTime(Moment(data.DataCriacao).local().format('HH:mm:ss'));
            setValueNote(data.Observation);
            setValueConsolidated(data.Consolidated);
            setValueTimes(data.TotalInstallments);
            setIsRecurrent(data.Operation.Recurrent);
            setIsPaindInstallments((data.TotalInstallments > 1))
            setInstallment(data.Installment);
            setValueCategory(data.Operation.Category.Id);
            setValuePortfolio(data.Portfolio.Id);
            if (data.DestinationPortfolio)
                setValueDestPortfolio(data.DestinationPortfolio.Id);
        }
    };

    const clearOperation = () => {
        setOperation(baseOperation);
        setValueDescription("");
    };

    const setOperationDefault = () => {
        setValueDescription('Transferência Entre Contas');
        var categoryTranference = categories.find(x => x.Name == 'Transferência Contas');
        if (categoryTranference !== undefined) {
            setValueCategory(categoryTranference.Id);
        }
    };

    useEffect(() => {
        stepInput.current.focus();
        
        setIsEditing(paramIsEditing);
        setTransactionId(paramTransactionId);
        setTransactionInternalId(paramTransactionInternalId);
        
        if (paramIsEditing) {
            loadDataSreen();
        }

        const tab = getCurrentStack(navigation);
        setStack(tab);
    }, [])

    useEffect(() => {
        setMode('date');
        if (!paramIsEditing) {
            setValueDate(Moment(new Date()).format('DD/MM/YYYY'))
            setValueTime(Moment(new Date()).format('HH:mm:ss'));
            clearOperation();
        }
        getLists();

        if (paramIsEditing)
            return;

        if (typeSelected == TypesTransaction.Transference) {
            setOperationDefault();
            setValueConsolidated(true);
        } else {
            setValueConsolidated(false);
            setValueCategory(null);
            setValueDestPortfolio(0);
        }
        stepInput.current.focus();
    }, [typeSelected]);

    const validateRequiredFields = () => {

        if (valueValue === 0) {
            Alert.alert("Atenção!", "O valor deve ser informado.");
            return false;
        }

        if (typeSelected !== TypesTransaction.Transference) {
            if (valueDescription === "") {
                Alert.alert("Atenção!", "A descrição deve ser informada.");
                return false;
            }

            if (valueCategory === 0) {
                Alert.alert("Atenção!", "A categoria deve ser selecionada.");
                return false;
            }
        }

        if (valuePortfolio === 0) {
            Alert.alert("Atenção!", "A conta deve ser selecionada.");
            return false;
        }

        if (valueDestPortfolio == 0 && typeSelected === TypesTransaction.Transference) {
            Alert.alert("Atenção!", "A conta de destino deve ser selecionada.");
            return false;
        }

        if (isPaindInstallments && valueTimes == 0) {
            Alert.alert("Atenção!", "A quantidade de vezes deve ser informada.");
            return false;
        }

        return true;
    }

    const handleDateTimeClick = (showDate: boolean, mode: string) => {
        setShowDate(showDate);
        setMode(mode);
    };

    const onChangeDate = (event: Event, date?: Date) => {
        setShowDate(false);

        const currentDate = date;

        if (mode === 'date') {
            setValueDate(Moment(currentDate).local().format('DD/MM/YYYY'));
        } else {
            setValueTime(Moment(currentDate).local().format('HH:mm:ss'));
        }
    };

    const getTextValueStyle = () => {
        return [transactionCreateStyle.inputValue, (typeSelected == TypesTransaction.Revenue ?
            transactionCreateStyle.inputValueRevenue :
            (typeSelected == TypesTransaction.Transference ?
                transactionCreateStyle.inputValueTransfer :
                transactionCreateStyle.inputValueExpense))]
    };

    const typeSelectedIsTransference = () => {
        return typeSelected == TypesTransaction.Transference;
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Esta transação será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        const data = route.params?.data ?? {} as I.Transaction;
                        let response = await excludeTransaction(data);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, "TransactionHome");
                    }
                }
            ],
            {cancelable: false}
        );
    };

    const getButtonsSelectedBar = (): ButtonsSelectedProps[] => {
        let buttonsSelectedBar: ButtonsSelectedProps[] = [];

        Object.values(constants.operationType).map(type => {
            buttonsSelectedBar.push({text: type.Name, value: type.Id});
        });

        return buttonsSelectedBar;
    }

    const handleOperationsClick = () => {
        setShowModal(true);
    };

    const handleItemOperationClick = (item: I.Operation) => {
        setOperation(item);
        setValueDescription(item.Name);
        setValueCategory(item.Category.Id);
        setIsRecurrent(item.Recurrent);
    };

    const handlePaindInstallmentSelect = (value: boolean) => {
        if (!value)
            setValueTimes(1);

        setIsPaindInstallments(value);
    }
    
    const handleCopyClick = () => {
        setIsEditing(false);
        setTransactionId(0);
        setTransactionInternalId(0);
        Alert.alert("Atenção!", "Você gerou uma cópia da transação.");
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);

        const data = route.params?.data ?? {} as I.Transaction;

        let operationDTO = {} as I.Operation;
        if (operation.Id !== undefined) {
            operationDTO.Id = operation.Id;
            operationDTO.InternalId = operation.InternalId;
        }

        operationDTO.Name = valueDescription;
        operationDTO.Type = typeSelected;
        operationDTO.Category = categories.find(x => x.Id === valueCategory) ?? {} as I.Category;
        operationDTO.Recurrent = isRecurrent ?? false;
        operationDTO.Salary = false;
        operationDTO.Status = 1;

        let transactionDTO = {} as I.Transaction;
        transactionDTO.Id = transactionId;
        transactionDTO.InternalId = transactionInternalId;
        transactionDTO.Value = valueValue;
        transactionDTO.Observation = valueNote;
        transactionDTO.Consolidated = valueConsolidated;
        transactionDTO.Installment = installment ?? 1;
        transactionDTO.TotalInstallments = valueTimes;
        transactionDTO.DataCriacao = toLocalDate(`${valueDate} ${valueTime}`);
        transactionDTO.DataAlteracao = getDate();
        transactionDTO.Portfolio = portfolios.find(x => x.Id === valuePortfolio) ?? {} as I.Portfolio;
        transactionDTO.DestinationPortfolio = (valueDestPortfolio > 0) ? portfolios.find(x => x.Id === valueDestPortfolio) ?? null : null;
        transactionDTO.Operation = operationDTO;
        transactionDTO.ParentTransaction = null;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterTransaction(data, transactionDTO);
        else
            response = await createTransaction(transactionDTO);

        setLoading(false);

        validateLogin(response, navigation);
        if (route.params?.sourceScreen === "Home")
            validateSuccess(response, navigation, 'HomeHome');
        else
            validateSuccess(response, navigation, 'TransactionHome');
    };

    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"transaction_register"}
            isEditing={isEditing}
            isLoading={loading}>
            <View style={transactionCreateStyle.areaButtonType}>
                <ButtonSelectBar
                    buttons={getButtonsSelectedBar()}
                    valueSelected={typeSelected}
                    handleValueSelected={setTypeSelected}
                    disabled={isEditing}
                />
            </View>
            <View style={styleCadastro.areaFields}>
                <View style={transactionCreateStyle.areaValue}>
                    <CurrencyInput
                        ref={stepInput}
                        style={getTextValueStyle()}
                        value={valueValue}
                        onChangeValue={(value: number) => setValueValue(value)}
                        prefix="R$"
                        delimiter="."
                        separator=","
                        precision={2}
                        autoFocus={!isEditing}
                        keyboardType="numeric"
                    />
                </View>
                <View>
                    <TouchableOpacity
                        style={transactionCreateStyle.areaCalculator}
                        onPress={() => setShowCalculator(true)}>
                        <CalculatorIcon width="35" height="35" fill={theme.colors.quintenaryIcon}/>
                    </TouchableOpacity>
                </View>
                {!typeSelectedIsTransference() &&
                    <TextInput
                        text={"Descrição"}
                        isMoveText={false}
                        value={valueDescription}
                        setValue={setValueDescription}
                        icon={<HistoryIcon width={30} fill={theme.colors.quintenaryIcon}/>}
                        onPressIcon={handleOperationsClick}
                    />
                }
                <View style={transactionCreateStyle.areaDateTime}>
                    <TextInput
                        text={"Data"}
                        isMoveText={false}
                        value={valueDate}
                        setValue={setValueDate}
                        icon={<TodayIcon width={30} fill={theme.colors.quintenaryIcon}/>}
                        onPressIcon={() => handleDateTimeClick(true, 'date')}
                        width={"49%"}
                    />
                    <TextInput
                        text={"Hora"}
                        isMoveText={false}
                        value={valueTime}
                        setValue={setValueTime}
                        icon={<ClockIcon width={30} fill={theme.colors.quintenaryIcon}/>}
                        onPressIcon={() => handleDateTimeClick(true, 'time')}
                        width={"49%"}
                    />
                    {showDate && <DateTimePicker
                        testID="dateTimePicker"
                        value={new Date(Number(valueDate.split('/')[2]), Number(valueDate.split('/')[1]) - 1, Number(valueDate.split('/')[0]), Number(valueTime.split(':')[0]), Number(valueTime.split(':')[1]), Number(valueTime.split(':')[2]))}
                        mode={mode}
                        is24Hour={true}
                        display="default"
                        onChange={onChangeDate}
                    />}
                </View>
                {!typeSelectedIsTransference() &&
                    <Select
                        label={"Categoria"}
                        value={valueCategory}
                        setValue={setValueCategory}
                        data={categories}
                        parentScreen={stack}
                        registerScreen={"CategoryCreate"}
                        navigation={navigation}
                        sourceScreen={route.name}
                        reference={constants.operations.category}
                    />
                }
                <Select
                    label={"Conta" + ((typeSelected === TypesTransaction.Transference) ? " Origem" : "")}
                    value={valuePortfolio}
                    setValue={setValuePortfolio}
                    data={portfolios}
                    parentScreen={stack}
                    registerScreen={"AccountCreate"}
                    navigation={navigation}
                    sourceScreen={route.name}
                    reference={constants.operations.portfolio}
                />
                {typeSelectedIsTransference() &&
                    <Select
                        label={"Conta Destino"}
                        value={valueDestPortfolio}
                        setValue={setValueDestPortfolio}
                        data={portfolios}
                        parentScreen={stack}
                        registerScreen={"AccountCreate"}
                        navigation={navigation}
                        sourceScreen={route.name}
                        reference={constants.operations.destinationPortfolio}
                    />
                }
                <TextInput
                    text={"Observação"}
                    isMoveText={false}
                    value={valueNote}
                    setValue={setValueNote}
                />
                {!typeSelectedIsTransference() &&
                    <>
                        <View style={transactionCreateStyle.areaChecks}>
                            <View style={styleCadastro.areaCard}>
                                <CheckBox
                                    value={isRecurrent}
                                    onValueChange={setIsRecurrent}
                                    tintColors={{
                                        true: theme.colors.primaryBaseColor,
                                        false: theme.colors.primaryBaseColor
                                    }}
                                    disabled={operation.Id !== undefined}
                                />
                                <Text style={styleCadastro.textCheckbox}>Recorrente</Text>
                            </View>
                        </View>
                        <View style={transactionCreateStyle.areaRepeat}>
                            <View style={styleCadastro.areaCard}>
                                <CheckBox
                                    value={isPaindInstallments}
                                    onValueChange={handlePaindInstallmentSelect}
                                    tintColors={{
                                        true: theme.colors.primaryBaseColor,
                                        false: theme.colors.primaryBaseColor
                                    }}
                                    disabled={isEditing || isRecurrent}
                                />
                                <Text style={styleCadastro.textCheckbox}>Parcelado</Text>
                            </View>
                            {isPaindInstallments &&
                                <View style={transactionCreateStyle.areaTimes}>
                                    <TextInput
                                        text={"Vezes"}
                                        isMoveText={false}
                                        value={valueTimes?.toString() ?? ""}
                                        setValue={setValueTimes}
                                        width={"100%"}
                                        editable={!isEditing}
                                        messageText={(valueTimes != 0) ? `Total: R$ ${(valueValue * valueTimes).toFixed(2)}` : ""}
                                    />
                                </View>
                            }
                        </View>
                        <View style={styleCadastro.areaCard}>
                            <CheckBox
                                value={valueConsolidated}
                                onValueChange={setValueConsolidated}
                                tintColors={{true: theme.colors.primaryBaseColor, false: theme.colors.primaryBaseColor}}
                            />
                            <Text
                                style={styleCadastro.textCheckbox}>{(typeSelected == 1) ? "Recebido" : (typeSelected == 2) ? "Pago" : ""}</Text>
                        </View>
                    </>
                }
            </View>
            {isEditing &&
                <View style={transactionCreateStyle.areaMoreActions}>
                    <AuxiliaryButton
                        text="Copiar Transação"
                        onPress={handleCopyClick}
                        icon="copy"
                        iconColor={theme.colors.quaternaryIcon}
                        type="secondary"
                    />
                </View>
            }
            <OperationModal
                show={showModal}
                setShow={setShowModal}
                setOperation={handleItemOperationClick}
                typeOperation={typeSelected}
            />
            <Calculator
                value={valueValue}
                setValue={setValueValue}
                show={showCalculator}
                setShow={setShowCalculator}/>
        </PageRegister>
    )
}

export default TransactionCreate;