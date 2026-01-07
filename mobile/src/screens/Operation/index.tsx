import React, {useEffect, useRef, useState} from "react";
import {Alert} from "react-native";
import {useFocusEffect} from "@react-navigation/native";
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

import {useTheme} from '../../contexts/ThemeContext';

import HistoryIcon from '../../assets/history.svg';
import {constants as pageConstants} from "../../components/Page/constants";
import {PageProcess} from "../../components/Page";

const Operation = ({navigation, route}) => {
    const { theme } = useTheme();
      
    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [operations, setOperations] = useState<I.Operation[]>([]);
    const [operationType, setOperationType] = useState<number>(constants.operationType.revenue.Id);
    
    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setOperations([]);
            }
        }, [route.params?.actionNavigation])
    );
    
    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        if (operations.length === 0) {
            setPageNumber(1);
            loadOperations(1);
        }
    }, [operations]);

    useEffect(() => {
        if (operations.length !== 0) {
            setIsLoadInternal(true);
            loadOperations(pageNumber);
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

    const loadOperations = async (page: number) => {
        setLoading(true);

        let responseOperations = null;

        if (isLoadInternal) {
            responseOperations = await loadAllOperationInternal(operationType, page, null);
        } else {
            responseOperations = await loadAllOperation(operationType, page, null);
            validateLogin(responseOperations, navigation);
        }

        setTotalPages(responseOperations?.totalPages ?? 1);
        appendOperations(responseOperations?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleNewClick = () => {
        navigation.navigate("Operation", {
            screen: 'OperationCreate',
            params: { isEditing: false, data: null }
        });
    }

    const handleItemClick = (data: I.Operation) => {
        if (!isScrolling)
            navigation.navigate("Operation", {
                screen: 'OperationCreate',
                params: { isEditing: true, data: data }
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
        <PageProcess
            headerType={pageConstants.headerType.processReduced}
            bodyType={pageConstants.bodyType.processLarger}
            title={"Operações"}
            helpType={"operation"}
            iconTitle={<HistoryIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>}
            onNewClick={handleNewClick}
            headerContent={
                <CarouselSelection
                    disabled={loading}
                    data={constants.operationType}
                    handleItemSelectedId={setOperationType}/>
            }>
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
        </PageProcess>
    );
}

export default Operation;