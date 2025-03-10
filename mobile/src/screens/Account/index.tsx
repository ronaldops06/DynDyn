import React, {useEffect, useState, useCallback, useRef} from 'react';
import {View, Text, TouchableOpacity, SafeAreaView, Alert} from 'react-native';
import {StackNavigationProp} from '@react-navigation/stack';
import {useNavigation} from '@react-navigation/core';
import { useFocusEffect } from '@react-navigation/native';
import _ from 'lodash';

import {RootStackParamList} from '../RootStackParams';

import {style} from '../../styles/styles';
import {accountStyle} from './styles';
import * as I from "../../interfaces/interfaces.tsx";
import {
    alterAccount,
    excludeAccount,
    loadAllAccount,
    loadAllAccountInternal
} from "../../controller/account.controller.tsx";
import {loadAllBalance} from "../../controller/balance.controller.tsx";
import CustomScroll from "../../components/CustomScroll";
import PlusIcon from "../../assets/plus.svg";
import AccountItem from "./AccountItem";
import {constants} from "../../constants";
import {validateLogin} from '../../utils.ts';
import AccountIcon from '../../assets/account.svg';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Account'>;

const Account = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [loading, setLoading] = useState(true);
    const isFirstRender = useRef(true);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [accounts, setAccounts] = useState<I.Account[]>([]);
    const [executado, setExecutado] = useState(false);
    
    /*useFocusEffect(
        useCallback(() => {
            
            if (!executado) {
                setPageNumber(1);
                setAccounts([]);
                loadAccounts();
                setExecutado(true);
            }
        }, [])
    );*/

    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

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

        if (isLoadInternal) {
            responseAccounts = await loadAllAccountInternal(pageNumber);
        } else {
            responseAccounts = await loadAllAccount(pageNumber);
            validateLogin(responseAccounts, navigation);
            let response = await loadAllBalance(null);
            //Carrega as contas novamente para pegar os saldos atualizados, na primeira página
            responseAccounts = await loadAllAccountInternal(pageNumber);
        }

        setTotalPages(responseAccounts?.totalPages ?? 1);
        appendAccounts(responseAccounts?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Account) => {
        if (!isScrolling)
            navigation.navigate("AccountCreate", {
                isEditing: true, data: data, onGoBack: (actionNavigation: string) => {
                    if (actionNavigation === constants.actionNavigation.reload) {
                        setIsLoadInternal(true);
                        setAccounts([]);
                    }
                }
            });
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
                        let response = await alterAccount(data);
                        validateLogin(response, navigation);

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
                        let response = await excludeAccount(data.Id, data.InternalId);
                        validateLogin(response, navigation);

                        if (response.success) {
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
        navigation.navigate("AccountCreate", {
            isEditing: false, data: null, onGoBack: (actionNavigation: string) => {
                if (actionNavigation === constants.actionNavigation.reload) {
                    setIsLoadInternal(true);
                    setAccounts([]);
                }
            }
        });
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <View style={style.titleScreen}>
                    <AccountIcon style={{ opacity: 1}} width="24" height="24" fill="#F1F1F1"/>
                    <Text style={style.titleScreemText}>Contas</Text>
                </View>
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