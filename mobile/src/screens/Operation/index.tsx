import {style} from "../../styles/styles.ts";
import {categoryStyle} from "../Category/styles";
import PlusIcon from "../../assets/plus.svg";
import {SafeAreaView, TouchableOpacity, View} from "react-native";
import React, {useEffect, useState} from "react";
import {useNavigation} from "@react-navigation/core";
import _ from 'lodash';

import {RootStackParamList} from '../RootStackParams';
import {StackNavigationProp} from "@react-navigation/stack";
import * as I from "../../interfaces/interfaces.tsx";
import {loadAllOperation, loadAllOperationInternal} from "../../controller/operation.controller.tsx";
import CustomScroll from "../../components/CustomScroll";
import {CustomAlert} from "../../components/CustomAlert";
import {constants} from "../../constants";
import CarouselSelection from "../../components/CarouselSelection";
import OperationItem from "./OperationItem";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Operation'>;

const Operation = () => {
    const navigation = useNavigation<homeScreenProp>();

    const [loading, setLoading] = useState(false);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [operations, setOperations] = useState<I.Operation[]>([]);
    const [operationType, setOperationType] = useState<number>(1);

    useEffect(() => {
        // Executado no goBack da tela seguinte
        navigation.addListener('focus', () => {
            setIsLoadInternal(true);
            setOperations([]);
        });
    }, [navigation]);

    useEffect(() => {
        if (operations.length === 0) {
            setPageNumber(1);
            loadOperations();
        }
    }, [operations]);

    useEffect(() => {
        setIsLoadInternal(true);
        loadOperations();
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

        if (isLoadInternal)
            responseOperations = await loadAllOperationInternal(operationType, pageNumber);
        else
            responseOperations = await loadAllOperation(operationType, pageNumber, navigation);

        setTotalPages(responseOperations?.totalPages ?? 1);
        appendOperations(responseOperations?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleNewClick = () => {
        navigation.navigate("OperationCreate");
    }

    const handleItemClick = (data: I.Operation) => {
        if (!isScrolling)
            navigation.navigate("OperationCreate", {isEditing: true, data: data});
    }

    const onSwipeLeft = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta operação terá o status alterado. Deseja continuar?", console.log("implementar"));
    }

    const onSwipeRight = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta operação será excluída. Deseja continuar?", console.log("implementar"));
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <CarouselSelection data={constants.operationType} handleItemSelectedId={setOperationType}/>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                >
                    {operations != null && operations.map((item, key) => (
                        <OperationItem
                            key={key}
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                    ))
                    }
                </CustomScroll>
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