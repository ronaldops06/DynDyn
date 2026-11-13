import React, {useEffect, useState} from "react";
import {ActivityIndicator, View} from "react-native";
import Picker from "../../../components/CustomPicker";
import TextItem from "../../../components/CustomTextInput";
import Button from "../../../components/Button";
import FieldFilterValue from "../../../components/FieldFilterValue";
import Select from "../../../components/Select";

import * as I from "../../../interfaces/interfaces";
import {loadAllPortfolioInternal} from "../../../controller/portfolio.controller";
import {loadAllCategoryInternal} from "../../../controller/category.controller";
import {Situation, TypesCategory} from "../../../enums/enums";
import {constants} from "../../../constants";

import {useTheme} from "../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../styles/styles.cadastro";
import {getFilterStyles} from "./styles";

interface FiltersProps {
    filter: I.PortfolioFilter
    setFilter: (filter: I.PortfolioFilter) => void;
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
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [search, setSearch] = useState("");
    const [situation, setSituation] = useState<number>(Situation.All);
    const [parentPortfolio, setParentPortfolio] = useState(0);
    const [category, setCategory] = useState(0);
    const [value, setValue] = useState<I.ValueFilter>({} as I.ValueFilter);

    useEffect(() => {
        getListsAndSetFilters();
    }, []);

    const getListsAndSetFilters = async () => {
        setLoading(true);
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Account, null, null);

        let groupsPortfolios = [];
        groupsPortfolios.push(constants.portfolioGroupType.ativo.contasBancarias.Id);
        groupsPortfolios.push(constants.portfolioGroupType.passivo.contasBancarias.Id);
        
        let responsePortfolios = await loadAllPortfolioInternal(null, groupsPortfolios, null);
        
        setCategories(responseCategories?.data ?? []);

        setPortfolios(responsePortfolios?.data ?? []);

        if (responseCategories?.data === null) {
            setLoading(false);
            return;
        }

        setSearch(props.filter.Search);
        setSituation(props.filter.Situation ?? Situation.All);
        setCategory(props.filter.CategoryId);
        setParentPortfolio(props.filter.ParentPortfolioId);
        setValue(props.filter.ValueFilter);
        
        setLoading(false);
    }

    const handleClean = () => {
        setSearch("");
        setSituation(Situation.All);
        setCategory(0);
        setParentPortfolio(0);
        setValue({} as I.ValueFilter);
    }

    const handleApply = () => {
        let portfolioFilter = {} as I.PortfolioFilter;
        portfolioFilter.Search = search?.toLowerCase() ?? "";
        portfolioFilter.Situation = situation;
        portfolioFilter.CategoryId = category;
        portfolioFilter.ParentPortfolioId = parentPortfolio;
        portfolioFilter.ValueFilter = value;

        props.setFilter(portfolioFilter);
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
                    <Select
                        label={"Conta Pai"}
                        value={parentPortfolio}
                        setValue={setParentPortfolio}
                        data={portfolios}
                    />
                    <FieldFilterValue value={value} setValue={setValue} />
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