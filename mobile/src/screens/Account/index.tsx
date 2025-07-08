import React, {useEffect, useRef, useState} from 'react';
import {Alert, SafeAreaView, Text, TouchableOpacity, View} from 'react-native';

import * as I from "../../interfaces/interfaces.tsx";
import {
    alterPortfolio,
    excludePortfolio,
    loadAllPortfolio,
    loadAllPortfolioInternal
} from "../../controller/portfolio.controller.tsx";
import {loadAllBalance} from "../../controller/balance.controller.tsx";
import CustomScroll from "../../components/CustomScroll";
import PlusIcon from "../../assets/plus.svg";
import AccountItem from "./AccountItem";
import {constants} from "../../constants";
import {validateLogin} from '../../utils.ts';
import AccountIcon from '../../assets/account.svg';

import { useTheme } from '../../contexts/ThemeContext';
import {getStyle} from "../../styles/styles.ts";
import {getAccountStyle} from './styles';

const Portfolio = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const accountStyle = getAccountStyle(theme);
    
    const [loading, setLoading] = useState(true);
    const isFirstRender = useRef(true);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [portfolios, setPortfolios] = useState<I.Portfolio[]>([]);
    const [executado, setExecutado] = useState(false);

    /*useFocusEffect(
        useCallback(() => {
            
            if (!executado) {
                setPageNumber(1);
                setPortfolios([]);
                loadPortfolios();
                setExecutado(true);
            }
        }, [])
    );*/

    useEffect(() => {
        if (route.params?.actionNavigation === constants.actionNavigation.reload) {
            setIsLoadInternal(true);
            setPortfolios([]);
        }
    }, [route.params?.actionNavigation]);

    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        if (portfolios.length === 0) {
            setPageNumber(1);
            loadPortfolios();
        }
    }, [portfolios]);

    useEffect(() => {
        setIsLoadInternal(true);
        loadPortfolios();
    }, [pageNumber]);

    const appendPortfolios = (data: I.Portfolio[]) => {
        let portfoliosNew = portfolios;
        if (data.length > 0) {
            data.map((item, key) => {
                portfoliosNew.push(item);
            });
            setPortfolios(portfoliosNew);
        }
    };

    const loadPortfolios = async () => {
        setLoading(true);

        let responsePortfolios = null;

        if (isLoadInternal) {
            responsePortfolios = await loadAllPortfolioInternal(pageNumber);
        } else {
            responsePortfolios = await loadAllPortfolio(pageNumber);
            validateLogin(responsePortfolios, navigation);
            
            let response = await loadAllBalance(null);
            //Carrega as contas novamente para pegar os saldos atualizados, na primeira página
            responsePortfolios = await loadAllPortfolioInternal(pageNumber);
        }

        setTotalPages(responsePortfolios?.totalPages ?? 1);
        appendPortfolios(responsePortfolios?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Portfolio) => {
        if (!isScrolling)
            navigation.navigate("AccountCreate", {
                isEditing: true, data: data, onGoBack: (actionNavigation: string) => {
                    if (actionNavigation === constants.actionNavigation.reload) {
                        setIsLoadInternal(true);
                        setPortfolios([]);
                    }
                }
            });
    }

    const onSwipeLeft = async (data: I.Portfolio) => {

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
                        let response = await alterPortfolio(data);
                        validateLogin(response, navigation);

                        setPortfolios((prevPortfolios) =>
                            prevPortfolios.map((item) =>
                                item.Id === data.Id ? data : item
                            )
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const onSwipeRight = (data: I.Portfolio) => {
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
                        let response = await excludePortfolio(data.Id, data.InternalId);
                        validateLogin(response, navigation);

                        if (response.success) {
                            setIsLoadInternal(true);
                            setPortfolios([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const handleNewClick = () => {
        navigation.navigate("AccountCreate", {
            isEditing: false, data: null
        });
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <View style={style.titleScreen}>
                    <View style={style.titleScreenTitle}>
                        <AccountIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>
                        <Text style={style.titleScreemText}>Contas</Text>
                    </View>
                </View>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    data={portfolios}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    styles={accountStyle.scroll}
                    renderItem={({item}) => (
                        <AccountItem
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                    )}
                />
                <TouchableOpacity
                    style={accountStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill={theme.colors.primaryBaseColor}/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Portfolio;