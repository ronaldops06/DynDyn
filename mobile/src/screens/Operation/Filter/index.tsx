import React, {useEffect, useState} from "react";
import * as I from "../../../interfaces/interfaces.tsx";
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import Picker from "../../../components/CustomPicker";
import TextItem from "../../../components/CustomTextInput";
import {Situation, TypesCategory} from "../../../enums/enums.tsx";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getFilterStyles} from "./styles";
import Button from "../../../components/Button";
import {ActivityIndicator, View} from "react-native";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import Select from "../../../components/Select";

interface FiltersProps {
    filter: I.OperationFilter
    setFilter: (filter: I.OperationFilter) => void;
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
    const [loading, setLoading] = useState(false);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [search, setSearch] = useState("");
    const [situation, setSituation] = useState(Situation.All);
    const [category, setCategory] = useState(0);
    const [salary, setSalary] = useState(Situation.All);
    const [recurrent, setRecurrent] = useState(Situation.All);

    useEffect(() => {
        getListsAndSetFilters();
    }, []);

    const getListsAndSetFilters = async () => {
        setLoading(true);
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Operation, null, null);
        
        setCategories(responseCategories?.data ?? []);

        if (responseCategories?.data === null) {
            setLoading(false);
            return;
        }

        setSearch(props.filter.Search);
        setSituation(props.filter.Situation ?? Situation.All);
        setCategory(props.filter.CategoryId);
        setSalary(props.filter.Salary ?? Situation.All);
        setRecurrent(props.filter.Recurrent ?? Situation.All);
        
        setLoading(false);
    }

    const handleClean = () => {
        setSearch("");
        setSituation(Situation.All);
        setCategory(0);
        setSalary(Situation.All);
        setRecurrent(Situation.All);
    }

    const handleApply = () => {
        let operationFilter = {} as I.OperationFilter;
        operationFilter.Search = search?.toLowerCase() ?? "";
        operationFilter.Situation = situation;
        operationFilter.CategoryId = category;
        operationFilter.Salary = salary;
        operationFilter.Recurrent = recurrent;

        props.setFilter(operationFilter);
        props.onClose();
    }

    return (
        <>
        {loading ?
            (<ActivityIndicator size="large" color={theme.colors.quaternaryTextColor}/>) :
            <>
                <View style={style.areaFields}>
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
                    <Select
                        label={"Categoria"}
                        value={category}
                        setValue={setCategory}
                        data={categories}
                    />
                    <Picker
                        data={situations}
                        text={"Salário"}
                        value={salary}
                        setValue={setSalary}
                        valueDefault={Situation.All}
                    />
                    <Picker
                        data={situations}
                        text={"Recorente"}
                        value={recurrent}
                        setValue={setRecurrent}
                        valueDefault={Situation.All}
                    />
                </View>
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
        }
        </>
    );
}

export default Filter;