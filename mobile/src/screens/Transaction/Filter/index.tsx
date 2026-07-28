import React, {useEffect, useState} from "react";
import * as I from "../../../interfaces/interfaces.tsx";
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import {loadAllPortfolioInternal} from "../../../controller/portfolio.controller.tsx";
import {loadAllOperationInternal} from "../../../controller/operation.controller.tsx";
import Picker from "../../../components/CustomPicker";
import TextItem from "../../../components/CustomTextInput";
import {TypesCategory} from "../../../enums/enums.tsx";
import {Situation} from "../../../enums/enums.tsx";
import {TransactionFilter} from "../../../interfaces/interfaces.tsx";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getFilterStyles} from "./styles";
import Button from "../../../components/Button";
import {ActivityIndicator, ScrollView, View} from "react-native";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import FieldFilterValue from "../../../components/FieldFilterValue";
import Select from "../../../components/Select";

interface FiltersProps {
    filter: TransactionFilter
    setFilter: (filter: I.TransactionFilter) => void;
    onClose: () => void;
}

const Filter = (props: FiltersProps) => {
    const {theme} = useTheme();
    const style = getFilterStyles(theme);
    const styleCadastro = getStyleCadastro(theme);

    const situations = [
        {Id: Situation.NotConsolidated, Name: "Pendente"},
        {Id: Situation.Consolidated, Name: "Consolidado"}
    ]
    const [loading, setLoading] = useState(false);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [operations, setOperations] = useState<I.Operation[]>([]);
    const [search, setSearch] = useState("");
    const [situation, setSituation] = useState(Situation.All);
    const [category, setCategory] = useState(0);
    const [portfolio, setPortfolio] = useState(0);
    const [operation, setOperation] = useState(0);
    const [value, setValue] = useState<I.ValueFilter>({} as I.ValueFilter);

    useEffect(() => {
        getListsAndSetFilters();
    }, []);

    const getListsAndSetFilters = async () => {
        setLoading(true);
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Operation, null, null);

        let groupsPortfolios = [];
        groupsPortfolios.push(constants.portfolioGroupType.ativo.contasBancarias.Id);
        groupsPortfolios.push(constants.portfolioGroupType.passivo.contasBancarias.Id);
        
        let responsePortfolios = await loadAllPortfolioInternal(null, groupsPortfolios, null);

        let responseOperations = await loadAllOperationInternal(null, null, null);

        setCategories(responseCategories?.data ?? []);
        setPortfolios(responsePortfolios?.data ?? []);
        setOperations(responseOperations?.data ?? []);

        if (responseCategories?.data === null) {
            setLoading(false);
            return;
        }

        setSearch(props.filter.Search);
        setSituation(props.filter.Situation ?? Situation.All);
        setCategory(props.filter.CategoryId);
        setPortfolio(props.filter.PortfolioId);
        setOperation(props.filter.OperationId);
        setValue(props.filter.ValueFilter);

        setLoading(false);
    }

    const handleClean = () => {
        setSearch("");
        setSituation(Situation.All);
        setCategory(0);
        setPortfolio(0);
        setOperation(0);
        setValue(null);
    }

    const handleApply = () => {
        let transactionFilter = {} as TransactionFilter;
        transactionFilter.Search = search?.toLowerCase() ?? "";
        transactionFilter.Situation = situation;
        transactionFilter.CategoryId = category;
        transactionFilter.OperationId = operation;
        transactionFilter.PortfolioId = portfolio;
        transactionFilter.ValueFilter = value;

        props.setFilter(transactionFilter);
        props.onClose();
    }

    return (
        <>
            {loading ?
                (<ActivityIndicator size="large" color={theme.colors.quaternaryTextColor}/>) :
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
                            text={"Situação"}
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
                        <Select
                            label={"Operação"}
                            value={operation}
                            setValue={setOperation}
                            data={operations}
                        />
                        <Select
                            label={"Conta"}
                            value={portfolio}
                            setValue={setPortfolio}
                            data={portfolios}
                        />
                        <FieldFilterValue value={value} setValue={setValue}/>
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
        }
        </>
    );
}

export default Filter;