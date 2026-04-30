import React, {useEffect, useState} from "react";
import {ActivityIndicator, View} from "react-native";
import * as I from "../../../interfaces/interfaces.tsx";
import TextItem from "../../../components/CustomTextInput";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import Button from "../../../components/Button";
import {getFilterStyles} from "./styles";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";

interface FiltersProps {
    filter: I.OperationRoleFilter
    setFilter: (filter: I.OperationRoleFilter) => void;
    onClose: () => void;
}

const Filter = (props: FiltersProps) => {
    const {theme} = useTheme();
    const style = getFilterStyles(theme);
    const styleCadastro = getStyleCadastro(theme);
    
    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState("");

    useEffect(() => {
        getListsAndSetFilters();
    }, []);

    const getListsAndSetFilters = async () => {
        setLoading(true);
 
        setSearch(props.filter.Search);
        
        setLoading(false);
    }

    const handleClean = () => {
        setSearch("");
    }

    const handleApply = () => {
        let operationFilter = {} as I.OperationFilter;
        operationFilter.Search = search?.toLowerCase() ?? "";

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