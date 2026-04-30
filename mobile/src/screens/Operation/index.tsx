import React, {useEffect, useRef, useState} from "react";
import {Alert} from "react-native";
import {useFocusEffect} from "@react-navigation/native";
import _ from 'lodash';
import * as I from "../../interfaces/interfaces.tsx";
import HistoryIcon from '../../assets/history.svg';
import {
    alterOperation,
    excludeOperation,
    loadAllOperation,
    loadAllOperationInternal
} from "../../controller/operation.controller.tsx";
import CustomScroll from "../../components/CustomScroll";
import {constants} from "../../constants";
import CarouselSelection from "../../components/CarouselSelection";
import {hasAnyFilter, validateLogin} from "../../utils.ts";

import {PageProcess} from "../../components/Page";
import {Situation} from "../../enums/enums.tsx";
import {constants as pageConstants} from "../../components/Page/constants";

import Filter from "../Operation/Filter";
import OperationItem from "./OperationItem";
import {useTheme} from '../../contexts/ThemeContext';

const Operation = ({navigation, route}) => {
    const { theme } = useTheme();
      
    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [filter, setFilter] = useState<I.OperationFilter>({} as I.OperationFilter);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [operations, setOperations] = useState<I.Operation[]>([]);
    const [operationType, setOperationType] = useState<number>(constants.operationType.revenue.Id);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    
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
        } else {
            if (operations.length === 0) {
                setPageNumber(1);
                loadOperations(1);
            }
        }
    }, [operations]);

    useEffect(() => {
        if (operations?.length !== 0) {
            setIsLoadInternal(true);
            loadOperations(pageNumber);
        }
    }, [pageNumber]);

    useEffect(() => {
        setPageNumber(1);
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

        let responseOperations = await loadAllOperationInternal(operationType, page, null);

        /*if (isLoadInternal) {
            responseOperations = await loadAllOperationInternal(operationType, page, null);
        } else {
            responseOperations = await loadAllOperation(operationType, page, null);
            validateLogin(responseOperations, navigation);
        }*/

        setTotalPages(responseOperations?.totalPages ?? 1);
        appendOperations(responseOperations?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const getOperationType = () => {
        let operationType = {...constants.operationType};
        delete operationType.transfer;
        
        return operationType;
    }
    
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

    const filterData = (operations: I.Operation[]): I.Operation[] => {
        let result = operations;

        if (filter.Situation != undefined && filter.Situation !== Situation.All) {
            result = result.filter(item => {
                return filter.Situation !== Situation.All ? item.Status === filter.Situation: item;
            });
        }

        if (filter.CategoryId) {
            result = result.filter(item => {
                return filter.CategoryId !== 0 ? item.Category.Id === filter.CategoryId : item;
            })
        }

        if (filter.Salary != undefined && filter.Salary !== Situation.All) {
            result = result.filter(item => {
                return filter.Salary !== Situation.All ? item.Salary === (filter.Salary === Situation.Consolidated) : item;
            });
        }

        if (filter.Recurrent != undefined && filter.Recurrent !== Situation.All) {
            result = result.filter(item => {
                return filter.Recurrent !== Situation.All ? item.Recurrent === (filter.Recurrent === Situation.Consolidated) : item;
            });
        }

        if (filter.Search && filter.Search !== "") {
            result = result.filter(item => {
                return item.Name.toLowerCase().includes(filter.Search);
            })
        }
        
        return result;
    };

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
                    data={getOperationType()}
                    handleItemSelectedId={setOperationType}/>
            }
            renderFilters={(closeModal) => (
                <Filter filter={filter} setFilter={setFilter} onClose={closeModal}/>
            )}
            filterActivated={hasAnyFilter(filter)}
        >
            <CustomScroll
                data={filterData(operations)}
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