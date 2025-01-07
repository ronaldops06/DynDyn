import {StackNavigationProp} from "@react-navigation/stack";
import {RootStackParamList} from "../RootStackParams.ts";
import {RouteProp, useNavigation, useRoute} from "@react-navigation/core";
import {constants} from "../../constants";
import React, {useEffect, useState} from "react";
import * as I from "../../interfaces/interfaces.tsx";
import {loadAllCategory} from "../../controller/category.controller.tsx";
import {TypesCategory} from "../../enums/enums.tsx";
import {Alert, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from "react-native";
import {alterAccount, createAccount, excludeAccount, loadAllAccount} from "../../controller/account.controller.tsx";
import {style} from "../../styles/styles.ts";
import {styleCadastro} from "../../styles/styles.cadastro.ts";
import PrevIcon from "../../assets/nav_prev.svg";
import TrashIcon from "../../assets/trash.svg";
import {accountCreateStyle} from "./create.styles";
import TextInput from "../../components/CustomTextInput";
import Picker from "../../components/CustomPicker";
import CheckBox from "@react-native-community/checkbox";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'AccountCreate'>;
const AccountCreate = () => {

    const navigation = useNavigation<homeScreenProp>();
    const route = useRoute<RouteProp<RootStackParamList, 'AccountCreate'>>();

    const accountId = route.params?.data.Id ?? 0;
    const accountInternalId = route.params?.data.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [name, setName] = useState<string>("");
    const [category, setCategory] = useState(0);
    const [parentAccount, setParentAccount] = useState(0);
    const [status, setStatus] = useState<boolean>(true);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [accounts, setAccounts] = useState<I.Account[]>([]);

    useEffect(() => {
        getLists();
        if (isEditing) {
            loadDataSreen();
        }
    }, [])

    const getLists = async () => {
        let responseCategories = await loadAllCategory(TypesCategory.Account, null, navigation);
        
        let responseAccounts = await loadAllAccount(null, navigation);

        setCategories(responseCategories?.data ?? []);
        setAccounts(responseAccounts?.data ?? []);
    }

    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
            setCategory(data.Category.Id);
            setParentAccount(data.ParentAccount?.Id ?? 0);
            setStatus(data.Status === constants.status.active.Id);
        }
    };

    const handleBackClick = () => {
        navigation.goBack();
    };

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
                        let success = await excludeAccount(accountId, accountInternalId, navigation);
                        if (success)
                            navigation.goBack();
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

        let accountDTO = {} as I.Account;
        accountDTO.Id = accountId;
        accountDTO.InternalId = accountInternalId;
        accountDTO.Name = name;
        accountDTO.Category = categories.find(x => x.Id === category) ?? {} as I.Category;
        accountDTO.ParentAccount = (parentAccount > 0) ? accounts.find(x => x.Id === parentAccount) ?? null : null;
        accountDTO.Status = status ? constants.status.active.Id : constants.status.inactive.Id;

        let success = false;
        if (isEditing)
            success = await alterAccount(accountDTO, navigation);
        else
            success = await createAccount(accountDTO, navigation);
        
        if (success)
            navigation.goBack();
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
                            value={parentAccount}
                            setValue={setParentAccount}
                            data={accounts}
                        />
                        <View style={accountCreateStyle.areaCheckbox}>
                            <CheckBox
                                value={status}
                                onValueChange={setStatus}
                                tintColors={{true: "#6E8BB8", false: "#6E8BB8"}}
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

export default AccountCreate