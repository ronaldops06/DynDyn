import React, {useState} from "react";
import {Text, TouchableOpacity, View, TextInput} from "react-native";
import ExpandIcon from "../../assets/expand.svg";
import BottomModal from "../BottomModal";
import CustomScroll from "../CustomScroll";
import SelectItem, {SelectItemRow} from "./SelectItem";
import TextItem from "../CustomTextInput";
import AddIcon from "../../assets/plus.svg";

import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getSelectStyle} from "./styles";
import {getStyleCadastro} from "../../styles/styles.cadastro.ts";
import * as I from "../../interfaces/interfaces.tsx";

interface SelectProps {
    label: string;
    value: SelectItemRow;
    setValue: any;
    required?: boolean | undefined;
    valueDefault?: number | undefined;
    width?: string | undefined;
    parentScreen?: string | undefined;
    registerScreen?: string | undefined;
    sourceScreen?: string | undefined;
    navigation?: any | undefined;
    reference?: string | undefined;
    data: SelectItemRow[];
}

const Select = (props: SelectProps) => {
    const {theme} = useTheme();
    const selectStyle = getSelectStyle(theme);
    const styleCadastro = getStyleCadastro(theme);

    const [show, setShow] = useState(false);
    const [search, setSearch] = useState("");

    const {
        required = false,
        valueDefault = 0,
        width = '100%',
    } = props;

    const handleItemClick = (value) => {
        props.setValue(value.Id);
        setShow(false);
    }

    const goTo = () => {
        let param = {isEditing: false, data: null, sourceScreen: props.sourceScreen, reference: props.reference };
        props.navigation.navigate(props.parentScreen, {
            screen: props.registerScreen,
            params: param
        });
        setShow(false);
    };

    return (
        <>
            <View style={[selectStyle.container, {width: width}]}>
                <Text style={selectStyle.label}>{props.label}</Text>
                <TouchableOpacity
                    style={selectStyle.containerInput}
                    onPress={() => setShow(true)}>
                    <TextInput
                        autoCapitalize={"none"}
                        style={selectStyle.input}
                        value={props.data?.find(x => x.Id === props.value)?.Name}
                        onChangeText={(text: string) => props.setValue(text)}
                        editable={false}
                        blurOnSubmit
                    />
                    <ExpandIcon width="25" height="25" fill={theme.colors.quaternaryIcon}/>
                </TouchableOpacity>
            </View>
            <BottomModal show={show} setShow={setShow}>
                <TextItem
                    text="Search"
                    value={search}
                    setValue={setSearch}
                    autoFocus={true}
                />
                <CustomScroll
                    styles={{}}
                    data={props.data.filter(x => x.Name.toUpperCase().includes(search.toUpperCase()))}
                    loading={false}
                    totalPages={1}
                    pageNumber={1}
                    handlePageNumber={() => {
                    }}
                    handleScrolling={() => {
                    }}
                    renderItem={({item}) => (
                        <SelectItem
                            data={item}
                            onPress={() => handleItemClick(item)}
                        />
                    )}
                />
                {props.registerScreen &&
                    <TouchableOpacity
                        style={selectStyle.buttonAdd}
                        onPress={() => goTo()}>
                        <AddIcon width="30" height="30" fill={theme.colors.primaryIcon}/>
                        <Text style={styleCadastro.textButtonSave}>Adicionar</Text>
                    </TouchableOpacity>
                }
            </BottomModal>
        </>
    );
}

export default Select;