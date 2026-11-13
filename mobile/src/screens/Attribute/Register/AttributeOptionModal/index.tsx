import React, {useEffect, useState} from "react";
import {Alert, View} from "react-native";
import BottomModal from "../../../../components/BottomModal";
import TextInput from "../../../../components/CustomTextInput";
import Button from "../../../../components/Button";
import CheckBox from "../../../../components/CheckBox";

import * as I from "../../../../interfaces/interfaces";
import {constants} from "../../../../constants";

import {useTheme} from "../../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../../styles/styles.cadastro";
import {getAttributeOptionModalStyle} from "./styles";

const MAX_LENGTH = 100;

interface AttributeOptionModalProps {
    show: boolean;
    setShow: (value: boolean) => void;
    option: I.AttributeOption | null;
    existingOptions: I.AttributeOption[];
    onSave: (option: I.AttributeOption) => void;
}

const AttributeOptionModal = ({
                                  show,
                                  setShow,
                                  option,
                                  existingOptions,
                                  onSave,
                              }: AttributeOptionModalProps) => {
    const {theme} = useTheme();
    const attributeOptionModalStyle = getAttributeOptionModalStyle(theme);
    const styleCadastro = getStyleCadastro(theme);

    const [label, setLabel] = useState("");
    const [isDefault, setIsDefault] = useState<boolean>(false);
    const [status, setStatus] = useState<boolean>(true);

    const isEditing = option !== null;

    useEffect(() => {
        if (show && isEditing) {
            setLabel(option?.Label ?? "");
            setIsDefault(option?.IsDefault === 1 ?? false);
            setStatus(option?.Status === constants.status.active.Id ?? true);
        } else if (show) {
            setLabel("");
            setIsDefault(false);
            setStatus(true);
        }
    }, [show, option]);

    const handleClose = () => {
        setShow(false);
    };

    const validateFields = () => {
        if (!label.trim()) {
            Alert.alert("Atenção!", "O label deve ser informado.");
            return false;
        }

        const duplicateValue = existingOptions.some(
            (item) =>
                item.Label.trim().toLowerCase() === label.trim().toLowerCase() &&
                item.tempId !== option?.tempId &&
                item.Id !== option?.Id
        );

        if (duplicateValue) {
            Alert.alert("Atenção!", "Já existe uma opção com este label.");
            return false;
        }

        return true;
    };

    const handleSave = () => {
        if (!validateFields()) return;

        onSave({
            ...option,
            Id: isEditing ? option.Id : 0,
            InternalId: isEditing ? option.InternalId : 0,
            Label: label.trim(),
            IsDefault: isDefault ? 1 : 0,
            Status: status ? 1 : 0,
            tempId: option?.tempId ?? `temp-${Date.now()}`,
        });
        setShow(false);
    };

    return (
        <BottomModal show={show} setShow={setShow}>
            <View style={styleCadastro.areaFields}>
                <View style={attributeOptionModalStyle.field}>
                    <TextInput
                        text="Label"
                        isMoveText={false}
                        value={label}
                        setValue={(text: string) => setLabel(text.slice(0, MAX_LENGTH))}
                    />
                </View>
                <View style={attributeOptionModalStyle.field}>
                    <CheckBox
                        value={isDefault}
                        onValueChange={setIsDefault}
                        label="Default"
                    />
                </View>
                <View style={attributeOptionModalStyle.field}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        label="Ativo"
                        description="Opções inativas não serão exibidas nas telas e não poderão ser utilizadas."
                    />
                </View>
            </View>
            <View style={styleCadastro.areaButtonSave}>
                <Button label="Adicionar" onPress={handleSave}/>
            </View>
        </BottomModal>
    );
};

export default AttributeOptionModal;
