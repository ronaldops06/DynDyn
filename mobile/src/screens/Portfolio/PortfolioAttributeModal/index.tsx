import React, {useEffect, useState} from "react";
import {ScrollView, View} from "react-native";
import BottomModal from "../../../components/BottomModal";
import Select from "../../../components/Select";
import CheckBox from "../../../components/CheckBox";
import RadioGroupCustom from "../../../components/RarioGroupCustom";
import Button from "../../../components/Button";
import DateTimeInput from "../../../components/DateTimeInput";
import TextInput from "../../../components/CustomTextInput";
import {SelectItemRow} from "../../../components/Select/SelectItem";

import {Attribute, PortfolioAttribute, SymbolString} from "../../../interfaces/interfaces";
import {constants} from "../../../constants";
import {toLocalDate} from "../../../utils";

import {loadAllAttributeInternal} from "../../../controller/attribute.controller";

import {useTheme} from "../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../styles/styles.cadastro";
import {getPortfolioAttributeStyle} from "./styles";

interface PortfolioAttributeModalProps {
    show: boolean;
    setShow: (value: boolean) => void;
    addPortfolioAttribute: (item: PortfolioAttribute) => void;
    portfolioAttribute?: PortfolioAttribute | undefined;
    isEditing: boolean;
};

const PortfolioAttributeModal = (props: PortfolioAttributeModalProps) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const portfolioAttributeStyle = getPortfolioAttributeStyle(theme);

    const [loading, setLoading] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [attributes, setAttibutes] = useState<Attribute[]>([] as Attribute[]);
    const [attribute, setAttribute] = useState<number>(0);
    const [status, setStatus] = useState<boolean>(true);
    const [actionType, setActionType] = useState<string>(constants.portfolioActionType.informed.Id.toString());
    const [valueText, setValueText] = useState<string>("");
    const [valueNumber, setValueNumber] = useState<number>(0);
    const [valueBoolean, setValueBoolean] = useState<boolean>(false);
    const [valueDate, setValueDate] = useState<string>();
    const [valueAttributeOption, setValueAttributeOption] = useState<number>(0);

    useEffect(() => {
        clearEditing();
        setPageNumber(1);
        loadAttributes();
        loadDataScreen();
    }, [props.show]);

    useEffect(() => {
        if (attributes.length === 0) {
            setPageNumber(1);
            loadAttributes();
        }
    }, [attributes]);

    const loadAttributes = async () => {
        setLoading(true);

        let response = await loadAllAttributeInternal(pageNumber, 1);
        setAttibutes(response?.data ?? []);
        setLoading(false);
    };

    const loadDataScreen = () => {

        if (props.portfolioAttribute !== undefined && props.portfolioAttribute !== null) {
            setAttribute(props.portfolioAttribute.Attribute.Id);
            setStatus(props.portfolioAttribute.Status === 1);
            setActionType(props.portfolioAttribute.ActionType.toString());
            setValueText(props.portfolioAttribute.ValueText ?? "");
            setValueNumber(props.portfolioAttribute.ValueNumber ?? 0);
            setValueDate(props.portfolioAttribute.ValueDate?.toString());
            setValueBoolean(props.portfolioAttribute.ValueBoolean === 1);
            setValueAttributeOption(props.portfolioAttribute.AttributeOption?.Id ?? 0);
        }
    }

    const getValueAttribute = (attributeParm: Attribute) => {

        return (
            <>
                {(() => {
                    switch (attributeParm.DataType) {
                        case constants.attributeDataType.text.Id:
                            return <TextInput
                                text="Valor Atributo"
                                value={valueText}
                                setValue={setValueText}
                                editable={actionType === constants.portfolioActionType.informed.Id.toString()}
                            />
                        case constants.attributeDataType.number.Id:
                            return <TextInput
                                text="Valor Atributo"
                                value={valueNumber?.toString()}
                                setValue={(value) => setValueNumber(Number(value))}
                                keyboardType="numeric"
                                editable={actionType === constants.portfolioActionType.informed.Id.toString()}
                            />
                        case constants.attributeDataType.boolean.Id:
                            return <CheckBox
                                label={attributeParm.Name}
                                value={valueBoolean}
                                onValueChange={setValueBoolean}
                            />
                        case constants.attributeDataType.date.Id:
                            return <DateTimeInput
                                dateLabel="Valor Atributo"
                                dateValue={valueDate ?? ""}
                                setDateValue={setValueDate}
                            />
                        case constants.attributeDataType.listOptions.Id:

                            const attributeOptions: SelectItemRow[] = attributes?.find(x => x.Id === attribute)?.Options?.map(item => {
                                return {
                                    Id: item.Id,
                                    Name: item.Label
                                }
                            }) ?? [] as SelectItemRow[];

                            return <Select
                                label="Valor Atributo"
                                value={valueAttributeOption}
                                setValue={setValueAttributeOption}
                                data={attributeOptions}
                            />
                    }
                })()}
            </>
        );
    }
    
    const getAttributes = (): Attribute[] => {
        let attributesFiltereded = [] as Attribute[];
            
        if (actionType === constants.portfolioActionType.calculated.Id.toString()) {
            attributesFiltereded = attributes.filter(x => x.DataType === constants.attributeDataType.number.Id);
        } else {
            attributesFiltereded = attributes;
        }
        
        return attributesFiltereded;
    }
    
    const handleActionType = (value: string) => {
        if (value === constants.portfolioActionType.calculated.Id.toString()) {
            setAttribute(0);
            setValueText("");
            setValueNumber(0);
            setValueDate("");
            setValueBoolean(false);
            setValueAttributeOption(0);
        }

        setActionType(value);
    }

    const handleApply = () => {
        const portfolioAttributeResponse = props.portfolioAttribute ?? {} as PortfolioAttribute;
        portfolioAttributeResponse.Attribute = attributes.find(x => x.Id === attribute) ?? {} as Attribute;
        portfolioAttributeResponse.Status = status ? 1 : 0;
        portfolioAttributeResponse.ActionType = Number(actionType);
        portfolioAttributeResponse.ValueText = valueText;
        portfolioAttributeResponse.ValueNumber = valueNumber;
        portfolioAttributeResponse.ValueDate = toLocalDate(valueDate ?? "");
        portfolioAttributeResponse.ValueBoolean = valueBoolean ? 1 : 0;
        portfolioAttributeResponse.AttributeOption = attributes?.find(x => x.Id === attribute)?.Options?.find(x => x.Id === valueAttributeOption);

        props.addPortfolioAttribute(portfolioAttributeResponse);
        props.setShow(false);
    }

    const clearEditing = () => {
        setAttribute(0);
        setStatus(true);
        setActionType(constants.portfolioActionType.informed.Id.toString());
        setValueText("");
        setValueNumber(0);
        setValueDate("");
        setValueBoolean(false);
        setValueAttributeOption(0);
    }

    const radioButtonsData: SymbolString[] = [
        {
            Id: constants.portfolioActionType.informed.Id.toString(),
            Name: constants.portfolioActionType.informed.Name
        },
        {
            Id: constants.portfolioActionType.calculated.Id.toString(),
            Name: constants.portfolioActionType.calculated.Name
        }];

    return (
        <BottomModal show={props.show} setShow={props.setShow}>
            <ScrollView style={portfolioAttributeStyle.areaFields}>
                <View style={styleCadastro.areaField}>
                    <RadioGroupCustom
                        label="Tipo de Ação"
                        data={radioButtonsData}
                        onPress={(value) => handleActionType(value)}
                        value={actionType}
                        isEditing={props.isEditing}
                    />
                </View>
                <View style={styleCadastro.areaField}>
                    <Select
                        label={"Atributo"}
                        value={attribute}
                        setValue={setAttribute}
                        data={getAttributes()}
                        isEditing={props.isEditing}
                    />
                </View>
                <View style={styleCadastro.areaField}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        label="Ativo"
                    />
                </View>
                {attribute > 0 &&
                   <View style={portfolioAttributeStyle.areaValue}>
                        {getValueAttribute(attributes?.find(x => x.Id === attribute) ?? {} as Attribute)}
                    </View>
                }
            </ScrollView>
            <View style={portfolioAttributeStyle.areaButtonApply}>
                <Button
                    label={"Concluir"}
                    onPress={handleApply}
                />
            </View>
        </BottomModal>
    );
}

export default PortfolioAttributeModal;