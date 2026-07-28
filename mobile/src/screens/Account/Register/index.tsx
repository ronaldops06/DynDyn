import React, {useCallback, useEffect, useState} from "react";
import {useFocusEffect} from "@react-navigation/native";
import {Alert, Text, View} from "react-native";
import * as I from "../../../interfaces/interfaces.tsx";
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import {TypesCategory} from "../../../enums/enums.tsx";
import {
    alterPortfolio,
    createPortfolio,
    excludePortfolio,
    loadAllPortfolioInternal
} from "../../../controller/portfolio.controller.tsx";
import TextInput from "../../../components/CustomTextInput";
import CheckBox from "@react-native-community/checkbox";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../../components/ButtonSelectBar";
import Select from "../../../components/Select";

import {getCurrentStack, validateLogin, validateSuccess} from "../../../utils.ts";
import {constants} from "../../../constants";
import {useTheme} from '../../../contexts/ThemeContext';
import {getStyle} from "../../../styles/styles.ts";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import {getAccountCreateStyle} from "./styles";
import {PageRegister} from "../../../components/Page";

const PortfolioCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const accountCreateStyle = getAccountCreateStyle(theme);

    const portfolioId = route.params?.data?.Id ?? 0;
    const portfolioInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;
    const sourceScreen = route.params?.sourceScreen ?? "AccountHome";
    const reference = route.params?.reference;

    const [stack, setStack] = useState("");
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState<string>("");
    const [type, setType] = useState<number>(1);
    const [category, setCategory] = useState(0);
    const [parentPortfolio, setParentPortfolio] = useState(0);
    const [status, setStatus] = useState<boolean>(true);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);

    useFocusEffect(
        useCallback(() => {
            if (route.params?.referenceId !== undefined) {
                let reference = route.params?.reference;
                
                if (reference === constants.operations.category)
                {
                    getCategories();

                    setCategory(route.params?.referenceId);
                }
            }
        }, [route.params?.actionNavigation])
    );
    
    useEffect(() => {
        getLists();
        if (isEditing) {
            loadDataSreen();
        }

        const tab = getCurrentStack(navigation);
        setStack(tab);
    }, [])

    const getLists = async () => {
        await getCategories();

        let groupsPortfolios = [];
        groupsPortfolios.push(constants.portfolioGroupType.ativo.contasBancarias.Id);
        groupsPortfolios.push(constants.portfolioGroupType.passivo.contasBancarias.Id);
        
        let responsePortfolios = await loadAllPortfolioInternal(null, groupsPortfolios, true);
        validateLogin(responsePortfolios, navigation);
        
        setPortfolios(responsePortfolios?.data ?? []);
    }

    const getCategories = async () => {
        let responseCategories = await loadAllCategoryInternal(TypesCategory.Account, null, true);
        validateLogin(responseCategories, navigation);
        setCategories(responseCategories?.data ?? []);
    }

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
            setType(data.Type);
            setCategory(data.Category.Id);
            setParentPortfolio(data.ParentPortfolio?.Id ?? 0);
            setStatus(data.Status === constants.status.active.Id);
        }
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const getButtonsSelectedBar = (): ButtonsSelectedProps[] => {
        let buttonsSelectedBar: ButtonsSelectedProps[] = [];

        Object.values(constants.portfolioType).map(type => {
            buttonsSelectedBar.push({text: type.Name, value: type.Id});
        });

        return buttonsSelectedBar;
    }

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Esta conta será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludePortfolio(portfolioId, portfolioInternalId);
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

        if (category === 0) {
            Alert.alert("Atenção!", "A categoria deve ser selecionada.");
            return false;
        }

        return true;
    }

    const handleSaveClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);

        let portfolioDTO = {} as I.Portfolio;
        portfolioDTO.Id = portfolioId;
        portfolioDTO.InternalId = portfolioInternalId;
        portfolioDTO.Name = name;
        portfolioDTO.Type = type;
        portfolioDTO.Group = constants.portfolioGroupType.contasBancarias.Id;
        portfolioDTO.Category = categories.find(x => x.Id === category) ?? {} as I.Category;
        portfolioDTO.ParentPortfolio = (parentPortfolio > 0) ? portfolios.find(x => x.Id === parentPortfolio) ?? null : null;
        portfolioDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterPortfolio(portfolioDTO);
        else
            response = await createPortfolio(portfolioDTO);

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, sourceScreen, reference);
    };

    return (
        <PageRegister 
            onTrashClick={handleTrashClick} 
            onBackClick={handleBackClick} 
            onSaveClick={handleSaveClick} 
            helpType={"account_register"}
            isEditing={isEditing} 
            isLoading={loading}>
            <View style={accountCreateStyle.areaButtonType}>
                <ButtonSelectBar
                    buttons={getButtonsSelectedBar()}
                    valueSelected={type}
                    handleValueSelected={setType}
                    disabled={false}
                />
            </View>
            <View style={accountCreateStyle.areaFields}>
                <TextInput
                    text={"Nome"}
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
                <Select
                    label={"Categoria"}
                    value={category}
                    setValue={setCategory}
                    data={categories}
                    parentScreen={stack}
                    registerScreen={"CategoryCreate"}
                    navigation={navigation}
                    sourceScreen={route.name}
                    reference={constants.operations.category}
                />
                <Select
                    label={"Conta Pai"}
                    value={parentPortfolio}
                    setValue={setParentPortfolio}
                    data={portfolios}
                    parentScreen={stack}
                />
                <View style={accountCreateStyle.areaCard}>
                    <CheckBox
                        value={status}
                        onValueChange={setStatus}
                        tintColors={{true: theme.colors.primaryTextColor, false: theme.colors.primaryTextColor}}
                    />
                    <Text
                        style={styleCadastro.textCheckbox}>Ativo</Text>
                </View>
            </View>
        </PageRegister>
    );
}

export default PortfolioCreate