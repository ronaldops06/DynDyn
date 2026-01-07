import React, {useEffect, useState} from "react";
import {Alert, Text, View} from 'react-native';
import TextInput from "../../components/CustomTextInput";
import Picker from "../../components/CustomPicker";
import {constants} from "../../constants";
import CheckBox from "@react-native-community/checkbox";

import * as I from "../../interfaces/interfaces.tsx";
import {alterCategory, createCategory, excludeCategory} from "../../controller/category.controller.tsx";
import {validateLogin, validateSuccess} from "../../utils.ts";

import {useTheme} from '../../contexts/ThemeContext';
import {getStyleCadastro} from '../../styles/styles.cadastro';
import {getStyle} from "../../styles/styles.ts";
import {getCategoryCreateStyle} from "./create.styles";
import {PageRegister} from "../../components/Page";

const CategoryCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const categoryCreateStyle = getCategoryCreateStyle(theme);

    const categoryId = route.params?.data?.Id ?? 0;
    const categoryInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [loading, setLoading] = useState(false);
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
                        validateSuccess(response, navigation, 'CategoryHome');
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

        if (type === 0 || type === null || type === undefined) {
            Alert.alert("Atenção!", "O tipo deve ser selecionado.");
            return false;
        }

        return true;
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);

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

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'CategoryHome');
    };

    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"category_register"}
            isEditing={isEditing}
            isLoading={loading}>
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
        </PageRegister>
    );
}

export default CategoryCreate;