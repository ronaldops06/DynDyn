import {constants} from "../../constants";
import React, {useEffect, useState} from "react";
import * as I from "../../interfaces/interfaces.tsx";
import {loadAllCategory} from "../../controller/category.controller.tsx";
import {TypesCategory} from "../../enums/enums.tsx";
import {Alert, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from "react-native";
import {
    alterPortfolio,
    createPortfolio,
    excludePortfolio,
    loadAllPortfolio
} from "../../controller/portfolio.controller.tsx";
import {style} from "../../styles/styles.ts";
import {styleCadastro} from "../../styles/styles.cadastro.ts";
import PrevIcon from "../../assets/nav_prev.svg";
import TrashIcon from "../../assets/trash.svg";
import {accountCreateStyle} from "./create.styles";
import TextInput from "../../components/CustomTextInput";
import Picker from "../../components/CustomPicker";
import CheckBox from "@react-native-community/checkbox";
import {validateLogin, validateSuccess} from "../../utils.ts";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../components/ButtonSelectBar";

const PortfolioCreate = ({navigation, route}) => {
    
    const portfolioId = route.params?.data?.Id ?? 0;
    const portfolioInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [name, setName] = useState<string>("");
    const [type, setType] = useState<number>(1);
    const [category, setCategory] = useState(0);
    const [parentPortfolio, setParentPortfolio] = useState(0);
    const [status, setStatus] = useState<boolean>(true);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);

    useEffect(() => {
        getLists();
        if (isEditing) {
            loadDataSreen();
        }
    }, [])

    const getLists = async () => {
        let responseCategories = await loadAllCategory(TypesCategory.Account, null);
        validateLogin(responseCategories, navigation);
        
        let responsePortfolios = await loadAllPortfolio(null);
        validateLogin(responsePortfolios, navigation);

        setCategories(responseCategories?.data ?? []);
        setPortfolios(responsePortfolios?.data ?? []);
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
        if (route.params?.onGoBack) 
            route.params.onGoBack(constants.actionNavigation.none);
        
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
                        validateSuccess(response, navigation, route);
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

        let portfolioDTO = {} as I.Portfolio;
        portfolioDTO.Id = portfolioId;
        portfolioDTO.InternalId = portfolioInternalId;
        portfolioDTO.Name = name;
        portfolioDTO.Type = type;
        portfolioDTO.Group = constants.portfolioGroupType.contasBancarias;
        portfolioDTO.Category = categories.find(x => x.Id === category) ?? {} as I.Category;
        portfolioDTO.ParentPortfolio = (parentPortfolio > 0) ? portfolios.find(x => x.Id === parentPortfolio) ?? null : null;
        portfolioDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterPortfolio(portfolioDTO);
        else
            response = await createPortfolio(portfolioDTO);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, route);
    };
    
    return(
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill="#F1F1F1"/>
                    </TouchableOpacity>
                    {isEditing &&
                        <TouchableOpacity
                            style={styleCadastro.buttonTrash}
                            onPress={handleTrashClick}>
                            <TrashIcon width="35" height="35" fill="#F1F1F1"/>
                        </TouchableOpacity>}
                </View>
                <View style={styleCadastro.viewBodyCadastro}>
                    <ButtonSelectBar
                        buttons={getButtonsSelectedBar()}
                        valueSelected={type}
                        handleValueSelected={setType}
                        disabled={false}
                    />
                    <View style={accountCreateStyle.areaFields}>
                        <TextInput
                            text={"Nome"}
                            isMoveText={false}
                            value={name}
                            setValue={setName}
                        />
                        <Picker
                            text={"Categoria"}
                            value={category}
                            setValue={setCategory}
                            data={categories}
                        />
                        <Picker
                            text={"Conta Pai"}
                            value={parentPortfolio}
                            setValue={setParentPortfolio}
                            data={portfolios}
                        />
                        <View style={accountCreateStyle.areaCheckbox}>
                            <CheckBox
                                value={status}
                                onValueChange={setStatus}
                                tintColors={{true: constants.colors.primaryTextColor, false: constants.colors.primaryTextColor}}
                            />
                            <Text
                                style={styleCadastro.textCheckbox}>Ativo</Text>
                        </View>
                    </View>
                    <View style={accountCreateStyle.areaButtonSave}>
                        <TouchableOpacity
                            style={styleCadastro.buttonSave}
                            onPress={handleSaveClick}
                        >
                            <Text style={styleCadastro.textButtonSave}>Salvar</Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

export default PortfolioCreate