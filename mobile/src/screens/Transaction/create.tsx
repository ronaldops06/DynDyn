import CheckBox from '@react-native-community/checkbox';
import DateTimePicker from '@react-native-community/datetimepicker';
import {RouteProp, useNavigation, useRoute} from '@react-navigation/core';
import {StackNavigationProp} from '@react-navigation/stack';
import Moment from 'moment';
import React, {useEffect, useState} from 'react';
import {Alert, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import CurrencyInput from 'react-native-currency-input';
import RadioGroup, {RadioButtonProps} from 'react-native-radio-buttons-group';

import ClockIcon from '../../assets/clock.svg';
import PrevIcon from '../../assets/nav_prev.svg';
import TodayIcon from '../../assets/today.svg';
import TrashIcon from '../../assets/trash.svg';
import HistoryIcon from '../../assets/history.svg';
import ButtonSelectBar, {ButtonsSelectedProps} from '../../components/ButtonSelectBar';
import Picker from '../../components/CustomPicker';
import TextInput from '../../components/CustomTextInput';
import OperationModal from './OperationModal';
import {TypesCategory, TypesTransaction} from '../../enums/enums';
import * as I from '../../interfaces/interfaces';
import {RootStackParamList} from '../RootStackParams';

import {alterTransaction, createTransaction, excludeTransaction} from '../../controller/transaction.controller';
import {loadAllCategory} from "../../controller/category.controller.tsx";
import {loadAllPortfolio} from "../../controller/portfolio.controller.tsx";

import {style} from '../../styles/styles';
import {styleCadastro} from '../../styles/styles.cadastro';
import {transactionCreateStyle} from './create.styles';
import {constants} from "../../constants";
import {validateLogin, validateSuccess} from "../../utils.ts";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'TransactionCreate'>;

const radioButtonsData: RadioButtonProps[] = [{
    id: '1',
    label: 'Repetir',
    value: 'repetir',
    color: '#6E8BB8',
    size: 16,
    labelStyle: transactionCreateStyle.labelRadioRepeat,
    selected: true
},
    {
        id: '2',
        label: 'Parcelar',
        value: 'parcelar',
        color: '#6E8BB8',
        size: 16,
        labelStyle: transactionCreateStyle.labelRadioRepeat,
        selected: false
    }];

const TransactionCreate = () => {
    //let stepInput = React.createRef<CustomTextInput>();
    const stepInput: React.RefObject<any> = React.createRef();

    const navigation = useNavigation<homeScreenProp>();
    const route = useRoute<RouteProp<RootStackParamList, 'TransactionCreate'>>();
    const transactionId = route.params?.data?.Id ?? 0;
    const transactionInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;
    const baseOperation = {} as I.Operation;

    const [typeSelected, setTypeSelected] = useState(1);
    const [showDate, setShowDate] = useState(false);
    const [mode, setMode] = useState('date');
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
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
    const [isSalary, setIsSalary] = useState(false);
    const [valueMultiply, setValeuMultiply] = useState(false);
    const [valueRadioRepeatSelectedId, setValueRadioRepeatSelectedId] = useState<string>();
    const [valueTimes, setValueTimes] = useState(1);
    const [showModal, setShowModal] = useState(false);

    const getLists = async () => {
        let responseCategories = await loadAllCategory(TypesCategory.Operation, null);
        validateLogin(responseCategories, navigation);
        
        let responsePortfolios = await loadAllPortfolio(null);
        validateLogin(responsePortfolios, navigation);
        
        setCategories(responseCategories?.data ?? []);
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
            setValueCategory(data.Operation.Category.Id);
            setValuePortfolio(data.Portfolio.Id);
            if (data.DestinationPortfolio)
                setValueDestPortfolio(data.DestinationPortfolio.Id);
            setValueNote(data.Observation);
            setValueConsolidated(data.Consolidated);
            setIsSalary(data.Operation.Salary);
            setValueTimes(data.TotalInstallments);
            setValeuMultiply(data.Operation.Recurrent || (data.TotalInstallments > 1));
            setValueRadioRepeatSelectedId(data.Operation.Recurrent ? '1' : '2');
        }
    };

    const clearOperation = () => {
        setOperation(baseOperation);
        setValueDescription("");
        setIsSalary(false);
    };

    const setOperationDefault = () => {
        setValueDescription('Transferência');
        var categoryTranference = categories.find(x => x.Name == 'Transferência');
        if (categoryTranference !== undefined) {
            setValueCategory(categoryTranference.Id);
        }
    };

    useEffect(() => {
        stepInput.current.focus();
        if (isEditing) {
            loadDataSreen();
        }
    }, [])

    useEffect(() => {
        setMode('date');
        if (!isEditing) {
            setValueDate(Moment(new Date()).format('DD/MM/YYYY'))
            setValueTime(Moment(new Date()).format('HH:mm:ss'));
            clearOperation();
        }
        getLists();

        if (typeSelected == TypesTransaction.Transference) {
            setOperationDefault();
            setValueConsolidated(true);
        }
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

        if (valueRadioRepeatSelectedId === '1' && valueTimes == 0) {
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

        if (mode == 'date') {
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
        if (route.params?.onGoBack)
            route.params.onGoBack(constants.actionNavigation.none);
        
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
                        let response= await excludeTransaction(data);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, route);
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
        setIsSalary(item.Salary);
    };

    const getRadioButtonsData = (): RadioButtonProps[] => {
        return radioButtonsData.map(item => {
            item.disabled = isEditing;

            return item;
        });
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        const data = route.params?.data ?? {} as I.Transaction;

        let operationDTO = {} as I.Operation;
        if (operation.Id !== undefined) {
            operationDTO.Id = operation.Id;
            operationDTO.InternalId = operation.InternalId;
        }
        operationDTO.Name = valueDescription;
        operationDTO.Type = typeSelected;
        operationDTO.Category = categories.find(x => x.Id === valueCategory) ?? {} as I.Category;
        operationDTO.Recurrent = valueRadioRepeatSelectedId === '1' ?? false;
        operationDTO.Salary = isSalary ?? false;
        operationDTO.Status = 1;

        let transactionDTO = {} as I.Transaction;
        transactionDTO.Id = transactionId;
        transactionDTO.InternalId = transactionInternalId;
        transactionDTO.Value = valueValue;
        transactionDTO.Observation = valueNote;
        transactionDTO.Consolidated = valueConsolidated;
        transactionDTO.TotalInstallments = valueTimes;
        transactionDTO.DataCriacao = new Date(Moment(valueDate + " " + valueTime, 'DD/MM/YYYY HH:mm:ss').local().format('YYYY-MM-DD HH:mm:ss'));
        transactionDTO.DataAlteracao = new Date(Moment().utc(true).format('YYYY-MM-DD HH:mm:ss'))
        transactionDTO.Portfolio = portfolios.find(x => x.Id === valuePortfolio) ?? {} as I.Portfolio;
        transactionDTO.DestinationPortfolio = (valueDestPortfolio > 0) ? portfolios.find(x => x.Id === valueDestPortfolio) ?? null : null;
        transactionDTO.Operation = operationDTO;
        transactionDTO.ParentTransaction = null;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterTransaction(data, transactionDTO);
        else
            response = await createTransaction(transactionDTO);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, route);
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill="#F1F1F1"/>
                    </TouchableOpacity>
                    {isEditing &&
                        <TouchableOpacity
                            style={styleCadastro.buttonTrash}
                            onPress={handleTrashClick}>
                            <TrashIcon width="35" height="35" fill="#F1F1F1"/>
                        </TouchableOpacity>}
                </View>
                <View style={styleCadastro.viewBodyCadastro}>
                    <ButtonSelectBar
                        buttons={getButtonsSelectedBar()}
                        valueSelected={typeSelected}
                        handleValueSelected={setTypeSelected}
                        disabled={isEditing}
                    />
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
                            />
                        </View>
                        {!typeSelectedIsTransference() &&
                            <TextInput
                                text={"Descrição"}
                                isMoveText={false}
                                value={valueDescription}
                                setValue={setValueDescription}
                                icon={<HistoryIcon width={30} fill="#6E8BB8"/>}
                                onPressIcon={handleOperationsClick}
                            />
                        }
                        <View style={transactionCreateStyle.areaDateTime}>
                            <TextInput
                                text={"Data"}
                                isMoveText={false}
                                value={valueDate}
                                setValue={setValueDate}
                                icon={<TodayIcon width={30} fill="#6E8BB8"/>}
                                onPressIcon={() => handleDateTimeClick(true, 'date')}
                                width={"49%"}
                            />
                            <TextInput
                                text={"Hora"}
                                isMoveText={false}
                                value={valueTime}
                                setValue={setValueTime}
                                icon={<ClockIcon width={30} fill="#6E8BB8"/>}
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
                            <Picker
                                text={"Categoria"}
                                value={valueCategory}
                                setValue={setValueCategory}
                                data={categories}
                            />
                        }
                        <Picker
                            text={"Conta" + ((typeSelected === TypesTransaction.Transference) ? " Origem" : "")}
                            value={valuePortfolio}
                            setValue={setValuePortfolio}
                            data={portfolios}
                        />
                        {typeSelectedIsTransference() &&
                            <Picker
                                text={"Conta Destino"}
                                value={valueDestPortfolio}
                                setValue={setValueDestPortfolio}
                                data={portfolios}
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
                                <View style={styleCadastro.areaCheckbox}>
                                    <CheckBox
                                        value={valueConsolidated}
                                        onValueChange={setValueConsolidated}
                                        tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
                                    />
                                    <Text
                                        style={styleCadastro.textCheckbox}>{(typeSelected == 1) ? "Recebido" : (typeSelected == 2) ? "Pago" : ""}</Text>
                                </View>
                                <View style={styleCadastro.areaCheckbox}>
                                    <CheckBox
                                        disabled={operation.Id !== undefined}
                                        value={isSalary}
                                        onValueChange={setIsSalary}
                                        tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
                                    />
                                    <Text style={styleCadastro.textCheckbox}>Salário</Text>
                                </View>
                                <View style={styleCadastro.areaCheckbox}>
                                    <CheckBox
                                        value={valueMultiply}
                                        onValueChange={setValeuMultiply}
                                        tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
                                        disabled={isEditing}
                                    />
                                    <Text style={styleCadastro.textCheckbox}>Multiplicar</Text>
                                </View>

                                {valueMultiply &&
                                    <View style={transactionCreateStyle.areaRepeat}>
                                        <RadioGroup
                                            radioButtons={getRadioButtonsData()}
                                            onPress={setValueRadioRepeatSelectedId}
                                            selectedId={valueRadioRepeatSelectedId}
                                            layout="row"
                                            containerStyle={transactionCreateStyle.radioRepeat}
                                        />
                                        {valueRadioRepeatSelectedId && valueRadioRepeatSelectedId === '2' &&
                                            <View style={transactionCreateStyle.areaTimes}>
                                                <TextInput
                                                    text={"Vezes"}
                                                    isMoveText={false}
                                                    value={valueTimes?.toString()}
                                                    setValue={setValueTimes}
                                                    width={"100%"}
                                                    editable={!isEditing}
                                                    //messageText={(valueTimes != 0 ) ? (valueTimes.toString() + "x R$ " + (valueValue / valueTimes).toString()) : ""}                                    
                                                />
                                            </View>}
                                    </View>
                                }
                            </>
                        }
                    </View>
                    <View style={styleCadastro.areaButtonSave}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleSaveClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Salvar</Text>
                        </TouchableOpacity>
                    </View>
                    <OperationModal
                        show={showModal}
                        setShow={setShowModal}
                        setOperation={handleItemOperationClick}
                        tipoOperation={typeSelected}
                    />
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default TransactionCreate;