import React, {useEffect, useState} from "react";
import {Alert, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native';
import TextInput from "../../components/CustomTextInput";

import PrevIcon from "../../assets/nav_prev.svg";
import TrashIcon from "../../assets/trash.svg";
import Picker from "../../components/CustomPicker";
import {constants} from "../../constants";
import CheckBox from "@react-native-community/checkbox";

import * as I from "../../interfaces/interfaces.tsx";
import {alterCategory, createCategory, excludeCategory} from "../../controller/category.controller.tsx";
import {validateLogin, validateSuccess} from "../../utils.ts";

import { useTheme } from '../../contexts/ThemeContext';
import {getStyleCadastro} from '../../styles/styles.cadastro';
import {getStyle} from "../../styles/styles.ts";
import {getCategoryCreateStyle} from "./create.styles";

const CategoryCreate = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const categoryCreateStyle = getCategoryCreateStyle(theme);

    const categoryId = route.params?.data?.Id ?? 0;
    const categoryInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [name, setName] = useState<string>("");
    const [type, setType] = useState<number>();
    const [status, setStatus] = useState<boolean>(true);

    useEffect(() => {
        if (isEditing) {
            loadDataSreen();
        }
    }, [])
    
    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
            setType(data.Type);
            setStatus(data.Status === constants.status.active.Id);
        }
    };
    
    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Esta categoria será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim", 
                    onPress: async () => {
                        let response = await excludeCategory(categoryId, categoryInternalId);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, route);
                    }
                }
            ],
            {cancelable: false}
        );
    };

    const validateRequiredFields = () => {

        if (name === "" || name === null || name === undefined) {
            Alert.alert("Atenção!", "O nome deve ser informado.");
            return false;
        }
        
        if (type === 0 || type === null || type === undefined){
            Alert.alert("Atenção!", "O tipo deve ser selecionado.");
            return false;
        }
        
        return true;
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        let categoryDTO = {} as I.Category;
        categoryDTO.Id = categoryId;
        categoryDTO.InternalId = categoryInternalId;
        categoryDTO.Name = name;
        categoryDTO.Type = type ?? 1;
        categoryDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterCategory(categoryDTO);
        else
            response = await createCategory(categoryDTO);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'Category');
    };

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                    {isEditing &&
                        <TouchableOpacity
                            style={styleCadastro.buttonTrash}
                            onPress={handleTrashClick}>
                            <TrashIcon width="35" height="35" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>}
                </View>
                <View style={styleCadastro.viewBodyCadastro}>
                    <View style={categoryCreateStyle.areaFields}>
                        <TextInput
                            text={"Nome"}
                            isMoveText={false}
                            value={name}
                            setValue={setName}
                        />
                        <Picker
                            text={"Tipo"}
                            value={type}
                            setValue={setType}
                            data={Object.values(constants.categoryType)}
                        />
                        <View style={categoryCreateStyle.areaCheckbox}>
                            <CheckBox
                                value={status}
                                onValueChange={setStatus}
                                tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                            />
                            <Text
                                style={styleCadastro.textCheckbox}>Ativo</Text>
                        </View>
                    </View>
                    <View style={categoryCreateStyle.areaButtonSave}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleSaveClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Salvar</Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default CategoryCreate;