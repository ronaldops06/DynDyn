import React, {useEffect, useState} from 'react';
import {View, Text, TouchableOpacity, SafeAreaView, Alert} from 'react-native';
import { StackNavigationProp } from '@react-navigation/stack';
import { useNavigation } from '@react-navigation/core';

import { RootStackParamList } from '../RootStackParams';

import { style } from '../../styles/styles';
import { accountStyle } from './styles';
import * as I from "../../interfaces/interfaces.tsx";
import {
    alterAccount,
    excludeAccount,
    loadAllAccount,
    loadAllAccountInternal
} from "../../controller/account.controller.tsx";
import {CustomAlert} from "../../components/CustomAlert";
import CustomScroll from "../../components/CustomScroll";
import PlusIcon from "../../assets/plus.svg";
import AccountItem from "./AccountItem";
import {constants} from "../../constants";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Account'>;

const Account = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [loading, setLoading] = useState(false);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [accounts, setAccounts] = useState<I.Account[]>([]);
    
    useEffect(() => {
        // Executado no goBack da tela seguinte
        navigation.addListener('focus', () => {
            setIsLoadInternal(true);
            setAccounts([]);
        });
    }, [navigation]);

    useEffect(() => {
        if (accounts.length === 0) {
            setPageNumber(1);
            loadAccounts();
        }
    }, [accounts]);

    useEffect(() => {
        setIsLoadInternal(true);
        loadAccounts();
    }, [pageNumber]);

    const appendAccounts = (data: I.Account[]) => {
        let accountsNew = accounts;
        if (data.length > 0) {
            data.map((item, key) => {
                accountsNew.push(item);
            });
            setAccounts(accountsNew);
        }
    };

    const loadAccounts = async () => {
        setLoading(true);

        let responseAccounts = null;

        if (isLoadInternal)
            responseAccounts = await loadAllAccountInternal(pageNumber);
        else
            responseAccounts = await loadAllAccount(pageNumber, navigation);

        setTotalPages(responseAccounts?.totalPages ?? 1);
        appendAccounts(responseAccounts?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Account) => {
        if (!isScrolling)
            navigation.navigate("AccountCreate", {isEditing: true, data: data});
    }

    const onSwipeLeft = async (data: I.Account) => {
                
        Alert.alert("Atenção!",
            "Esta conta terá o status alterado. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        data.Status = (data.Status === constants.status.active.Id) ? constants.status.inactive.Id : constants.status.active.Id;
                        data = await alterAccount(data, null);

                        setAccounts((prevAccounts) =>
                            prevAccounts.map((item) =>
                                item.Id === data.Id ? data : item
                            )
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const onSwipeRight = (data: I.Account) => {
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
                        let success = await excludeAccount(data.Id, data.InternalId, null);
                        if (success){
                            setIsLoadInternal(true);
                            setAccounts([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const handleNewClick = () => {
        navigation.navigate("AccountCreate");
    }

    return(
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <Text style={style.textHeaderConsultaTitle}>Contas</Text>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                >
                    {accounts != null && accounts.map((item, key) => (
                        <AccountItem
                            key={key}
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                    ))
                    }
                </CustomScroll>
                <TouchableOpacity
                    style={accountStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill="#6E8BB8"/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Account;