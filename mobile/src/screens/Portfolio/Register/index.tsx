import React, {useEffect, useState} from "react";
import {ActivityIndicator, Alert, Switch, Text, TouchableOpacity, View} from "react-native";
import {useFocusEffect} from "@react-navigation/native";

import CameraIcon from "../../../assets/portfolio.svg";
import PlusIcon from "../../../assets/plus.svg";
import TextInputCustom from "../../../components/CustomTextInput";
import Select from "../../../components/Select";
import TextArea from "../../../components/TextArea";
import DateTimeInput from "../../../components/DateTimeInput";
import {PageSpecial} from "../../../components/Page";
import * as I from "../../../interfaces/interfaces.tsx";
import {
    alterPortfolio,
    createPortfolio,
    excludePortfolio,
    loadAllPortfolioInternal
} from "../../../controller/portfolio.controller.tsx";
import {constants} from "../../../constants";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getCurrentStack, validateLogin, validateSuccess} from "../../../utils.ts";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../../components/ButtonSelectBar";
import {getPortfolioRegisterStyle} from "./styles";
import Button from "../../../components/Button";

interface ExampleAttribute {
    Id: number;
    Name: string;
}

const PortfolioRegister = ({navigation, route}: any) => {
    const {theme} = useTheme();
    const styles = getPortfolioRegisterStyle(theme);

    const portfolioId = route.params?.data?.Id ?? 0;
    const portfolioInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;
    const sourceScreen = route.params?.sourceScreen ?? "Portfolio";
    const reference = route.params?.reference;

    const [stack, setStack] = useState("");
    const [loading, setLoading] = useState(false);
    const [name, setName] = useState("");
    const [type, setType] = useState(constants.portfolioType.ativo.Id);
    const [group, setGroup] = useState(0);
    const [parentPortfolio, setParentPortfolio] = useState(0);
    const [description, setDescription] = useState("");
    const [acquisitionDate, setAcquisitionDate] = useState("");
    const [saleDate, setSaleDate] = useState("");
    const [acquisitionValue, setAcquisitionValue] = useState("");
    const [currentValue, setCurrentValue] = useState("");
    const [currency, setCurrency] = useState(1);
    const [financialNotes, setFinancialNotes] = useState("");
    const [status, setStatus] = useState(true);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);

    useFocusEffect(
        React.useCallback(() => {
            /*if (route.params?.referenceId !== undefined && route.params?.reference === constants.operations.category) {
                getCategories();
                setCategory(route.params.referenceId);
            }*/
        }, [route.params?.actionNavigation])
    );

    useEffect(() => {
        getLists();

        if (isEditing)
            loadDataScreen();

        setStack(getCurrentStack(navigation));
    }, []);

    const getLists = async () => {

        let groupsPortfolios = [];
        groupsPortfolios.push(constants.portfolioGroupType.ativo.contasBancarias.Id);
        groupsPortfolios.push(constants.portfolioGroupType.passivo.contasBancarias.Id);
        
        const responsePortfolios = await loadAllPortfolioInternal(null, groupsPortfolios, true);
        validateLogin(responsePortfolios, navigation);
        setPortfolios(responsePortfolios?.data ?? []);
    };
    
    const loadDataScreen = () => {
        const data = route.params?.data;

        if (data !== undefined) {
            setName(data.Name);
            setType(data.Type);
            setGroup(data.Group);
            setParentPortfolio(data.ParentPortfolio?.Id ?? 0);
            setStatus(data.Status === constants.status.active.Id);
        }
    };

    const getButtonsSelectedBar = (): ButtonsSelectedProps[] => {
        let buttonsSelectedBar: ButtonsSelectedProps[] = [];

        Object.values(constants.portfolioType).map(type => {
            buttonsSelectedBar.push({text: type.Name, value: type.Id});
        });

        return buttonsSelectedBar;
    }

    const getPortfolioGroups = () => {
        const activeGroups = Object.values(constants.portfolioGroupType.ativo);
        const liabilityGroups = Object.values(constants.portfolioGroupType.passivo);

        return [...activeGroups, ...liabilityGroups].filter(item => item.Type === type);
    };

    const validateRequiredFields = () => {
        if (!name) {
            Alert.alert("Atenção!", "O nome do portfólio deve ser informado.");
            return false;
        }
        
        if (group === 0) {
            Alert.alert("Atenção!", "O grupo deve ser selecionado.");
            return false;
        }

        if (!acquisitionDate) {
            Alert.alert("Atenção!", "A data de aquisição deve ser informada.");
            return false;
        }

        if (!acquisitionValue) {
            Alert.alert("Atenção!", "O valor de aquisição deve ser informado.");
            return false;
        }

        return true;
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Este portfólio será excluído. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        const response = await excludePortfolio(portfolioId, portfolioInternalId);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, sourceScreen);
                    }
                }
            ],
            {cancelable: false}
        );
    };

    const handleSaveClick = async () => {
        if (!validateRequiredFields()) return;

        setLoading(true);

        const portfolioDTO = {} as I.Portfolio;
        portfolioDTO.Id = portfolioId;
        portfolioDTO.InternalId = portfolioInternalId;
        portfolioDTO.Name = name;
        portfolioDTO.Type = type;
        portfolioDTO.Group = group;
        //portfolioDTO.Category = categories.find(item => item.Id === category) ?? {} as I.Category;
        portfolioDTO.ParentPortfolio = parentPortfolio > 0
            ? portfolios.find(item => item.Id === parentPortfolio) ?? null
            : null;
        portfolioDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let response = {} as I.Response;
        if (isEditing)
            response = await alterPortfolio(portfolioDTO);
        else
            response = await createPortfolio(portfolioDTO);

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, sourceScreen, reference);
    };

    const handleTypeChange = (value: number) => {
        setType(value);
        setGroup(0);
    };

    const renderSectionTitle = (title: string) => (
        <View style={styles.divider}>
            <Text style={styles.sectionTitle}>{title}</Text>
        </View>
    );

    const currencies = [
        {Id: 1, Name: "BRL - Real"},
        {Id: 2, Name: "USD - Dólar"},
        {Id: 3, Name: "EUR - Euro"}
    ];
    
    return (
        <PageSpecial
            title={isEditing ? "Editar Portfólio" : "Novo Portfólio"}
            helpType="portfolio_register"
            onBackClick={handleBackClick}
        >
            <View style={styles.container}>
                <View style={styles.areaButtonType}>
                    <ButtonSelectBar
                        buttons={getButtonsSelectedBar()}
                        valueSelected={type}
                        handleValueSelected={setType}
                        disabled={false}
                    />
                </View>
                
                {renderSectionTitle("Informações básicas")}
                                
                <TextInputCustom
                    text="Nome *"
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
                
                <Select
                    label="Grupo *"
                    value={group as any}
                    setValue={setGroup}
                    data={getPortfolioGroups()}
                />

                <Select
                    label="Portfólio pai"
                    value={parentPortfolio as any}
                    setValue={setParentPortfolio}
                    data={portfolios.filter(item => item.Id !== portfolioId)}
                    parentScreen={stack}
                />
                
                <TextArea
                    label="Descrição"
                    value={description}
                    setValue={setDescription}
                    placeholder="Adicione uma descrição (opcional)"
                />

                <View style={styles.switchRow}>
                    <Text style={styles.switchLabel}>Patrimônio Aberto</Text>
                    <Switch
                        value={status}
                        onValueChange={setStatus}
                        trackColor={{
                            false: theme.colors.tertiaryBaseColor,
                            true: theme.colors.tertiaryBaseColor
                        }}
                        thumbColor={theme.colors.primaryBaseColor}
                    />
                </View>

                <DateTimeInput
                    dateLabel="Data de aquisição *"
                    dateValue={acquisitionDate}
                    setDateValue={setAcquisitionDate}
                />
                <DateTimeInput
                    dateLabel="Data de venda"
                    dateValue={saleDate}
                    setDateValue={setSaleDate}
                />
                
                {renderSectionTitle("Informações financeiras")}

                <TextInputCustom
                    text="Valor de aquisição (R$) *"
                    isMoveText={false}
                    value={acquisitionValue}
                    setValue={setAcquisitionValue}
                    keyboardType="numeric"
                />

                <TextInputCustom
                    text="Valor atual (R$)"
                    isMoveText={false}
                    value={currentValue}
                    setValue={setCurrentValue}
                    keyboardType="numeric"
                />

                <Select
                    label="Moeda"
                    value={currency as any}
                    setValue={setCurrency}
                    data={currencies}
                />
                                
                {renderSectionTitle("Atributos personalizados")}
                <Text style={styles.helperText}>
                    Crie campos personalizados para armazenar informações específicas deste portfólio.
                </Text>

                <TouchableOpacity style={styles.attributeButton}>
                    <PlusIcon width="22" height="22" fill={theme.colors.primaryTextColor}/>
                    <Text style={styles.attributeButtonText}>Adicionar atributo</Text>
                </TouchableOpacity>
                
                <View style={styles.actions}>
                    <Button
                        label={"Salvar"}
                        onPress={handleSaveClick}
                        loading={loading}
                        disabled={loading}
                    />
                    {isEditing &&
                        <TouchableOpacity
                            style={styles.deleteButton}
                            onPress={handleTrashClick}
                        >
                            <Text style={styles.deleteText}>Excluir portfólio</Text>
                        </TouchableOpacity>}
                </View>
            </View>
        </PageSpecial>
    );
};

export default PortfolioRegister;
