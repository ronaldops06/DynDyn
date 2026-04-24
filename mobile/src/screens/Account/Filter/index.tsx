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
import {loadAllPortfolioInternal} from "../../../controller/portfolio.controller.tsx";
import FieldFilterValue from "../../../components/FieldFilterValue";

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

        let responsePortfolios = await loadAllPortfolioInternal(null, null);
        
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
        setValue(null);
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
                    <Picker
                        data={categories}
                        text={"Categoria"}
                        value={category}
                        setValue={setCategory}
                    />
                    <Picker
                        data={portfolios}
                        text={"Conta Pai"}
                        value={parentPortfolio}
                        setValue={setParentPortfolio}
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