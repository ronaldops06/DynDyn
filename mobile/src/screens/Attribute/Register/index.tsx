import React, {useEffect, useState} from "react";
import {Alert, ScrollView, Text, View} from 'react-native';
import TextInput from "../../../components/CustomTextInput";
import Select from "../../../components/Select";

import * as I from "../../../interfaces/interfaces.tsx";
import {alterAttribute, createAttribute, excludeAttribute} from "../../../controller/attribute.controller.tsx";
import {validateLogin, validateSuccess} from "../../../utils.ts";

import {constants} from "../../../constants";
import {useTheme} from '../../../contexts/ThemeContext';
import {getStyleCadastro} from '../../../styles/styles.cadastro';
import {getAttributeCreateStyle} from "./styles";
import {PageRegister} from "../../../components/Page";
import CheckBox from "../../../components/CheckBox";
import TextArea from "../../../components/TextArea";
import AttributeOptionModal from "./AttributeOptionModal";
import AttributeOptionItem from "./AttributeOptionItem";
import AuxiliaryButton from "../../../components/AuxiliaryButton";

const AttributeCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const attributeCreateStyle = getAttributeCreateStyle(theme);

    const attributeId = route.params?.data?.Id ?? 0;
    const attributeInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;
    const sourceScreen = route.params?.sourceScreen ?? "Attribute";
    const reference = route.params?.reference;

    const [loading, setLoading] = useState(false);
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [dataType, setDataType] = useState<number>();
    const [status, setStatus] = useState<boolean>(true);
    const [options, setOptions] = useState<I.AttributeOption[]>([]);
    const [showOptionModal, setShowOptionModal] = useState(false);
    const [editingOption, setEditingOption] = useState<I.AttributeOption | null>(null);

    const isListOptions = dataType === constants.attributeDataType.listOptions.Id;

    useEffect(() => {
        if (isEditing) {
            loadDataSreen();
        }
    }, [])

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
            setDescription(data.Description ?? "");
            setDataType(data.DataType);
            setStatus(data.Status === constants.status.active.Id);
            setOptions(data.Options ?? []);
        }
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Este atributo será excluído. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeAttribute(attributeId, attributeInternalId);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, sourceScreen);
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

        if (dataType === 0 || dataType === null || dataType === undefined) {
            Alert.alert("Atenção!", "O tipo de dado deve ser selecionado.");
            return false;
        }

        if (isListOptions && options.length === 0) {
            Alert.alert("Atenção!", "Adicione ao menos uma opção para este atributo.");
            return false;
        }

        return true;
    }

    const serializeOptions = (items: I.AttributeOption[]) => {
        return items.map(({tempId, ...item}) => item);
    };

    const isSameOption = (left: I.AttributeOption, right: I.AttributeOption) => {
        if (left.tempId && right.tempId) {
            return left.tempId === right.tempId;
        }

        if (left.Id !== undefined && right.Id !== undefined) {
            return left.Id === right.Id;
        }

        return false;
    };

    const handleAddOption = () => {
        setEditingOption(null);
        setShowOptionModal(true);
    };

    const handleEditOption = (option: I.AttributeOption) => {
        setEditingOption(option);
        setShowOptionModal(true);
    };

    const handleDeleteOption = (option: I.AttributeOption) => {
        Alert.alert(
            "Atenção!",
            "Esta opção será excluída. Deseja continuar?",
            [
                {text: "Não", style: "cancel"},
                {
                    text: "Sim",
                    onPress: () => {
                        setOptions(prev =>
                            prev.filter(item => !isSameOption(item, option))
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    };

    const handleSaveOption = (option: I.AttributeOption) => {
        setOptions(prev => {
            const next = prev.some(item => isSameOption(item, option))
                ? prev.map(item =>
                    isSameOption(item, option)
                        ? {...item, ...option}
                        : item
                )
                : [...prev, option];

            return next;
        });
    };

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);

        let attributeDTO = {} as I.Attribute;
        attributeDTO.Id = attributeId;
        attributeDTO.InternalId = attributeInternalId;
        attributeDTO.Name = name;
        attributeDTO.Description = description;
        attributeDTO.DataType = dataType ?? 1;
        attributeDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;
        attributeDTO.Options = isListOptions ? serializeOptions(options) : [];

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterAttribute(attributeDTO);
        else
            response = await createAttribute(attributeDTO);

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, sourceScreen, reference);
    };

    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"attribute_register"}
            isEditing={isEditing}
            isLoading={loading}>
            <View style={styleCadastro.areaFields}>
                <TextInput
                    text={"Nome"}
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
                <TextArea
                    label="Descrição"
                    value={description}
                    setValue={setDescription}
                    placeholder="Digite a descrição (optional)"
                />
                <Select
                    label={"Tipo de dado"}
                    value={dataType}
                    setValue={setDataType}
                    data={Object.values(constants.attributeDataType)}
                />
                {isListOptions ? (
                    <View style={attributeCreateStyle.optionsSection}>
                        <View style={attributeCreateStyle.optionsHeader}>
                            <View style={attributeCreateStyle.optionsHeaderText}>
                                <Text style={attributeCreateStyle.optionsTitle}>Opções</Text>
                                <Text style={attributeCreateStyle.optionsDescription}>
                                    Cadastre as opções disponíveis para este atributo.
                                </Text>
                            </View>
                        </View>
                        <View style={attributeCreateStyle.areaButtonAdd}>
                            <AuxiliaryButton
                                text="Adicionar opção"
                                onPress={handleAddOption}
                                icon="plus"
                                iconColor={theme.colors.quaternaryIcon}
                                type="secondary"
                            />
                        </View>
                        {options.length > 0 ? (
                            <ScrollView style={attributeCreateStyle.scrollOptions} nestedScrollEnabled>
                                {options.map((item, index) => (
                                    <AttributeOptionItem
                                        key={`${index}-${item.Id}`}
                                        data={item}
                                        onEdit={handleEditOption}
                                        onDelete={handleDeleteOption}
                                    />
                                ))}
                            </ScrollView>
                        ) : (
                            <View style={attributeCreateStyle.emptyState}>
                                <Text style={attributeCreateStyle.emptyStateText}>
                                    Nenhuma opção cadastrada.
                                </Text>
                            </View>
                        )}

                    </View>
                ) : null}
                <View style={attributeCreateStyle.areaCard}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        label="Ativo"
                        description="Campos inativos não serão exibidos nas telas e não poderão ser utilizados."
                    />
                </View>
            </View>
            <AttributeOptionModal
                show={showOptionModal}
                setShow={setShowOptionModal}
                option={editingOption}
                existingOptions={options}
                onSave={handleSaveOption}
            />
        </PageRegister>
    );
}

export default AttributeCreate;
