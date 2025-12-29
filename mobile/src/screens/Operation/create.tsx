import React, {useEffect, useState} from "react";
import {Alert, Text, View} from 'react-native';
import {alterOperation, createOperation, excludeOperation} from "../../controller/operation.controller.tsx";
import {constants} from "../../constants";
import * as I from "../../interfaces/interfaces.tsx";
import {loadAllCategory} from "../../controller/category.controller.tsx";
import {TypesCategory} from "../../enums/enums.tsx";
import TextInput from "../../components/CustomTextInput";
import Picker from "../../components/CustomPicker";
import CheckBox from "@react-native-community/checkbox";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../components/ButtonSelectBar";
import {validateLogin, validateSuccess} from "../../utils.ts";

import {useTheme} from '../../contexts/ThemeContext';
import {getStyle} from "../../styles/styles.ts";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import {getOperationCreateStyle} from "./create.styles";
import {PageRegister} from "../../components/Page";

const OperationCreate = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const operationCreateStyle = getOperationCreateStyle(theme);
    
    const operationId = route.params?.data?.Id ?? 0;
    const operationInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [loading, setLoading] = useState(false);
    const [name, setName] = useState<string>("");
    const [type, setType] = useState<number>(constants.operationType.revenue.Id);
    const [category, setCategory] = useState(0);
    const [isRecurrent, setIsRecurrent] = useState<boolean>(false);
    const [isSalary, setIsSalary] = useState<boolean>(false);
    const [status, setStatus] = useState<boolean>(true);
    const [categories, setCategories] = useState<I.Category[]>([]);

    useEffect(() => {
        getLists();
        if (isEditing) {
            loadDataSreen();
        }
    }, [])

    const getLists = async () => {
        let responseCategories = await loadAllCategory(TypesCategory.Operation, null, true);
        validateLogin(responseCategories, navigation);
        
        setCategories(responseCategories?.data ?? []);
    }

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
            setType(data.Type);
            setStatus(data.Status === constants.status.active.Id);
            setCategory(data.Category.Id);
            setIsSalary(data.Salary);
            setIsRecurrent(data.Recurrent);
        }
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Esta operação será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeOperation(operationId, operationInternalId);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, 'OperationHome');
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

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);
        
        let operationDTO = {} as I.Operation;
        operationDTO.Id = operationId;
        operationDTO.InternalId = operationInternalId;
        operationDTO.Name = name;
        operationDTO.Type = type;
        operationDTO.Category = categories.find(x => x.Id === category) ?? {} as I.Category;
        operationDTO.Recurrent = isRecurrent;
        operationDTO.Salary = isSalary ?? false;
        operationDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterOperation(operationDTO);
        else
            response = await createOperation(operationDTO);

        setLoading(false);
        
        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'OperationHome');
    };

    const validateRequiredFields = () => {

        if (name === "" || name === null || name === undefined) {
            Alert.alert("Atenção!", "O nome deve ser informado.");
            return false;
        }

        if (type === 0 || type === null || type === undefined) {
            Alert.alert("Atenção!", "O tipo deve ser selecionado.");
            return false;
        }

        if (category === 0) {
            Alert.alert("Atenção!", "A categoria deve ser selecionada.");
            return false;
        }

        return true;
    }

    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"operation_register"}
            isEditing={isEditing}
            isLoading={loading}>
            <ButtonSelectBar
                buttons={getButtonsSelectedBar()}
                valueSelected={type}
                handleValueSelected={setType}
                disabled={false}
            />
            <View style={operationCreateStyle.areaFields}>
                <TextInput
                    text={"Nome"}
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
                <Picker
                    text={"Categoria"}
                    value={category}
                    setValue={setCategory}
                    data={categories}
                />
                <View style={styleCadastro.areaGroupCheckbox}>
                    <View style={operationCreateStyle.areaCheckbox}>
                        <CheckBox
                            value={isRecurrent}
                            onValueChange={setIsRecurrent}
                            tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                        />
                        <Text
                            style={styleCadastro.textCheckbox}>Recorrente</Text>
                    </View>
                    <View style={operationCreateStyle.areaCheckbox}>
                        <CheckBox
                            value={isSalary}
                            onValueChange={setIsSalary}
                            tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                        />
                        <Text
                            style={styleCadastro.textCheckbox}>Salário</Text>
                    </View>
                </View>
                <View style={operationCreateStyle.areaCheckbox}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                    />
                    <Text
                        style={styleCadastro.textCheckbox}>Ativo</Text>
                </View>
            </View>
        </PageRegister>
    );
}

export default OperationCreate;