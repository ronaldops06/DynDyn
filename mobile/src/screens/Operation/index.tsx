import React, {useEffect, useRef, useState} from "react";
import PlusIcon from "../../assets/plus.svg";
import {Alert, SafeAreaView, Text, TouchableOpacity, View} from "react-native";
import _ from 'lodash';
import * as I from "../../interfaces/interfaces.tsx";
import {
    alterOperation,
    excludeOperation,
    loadAllOperation,
    loadAllOperationInternal
} from "../../controller/operation.controller.tsx";
import CustomScroll from "../../components/CustomScroll";
import {constants} from "../../constants";
import CarouselSelection from "../../components/CarouselSelection";
import OperationItem from "./OperationItem";
import {validateLogin} from "../../utils.ts";

import {style} from "../../styles/styles.ts";
import {categoryStyle} from "../Category/styles";
import HistoryIcon from '../../assets/history.svg';

const Operation = ({navigation}) => {

    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [operations, setOperations] = useState<I.Operation[]>([]);
    const [operationType, setOperationType] = useState<number>(constants.operationType.revenue.Id);

    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        if (operations.length === 0) {
            setPageNumber(1);
            loadOperations();
        }
    }, [operations]);

    useEffect(() => {
        if (operations.length !== 0) {
            setIsLoadInternal(true);
            loadOperations();
        }
    }, [pageNumber]);

    useEffect(() => {

        updateOperations();

        return () => updateOperations.cancel();
    }, [operationType]);

    /*Se clicar várias vezes na troca de tipos essa lógica faz com que não seja efetuado a busca em todas as trocas, 
    o "debounce" faz com que aguarde para executar a função e se for chamada novamente enquanto o tempo não acabou
    cancela a chamada anterior e começa a aguardar novamente.*/
    const updateOperations = _.debounce(() => {
        setOperations([]);
    }, 500);

    const appendOperations = (data: I.Operation[]) => {
        let operationsNew = operations;
        if (data.length > 0) {
            data.map((item, key) => {
                operationsNew.push(item);
            });
            setOperations(operationsNew);
        }
    };

    const loadOperations = async () => {
        setLoading(true);

        let responseOperations = null;

        if (isLoadInternal) {
            responseOperations = await loadAllOperationInternal(operationType, pageNumber);
        } else {
            responseOperations = await loadAllOperation(operationType, pageNumber);
            validateLogin(responseOperations, navigation);
        }

        setTotalPages(responseOperations?.totalPages ?? 1);
        appendOperations(responseOperations?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleNewClick = () => {
        navigation.navigate("OperationCreate", {
            isEditing: false, data: null,
            onGoBack: (actionNavigation: string) => {
                if (actionNavigation === constants.actionNavigation.reload) {
                    setIsLoadInternal(true);
                    setOperations([]);
                }
            }
        });
    }

    const handleItemClick = (data: I.Operation) => {
        if (!isScrolling)
            navigation.navigate("OperationCreate", {
                isEditing: true, data: data, onGoBack: (actionNavigation: string) => {
                    if (actionNavigation === constants.actionNavigation.reload) {
                        setIsLoadInternal(true);
                        setOperations([]);
                    }
                }
            });
    }

    const onSwipeLeft = (data: I.Operation) => {
        Alert.alert("Atenção!",
            "Esta operação terá o status alterado. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        data.Status = (data.Status === constants.status.active.Id) ? constants.status.inactive.Id : constants.status.active.Id;
                        let response = await alterOperation(data);
                        validateLogin(response, navigation);

                        setOperations((prevOperations) =>
                            prevOperations.map((item) =>
                                item.Id === data.Id ? data : item
                            )
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const onSwipeRight = (data: I.Operation) => {
        Alert.alert("Atenção!",
            "Esta operação será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeOperation(data.Id, data.InternalId);
                        validateLogin(response, navigation);

                        if (response.success) {
                            setIsLoadInternal(true);
                            setOperations([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <View style={style.titleScreen}>
                    <View style={style.titleScreenTitle}>
                        <HistoryIcon style={{opacity: 1}} width="24" height="24" fill="#F1F1F1"/>
                        <Text style={style.titleScreemText}>Operações</Text>
                    </View>
                </View>
                <CarouselSelection data={constants.operationType} handleItemSelectedId={setOperationType}/>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    data={operations}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    renderItem={({ item }) => (
                        <OperationItem
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                    )}
                />
                <TouchableOpacity
                    style={categoryStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill="#6E8BB8"/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Operation;