import React, { useState, useEffect } from 'react';
import { Alert, ScrollView, SafeAreaView, View, TouchableOpacity, Text, TextInput as Input } from 'react-native';
import CurrencyInput from 'react-native-currency-input';
import DateTimePicker from '@react-native-community/datetimepicker';
import CheckBox from '@react-native-community/checkbox';
import RadioGroup, {RadioButtonProps} from 'react-native-radio-buttons-group';
import { StackNavigationProp } from '@react-navigation/stack';
import Moment from 'moment';
import AsyncStorage from '@react-native-community/async-storage';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/core';

import { RootStackParamList } from '../RootStackPrams';
import ButtonSelectBar from '../../components/ButtonSelectBar';
import Picker from '../../components/CustomPicker';
import TextInput from '../../components/CustomTextInput';
import OperationModal from '../../components/OperationModal';
import TodayIcon from '../../assets/today.svg';
import ClockIcon from '../../assets/clock.svg';
import PrevIcon from '../../assets/nav_prev.svg';
import TrashIcon from '../../assets/trash.svg';
import * as I from '../../interfaces/interfaces';
import { TypesTransaction } from '../../enums/enums';
import { getCategories, getAccounts, postTransaction, putTransaction, deleteTransaction } from './transactions.api';

import { style } from '../../styles/styles';
import { transactionCreateStyle } from './create.styles';

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
    const navigation = useNavigation<homeScreenProp>();
    const route = useRoute<RouteProp<RootStackParamList, 'TransactionCreate'>>();
    const transactionID = route.params?.data.id ?? 0;
    const isEditing = route.params?.isEditing ?? false;
    const baseOperation = {} as I.Operacao;
    
    const [ typeSelected, setTypeSelected ] = useState(1);    
    const [ showDate, setShowDate ] = useState(false);
    const [ mode, setMode ] = useState('date');
    const [ categories, setCategories ] = useState<I.Categoria[]>([]);
    const [ accounts, setAccounts ] = useState<I.Conta[]>([]);
    const [ valueValue, setValueValue ] = useState(0);    
    const [ operation, setOperation ] = useState<I.Operacao>(baseOperation);
    const [ valueDescription, setValueDescription ] = useState("");
    const [ valueDate, setValueDate ] = useState("");
    const [ valueTime, setValueTime ] = useState("");
    const [ valueCategory, setValueCategory ] = useState(0);
    const [ valueAccount, setValueAccount ] = useState(0);
    const [ valueDestAccount, setValueDestAccount ] = useState(0);
    const [ valueNote, setValueNote ] = useState("");
    const [ valueConsolidated, setValueConsolidated ] = useState(false);
    const [ valueMultiply, setValeuMultiply ] = useState(false);
    const [ valueRadioRepeat, setValueRadioRepeat ] = useState<RadioButtonProps[]>(radioButtonsData);
    const [ valueTimes, setValueTimes ] = useState(1);
    const [ showModal, setShowModal ] = useState(false);

    const getLists = async () => {
        setCategories(await getCategories(navigation) ?? []);
        setAccounts(await getAccounts(navigation) ?? []);
    }

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined){
            setTypeSelected(data.operacao.tipo);
            setValueValue(data.valor);
            setOperation(data.operacao);
            setValueDescription(data.operacao.nome);
            setValueDate(Moment(data.dataCriacao).local().format('DD/MM/YYYY'));
            setValueTime(Moment(data.dataCriacao).local().format('HH:mm:ss'));
            setValueCategory(data.operacao.categoria.id);
            setValueAccount(data.conta.id);
            if (data.contaDestino)
                setValueDestAccount(data.contaDestino.id);
            setValueNote(data.observacao);
            setValueConsolidated(data.consolidado == 1 ? true : false);
        }
    };

    const clearOperation = () => {
        setOperation(baseOperation);
    };

    const setOperationDefault = () => {
        setValueDescription('Transferência');
        var categoryTranference = categories.find(x => x.nome == 'Transferência');
        if (categoryTranference !== undefined) {
            setValueCategory(categoryTranference.id);
        }
    };

    useEffect(() => {
        if (isEditing){
            loadDataSreen();
        }
    },[])

    useEffect(() => {
        setMode('date');
        if (!isEditing){
            setValueDate(Moment(new Date()).format('DD/MM/YYYY'))
            setValueTime(Moment(new Date()).format('HH:mm:ss'));
        }
        getLists();
        clearOperation();

        if (typeSelected == TypesTransaction.Transference) {
            setOperationDefault();
        }
    }, [typeSelected]);

    const validateRequiredFields = () => {

        if (valueValue === 0){
            Alert.alert("Atenção!", "O valor deve ser informado.");
            return false;
        }

        if (typeSelected !== TypesTransaction.Transference){
            if (valueDescription === ""){
                Alert.alert("Atenção!", "A descrição deve ser informada.");
                return false;
            } 
            
            if (valueCategory === 0){
                Alert.alert("Atenção!", "A categoria deve ser selecionada.");
                return false;
            }
        }      

        if (valueAccount === 0){
            Alert.alert("Atenção!", "A conta deve ser selecionada.");
            return false;
        }

        if (valueDestAccount == 0 && typeSelected === TypesTransaction.Transference){
            Alert.alert("Atenção!", "A conta de destino deve ser selecionada.");
            return false;
        }

        if (valueRadioRepeat[1].selected && valueTimes == 0){
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
        
        if (mode == 'date'){
            setValueDate(Moment(currentDate).local().format('DD/MM/YYYY'));
        } else { 
            setValueTime(Moment(currentDate).local().format('HH:mm:ss'));
        }
    };

    const getButtonStyle = (type: number) => {
        return [transactionCreateStyle.button, (typeSelected == type ?
            transactionCreateStyle.buttonSelected :
            transactionCreateStyle.buttonDefault)];
    };

    const getTextButtonStyle = (type: number) => {
        return [transactionCreateStyle.textButton, (typeSelected == type ?
            transactionCreateStyle.textButtonSelected :
            transactionCreateStyle.textButtonDefault)];
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
                            onPress: () => deleteTransaction(transactionID, navigation)
                        }
                    ],
                    { cancelable: false }
                    );
    };
    
    const handleOperationsClick = () => {
        setShowModal(true);
    };

    const handleItemOperationClick = (item: I.Operacao) => {
        setOperation(item);
        setValueDescription(item.nome);
        setValueCategory(item.categoria.id);
    };  

    const handleSaveClick = async () => {
        
        if (!validateRequiredFields()) return;

        let operationDTO = {} as I.Operacao;
        operationDTO.id = operation.id;
        operationDTO.nome = valueDescription;
        operationDTO.tipo = typeSelected;
        operationDTO.categoriaID = valueCategory;
        operationDTO.recorrente = (valueRadioRepeat[0].selected) ? 1 : 0;
        operationDTO.status = 1;
        
        let transactionDTO = {} as I.Transaction;
        transactionDTO.id = transactionID;
        transactionDTO.valor = valueValue;
        transactionDTO.observacao = valueNote;
        transactionDTO.consolidado = valueConsolidated ? 1 : 0;
        transactionDTO.totalParcelas = valueTimes;
        transactionDTO.dataCriacao = new Date(Moment(valueDate + " " + valueTime, 'DD/MM/YYYY HH:mm:ss').format());
        transactionDTO.contaID = valueAccount;
        transactionDTO.contaDestinoID = (valueDestAccount !== 0) ? valueDestAccount : undefined;
        transactionDTO.operacaoID = operation.id;
        transactionDTO.operacao = operationDTO;

        if (isEditing){
            putTransaction(transactionDTO, navigation);
            /*api.put(`Movimento/${transactionID}`, transactionDTO, {
                headers: { 'Authorization': 'Bearer ' + token ?? ""}
            }).then(response => {
                Alert.alert("Sucesso!", "Transação atualizada com sucesso.")
                navigation.goBack();
            }).catch((error) => {
                Alert.alert("Erro!", error.response.data);
            });*/
        } else {
            postTransaction(transactionDTO, navigation);
            /*api.post('Movimento', transactionDTO, {
                headers: { 'Authorization': 'Bearer ' + token ?? ""}
            }).then(response => {
                Alert.alert("Sucesso!", "Transação cadastrada com sucesso.")
                navigation.goBack();
            }).catch((error) => {
                Alert.alert("Erro!", error.response.data);
            });*/
        }
    };

    /*const deleteTransaction = async () => {
        const token = await AsyncStorage.getItem('token');
        
        api.delete(`Movimento/${transactionID}`,{
            headers: { 'Authorization': 'Bearer ' + token ?? ""}
        }).then(response => {
            Alert.alert("Sucesso!", "Transação excluída com sucesso.");
            navigation.goBack();
        }).catch((error) => {
            Alert.alert("Erro!", error.response.data);
        });
    }*/

    return(
        <SafeAreaView style={[style.container,style.containerCadastro]}>
            <ScrollView style={transactionCreateStyle.scroll} >
                <View style={transactionCreateStyle.viewHeaderCadastro}>
                    <TouchableOpacity 
                        style={transactionCreateStyle.buttonBack}
                        onPress={handleBackClick}>
                            <PrevIcon width="40" height="40" fill="#F1F1F1" />
                    </TouchableOpacity>
                    {isEditing && 
                    <TouchableOpacity 
                        style={transactionCreateStyle.buttonTrash}
                        onPress={handleTrashClick}>
                            <TrashIcon width="35" height="35" fill="#F1F1F1" />
                    </TouchableOpacity>}
                </View>
                <View style={transactionCreateStyle.viewBodyCadastro}>
                    <ButtonSelectBar>
                        <TouchableOpacity
                            style={getButtonStyle(TypesTransaction.Revenue)}
                            onPress={() => setTypeSelected(TypesTransaction.Revenue)}>
                            <Text style={getTextButtonStyle(TypesTransaction.Revenue)}>Receita</Text>
                        </TouchableOpacity>
                        <TouchableOpacity 
                            style={getButtonStyle(TypesTransaction.Transference)}
                            onPress={() => setTypeSelected(TypesTransaction.Transference)}>
                            <Text style={getTextButtonStyle(TypesTransaction.Transference)}>Transferência</Text>
                        </TouchableOpacity>
                        <TouchableOpacity 
                            style={getButtonStyle(TypesTransaction.Expense)}
                            onPress={() => setTypeSelected(TypesTransaction.Expense)}>
                            <Text style={getTextButtonStyle(TypesTransaction.Expense)}>Despesa</Text>
                        </TouchableOpacity>
                    </ButtonSelectBar>
                    <View style={transactionCreateStyle.areaFields} >
                        <View style={transactionCreateStyle.areaValue}>
                            <TouchableOpacity
                                style={transactionCreateStyle.buttonMinus}
                                onPress={() => {}}
                            >
                                <Text style={transactionCreateStyle.textButtonMinus}>-</Text>
                            </TouchableOpacity>
                            <CurrencyInput
                                style={getTextValueStyle()}
                                value={valueValue}
                                onChangeValue={(value: number) => setValueValue(value)}
                                prefix="R$"
                                delimiter="."
                                separator=","
                                precision={2}
                            />
                            <TouchableOpacity
                                style={transactionCreateStyle.buttonPlus}
                                onPress={() => {}}
                            >
                                <Text style={transactionCreateStyle.textButtonPlus}>+</Text>
                            </TouchableOpacity>
                        </View>
                        {!typeSelectedIsTransference() &&
                            <>
                                <Text 
                                    style={transactionCreateStyle.textListOperations}
                                    onPress={handleOperationsClick}>
                                    Operações
                                </Text>
                                <TextInput
                                    text={"Descrição"}
                                    isMoveText={false}
                                    value={valueDescription}
                                    setValue={setValueDescription}
                                />
                            </>
                        }
                        <View style={transactionCreateStyle.areaDateTime} >
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
                                value={new Date(Number(valueDate.split('/')[2]), Number(valueDate.split('/')[1]) -1, Number(valueDate.split('/')[0]), Number(valueTime.split(':')[0]), Number(valueTime.split(':')[1]), Number(valueTime.split(':')[2]))}
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
                            text={"Conta" + ((typeSelected == 0) ? " Origem" : "")}
                            value={valueAccount}
                            setValue={setValueAccount}
                            data={accounts}
                        />
                        {typeSelectedIsTransference() &&
                            <Picker
                                text={"Conta Destino"}
                                value={valueDestAccount}
                                setValue={setValueDestAccount}
                                data={accounts}
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
                                <View style={transactionCreateStyle.areaCheckbox}>
                                    <CheckBox
                                        value={valueConsolidated}
                                        onValueChange={setValueConsolidated}
                                        tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
                                    />
                                    <Text style={transactionCreateStyle.textCheckbox}>{(typeSelected == 1) ? "Recebido" : (typeSelected == 2) ? "Pago" : ""}</Text>
                                </View>
                                {!isEditing &&
                                    <>
                                        <View style={transactionCreateStyle.areaCheckbox}>
                                            <CheckBox
                                                value={valueMultiply}
                                                onValueChange={setValeuMultiply}
                                                tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
                                            />
                                            <Text style={transactionCreateStyle.textCheckbox}>Multiplicar</Text>
                                        </View>
                                    
                                        {valueMultiply &&
                                            <View style={transactionCreateStyle.areaRepeat}>
                                                <RadioGroup 
                                                    radioButtons={valueRadioRepeat} 
                                                    onPress={setValueRadioRepeat}
                                                    layout="row"
                                                    containerStyle={transactionCreateStyle.radioRepeat}
                                                />
                                                {valueRadioRepeat[1].selected &&
                                                    <View style={transactionCreateStyle.areaTimes}>
                                                        <TextInput
                                                            text={"Vezes"}
                                                            isMoveText={false}
                                                            value={valueTimes.toString()}
                                                            setValue={setValueTimes}
                                                            width={"100%"}
                                                            //messageText={(valueTimes != 0 ) ? (valueTimes.toString() + "x R$ " + (valueValue / valueTimes).toString()) : ""}                                    
                                                            />
                                                </View>}
                                            </View>
                                        }
                                    </>
                                }
                            </>
                        }
                    </View>
                    <View style={transactionCreateStyle.areaButtonSave}>
                        <TouchableOpacity
                            style={transactionCreateStyle.buttonSave}
                            onPress={handleSaveClick}
                        >
                            <Text style={transactionCreateStyle.textButtonSave}>Salvar</Text>
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