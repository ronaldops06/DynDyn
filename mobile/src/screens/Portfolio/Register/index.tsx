import React, {useEffect, useState} from "react";
import {Alert, Switch, Text, TouchableOpacity, View} from "react-native";
import {useFocusEffect} from "@react-navigation/native";
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
import {getCurrentStack, toLocalDate, validateLogin, validateSuccess} from "../../../utils.ts";
import ButtonSelectBar, {ButtonsSelectedProps} from "../../../components/ButtonSelectBar";
import {getPortfolioRegisterStyle} from "./styles";
import Button from "../../../components/Button";
import AuxiliaryButton from "../../../components/AuxiliaryButton";
import Moment from "moment";
import {loadAllCategoryInternal} from "../../../controller/category.controller.tsx";
import {TypesCategory} from "../../../enums/enums.tsx";
import {CustomAlert} from "../../../components/CustomAlert";
import {PortfolioAttribute} from "../../../interfaces/interfaces.tsx";
import PortfolioAttributeModal from "../PortfolioAttributeModal";
import Icon from "../../../components/Icon";

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
    const [category, setCategory] = useState(0);
    const [parentPortfolio, setParentPortfolio] = useState(0);
    const [description, setDescription] = useState("");
    const [acquisitionDate, setAcquisitionDate] = useState("");
    const [saleDate, setSaleDate] = useState("");
    const [acquisitionValue, setAcquisitionValue] = useState<number>();
    const [currency, setCurrency] = useState(1);
    const [status, setStatus] = useState(true);
    const [portfolioAttributes, setPortfolioAttributes] = useState<I.PortfolioAttribute[]>([] as I.PortfolioAttribute[]);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [showModalAttributeView, setShowModalAttributeView] = useState(false);
    const [isEditingAttribute, setIsEditingAttribute] = useState(false);
    const [portfolioAtrributeSelected, setPortfolioAtrributeSelected] = useState<I.PortfolioAttribute>();

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.referenceId !== undefined && route.params?.reference === constants.operations.category) {
                getCategories();
                setCategory(route.params.referenceId);
            }
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

        await getCategories();
    };

    const getCategories = async () => {
        const responseCategories = await loadAllCategoryInternal(TypesCategory.Account, null, true);
        validateLogin(responseCategories, navigation);
        setCategories(responseCategories?.data ?? []);
    }

    const loadDataScreen = () => {
        const data = route.params?.data;

        if (data !== undefined) {
            setName(data.Name);
            setType(data.Type);
            setGroup(data.Group);
            setCategory(data.Category.Id);
            setDescription(data.Description);
            setParentPortfolio(data.ParentPortfolio?.Id ?? 0);
            setStatus(data.Status === constants.status.active.Id);
            setAcquisitionDate(data.DataCriacao);
            setAcquisitionValue(data.AcquisitionCost);
            setSaleDate(data.EndDate);
            setCurrency(data.CurrencyCode);
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
        await CustomAlert("Atenção!",
            "Este patrimônio será excluído. Deseja continuar?",
            async () => {
                let response = await excludePortfolio(portfolioId, portfolioInternalId);
                validateLogin(response, navigation);
                validateSuccess(response, navigation, sourceScreen);
            }
        );
    };

    const handleEditClick = (item: I.PortfolioAttribute) => {
        setIsEditingAttribute(true);
        setPortfolioAtrributeSelected(item);
        setShowModalAttributeView(true);
    }

    const handleSaveClick = async () => {
        if (!validateRequiredFields()) return;

        setLoading(true);

        const portfolioDto = {} as I.Portfolio;
        portfolioDto.Id = portfolioId;
        portfolioDto.InternalId = portfolioInternalId;
        portfolioDto.Name = name;
        portfolioDto.Type = type;
        portfolioDto.Group = group;
        portfolioDto.Category = categories.find(item => item.Id === category) ?? {} as I.Category;
        portfolioDto.ParentPortfolio = parentPortfolio > 0
            ? portfolios.find(item => item.Id === parentPortfolio) ?? null
            : null;
        portfolioDto.Status = status ? constants.status.active.Id : constants.status.inactive.Id;
        portfolioDto.Description = description;
        portfolioDto.EndDate = toLocalDate(saleDate);
        portfolioDto.DataCriacao = toLocalDate(acquisitionDate);
        portfolioDto.AcquisitionCost = acquisitionValue;
        portfolioDto.CurrencyCode = currencies.find(x => x.Id === currency)?.Code ?? "";
        portfolioDto.Attributes = portfolioAttributes;

        let response = {} as I.Response;
        if (isEditing)
            response = await alterPortfolio(portfolioDto);
        else
            response = await createPortfolio(portfolioDto);

        setLoading(false);

        validateLogin(response, navigation);
        validateSuccess(response, navigation, sourceScreen, reference);
    };

    const addPortfolioAttribute = (item: I.PortfolioAttribute) => {
        let index = portfolioAttributes.findIndex(x => x.Attribute.InternalId === item.InternalId);

        if (index > 0) {
            portfolioAttributes.splice(index, 1);
        }

        portfolioAttributes.push(item);
        setPortfolioAttributes(portfolioAttributes);
        setIsEditingAttribute(false);
        setPortfolioAtrributeSelected(undefined);
    }

    const renderSectionTitle = (title: string) => (
        <View style={styles.divider}>
            <Text style={styles.sectionTitle}>{title}</Text>
        </View>
    );

    const getValueAttribute = (portfolioAttribute: PortfolioAttribute) => {

        return (
            <>
                {(() => {
                    switch (portfolioAttribute.Attribute.DataType) {
                        case constants.attributeDataType.text.Id:
                            return <Text style={styles.infoValue}>{portfolioAttribute.ValueText ?? ""}</Text>;
                        case constants.attributeDataType.number.Id:
                            return <Text style={styles.infoValue}>{portfolioAttribute.ValueNumber ?? "0.00"}</Text>;
                        case constants.attributeDataType.boolean.Id:
                            return <Text
                                style={styles.infoValue}>{(portfolioAttribute.ValueBoolean) ? 'Verdadeiro' : 'Falso'}</Text>;
                        case constants.attributeDataType.date.Id:
                            return <Text
                                style={styles.infoValue}>{portfolioAttribute.ValueDate ? Moment(portfolioAttribute.ValueDate).format('DD/MM/YYYY') : "-"}</Text>;
                        case constants.attributeDataType.listOptions.Id:
                            return <Text
                                style={styles.infoValue}>{portfolioAttribute.AttributeOption?.Label}</Text>
                    }
                })()}
            </>
        );
    }

    const currencies = [
        {Id: 1, Name: "BRL - Real", Code: "BRL"},
        {Id: 2, Name: "USD - Dólar", Code: "USD"},
        {Id: 3, Name: "EUR - Euro", Code: "EUR"}
    ];

    return (
        <PageSpecial
            title={isEditing ? "Editar Patrimônio" : "Novo Patrimônio"}
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
                    label="Categoria *"
                    value={category}
                    setValue={setCategory}
                    data={categories}
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
                    value={acquisitionValue?.toString() ?? ""}
                    setValue={setAcquisitionValue}
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
                    Defina campos personalizados para armazenar informações específicas deste patrimônio.
                </Text>

                <View style={styles.box}>
                    {portfolioAttributes?.map((attr: I.PortfolioAttribute, index: number) => (
                        <View key={index} style={styles.infoRow}>
                            <View style={styles.infoLabel}>
                                <Text style={styles.infoLabelText}>{attr.Attribute.Name}</Text>
                            </View>
                            {getValueAttribute(attr)}
                            <TouchableOpacity 
                                style={styles.infoAction}
                                onPress={() => handleEditClick(attr)}>
                                <Icon name="edit" size={20} color={theme.colors.quaternaryIcon}/> 
                            </TouchableOpacity>
                        </View>
                    ))}
                </View>

                <AuxiliaryButton
                    text="Adicionar atributo"
                    onPress={() => setShowModalAttributeView(true)}
                    icon="plus"
                    iconColor={theme.colors.primaryTextColor}
                    type="secondary"
                />

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
                            <Text style={styles.deleteText}>Excluir patrimônio</Text>
                        </TouchableOpacity>}
                </View>
            </View>
            <PortfolioAttributeModal
                show={showModalAttributeView}
                setShow={setShowModalAttributeView}
                addPortfolioAttribute={addPortfolioAttribute}
                isEditing={isEditingAttribute}
                portfolioAttribute={portfolioAtrributeSelected}
            />
        </PageSpecial>
    );
};

export default PortfolioRegister;
