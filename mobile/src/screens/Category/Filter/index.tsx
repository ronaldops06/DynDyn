import React, {useEffect, useState} from "react";
import {ScrollView, View} from "react-native";
import Picker from "../../../components/CustomPicker";
import TextItem from "../../../components/CustomTextInput";
import Button from "../../../components/Button";

import * as I from "../../../interfaces/interfaces";
import {Situation} from "../../../enums/enums";

import {useTheme} from "../../../contexts/ThemeContext";
import {getFilterStyles} from "./styles";
import {getStyleCadastro} from "../../../styles/styles.cadastro";

interface FiltersProps {
    filter: I.CategoryFilter,
    setFilter: (filter: I.CategoryFilter) => void;
    onClose: () => void;
}

const Filter = (props: FiltersProps) => {
    const {theme} = useTheme();
    const style = getFilterStyles(theme);
    const styleCadastro = getStyleCadastro(theme);

    const situations = [
        {Id: Situation.NotConsolidated, Name: "Não"},
        {Id: Situation.Consolidated, Name: "Sim"}
    ]
    const [search, setSearch] = useState("");
    const [situation, setSituation] = useState(Situation.All);

    useEffect(() => {
        setSearch(props.filter.Search);
        setSituation(props.filter.Situation ?? Situation.All);
    }, []);

    const handleClean = () => {
        setSearch("");
        setSituation(Situation.All);
    }

    const handleApply = () => {
        let categoryFilter = {} as I.CategoryFilter;
        categoryFilter.Search = search?.toLowerCase() ?? "";
        categoryFilter.Situation = situation;

        props.setFilter(categoryFilter);
        props.onClose();
    }

    return(
        <>
            <ScrollView style={style.areaFields}>
                <TextItem
                    text="Search"
                    value={search}
                    setValue={setSearch}
                    width="100%"
                />
                <Picker
                    data={situations}
                    text={"Ativo"}
                    value={situation}
                    setValue={setSituation}
                    valueDefault={Situation.All}
                />
            </ScrollView>
            <View style={styleCadastro.areaButtonSave}>
                <Button
                    label={"Limpar"}
                    onPress={handleClean}
                    type={"secondary"}
                />
                <Button
                    label={"Aplicar"}
                    onPress={handleApply}
                />
            </View>
        </>
    );
}

export default Filter;