import React, {useCallback, useEffect, useState} from "react";
import {useFocusEffect} from "@react-navigation/native";
import {Alert, ScrollView, Text, View} from 'react-native';
import {Chip} from 'react-native-paper';
import {alterOperation, createOperation, excludeOperation} from "../../../controller/operation.controller.tsx";
import {constants} from "../../../constants";
import * as I from "../../../interfaces/interfaces.tsx";
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import {TypesCategory} from "../../../enums/enums.tsx";
import TextInput from "../../../components/CustomTextInput";
import CheckBox from "@react-native-community/checkbox";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../../components/ButtonSelectBar";
import {getCurrentStack, validateLogin, validateSuccess} from "../../../utils.ts";

import {useTheme} from '../../../contexts/ThemeContext';
import {getStyle} from "../../../styles/styles.ts";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import {getOperationCreateStyle} from "./styles";
import {PageRegister} from "../../../components/Page";
import OperationRoleModal from "../OperationRoleModal";
import {CustomAlert} from "../../../components/CustomAlert";
import Select from "../../../components/Select";
import AuxiliaryButton from "../../../components/AuxiliaryButton";


const OperationCreate = ({navigation, route}: {navigation: any, route: any}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const operationCreateStyle = getOperationCreateStyle(theme);

    const operationId = route.params?.data?.Id ?? 0;
    const operationInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [stack, setStack] = useState<string>("")
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState<string>("");
    const [type, setType] = useState<number>(constants.operationType.revenue.Id);
    const [category, setCategory] = useState(0);
    const [isRecurrent, setIsRecurrent] = useState<boolean>(false);
    const [status, setStatus] = useState<boolean>(true);
    const [operationRoles, setOperationRoles] = useState<I.OperationRole[]>([]);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [showModalRole, setShowModalRole] = useState<boolean>(false);

    useFocusEffect(
        useCallback(() => {
            if (route.params?.referenceId !== undefined) {
                let reference = route.params?.reference;
                
                if (reference === constants.operations.category)
                {
                    getCategories();
                    
                    setCategory(route.params?.referenceId);
                }
            }
        }, [route.params?.actionNavigation])
    );

    useEffect(() => {
        getLists();
        if (isEditing) {
            loadDataSreen();
        }
        
        const tab = getCurrentStack(navigation);
        setStack(tab);
    }, [])
    
    const getLists = async () => {
        await getCategories();
    }
    
    const getCategories = async () => {
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Operation, null, true);
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
            setIsRecurrent(data.Recurrent);
            setOperationRoles(data.OperationRoles);
        }
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        await CustomAlert("Atenção!",
            "Esta operação será excluída. Deseja continuar?",
            async () => {
                let response = await excludeOperation(operationId, operationInternalId);
                validateLogin(response, navigation);
                validateSuccess(response, navigation, 'OperationHome');
            }
        );
    };

    const getButtonsSelectedBar = (): ButtonsSelectedProps[] => {
        let buttonsSelectedBar: ButtonsSelectedProps[] = [];

        Object.values(constants.operationType).map(type => {
            if (type.Id !== constants.operationType.transfer.Id)
                buttonsSelectedBar.push({text: type.Name, value: type.Id});
        });

        return buttonsSelectedBar;
    }

    const addOperationRole = (item: I.OperationRole) => {
        let exists = operationRoles.findIndex(x => x.InternalId === item.InternalId);

        if (exists < 0) {
            let operationsRolesAux = operationRoles;
            operationsRolesAux.push(item);
            setOperationRoles(operationsRolesAux);
        }
    }

    const removeOperationRole = (item: I.OperationRole) => {
        setOperationRoles(prev =>
            prev.filter(x => x.InternalId !== item.InternalId)
        );
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
        operationDTO.Salary = false;
        operationDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;
        operationDTO.OperationRoles = operationRoles;

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
        
        if (operationRoles.length === 0){
            Alert.alert("Atenção!", "É necessário informar ao menos um papel de operação.");
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
            <View style={operationCreateStyle.areaButtonType}>
                <ButtonSelectBar
                    buttons={getButtonsSelectedBar()}
                    valueSelected={type}
                    handleValueSelected={setType}
                    disabled={false}
                />
            </View>
            <View style={operationCreateStyle.areaFields}>
                <TextInput
                    text={"Nome"}
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
                <Select
                    label={"Categoria"}
                    value={category}
                    setValue={setCategory}
                    data={categories}
                    parentScreen={stack}
                    registerScreen={"CategoryCreate"}
                    navigation={navigation}
                    sourceScreen={route.name}
                    reference={constants.operations.category}
                />
                <View style={styleCadastro.areaGroupCheckbox}>
                    <View style={operationCreateStyle.areaCard}>
                        <CheckBox
                            value={isRecurrent}
                            onValueChange={setIsRecurrent}
                            tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                        />
                        <Text
                            style={styleCadastro.textCheckbox}>Recorrente</Text>
                    </View>
                </View>
                <View style={operationCreateStyle.areaCard}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                    />
                    <Text
                        style={styleCadastro.textCheckbox}>Ativo</Text>
                </View>
                <AuxiliaryButton
                    text="Papéis de Operações"
                    onPress={() => setShowModalRole(true)}
                    icon="rule"
                    iconColor={theme.colors.primaryIcon}
                />
                <ScrollView style={operationCreateStyle.scrollRoles} nestedScrollEnabled>
                    <View style={operationCreateStyle.areaRoles}>
                        {operationRoles.map(item => (
                            <Chip
                                key={item.InternalId}
                                onClose={() => removeOperationRole(item)}
                                mode="flat"
                                style={{marginVertical: 4, marginLeft: 8}}
                                closeIcon="close"
                            >
                                {item.Name}
                            </Chip>
                        ))
                        }
                    </View>
                </ScrollView>
            </View>
            <OperationRoleModal
                show={showModalRole}
                setShow={setShowModalRole}
                navigation={navigation}
                addOperationRole={addOperationRole}
            />
        </PageRegister>
    );
}

export default OperationCreate;