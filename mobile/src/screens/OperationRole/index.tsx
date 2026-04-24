import React, {useEffect, useRef, useState} from "react";
import {useFocusEffect} from "@react-navigation/native";
import RuleIcon from "../../assets/rule.svg";
import PrevIcon from "../../assets/nav_prev.svg";
import Filter from "../OperationRole/Filter";
import {hasAnyFilter, validateLogin} from "../../utils.ts";
import _ from 'lodash';

import * as I from "../../interfaces/interfaces.tsx";
import {Alert, TouchableOpacity} from "react-native";
import {
    excludeOperationRole,
    loadAllOperationRole,
    loadAllOperationRoleInternal
} from "../../controller/operation.role.controller.ts";

import {useTheme} from '../../contexts/ThemeContext.tsx'
import {constants as pageConstants} from "../../components/Page/constants";
import {PageProcess} from "../../components/Page";
import CustomScroll from "../../components/CustomScroll";
import OperationRoleItem from "./OperationRoleItem";
import {constants} from "../../constants";
import {getStyle} from "../../styles/styles.ts";

const OperationRole = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);

    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [filter, setFilter] = useState<I.OperationRoleFilter>({} as I.OperationRoleFilter);
    const [operationsRoles, setOperationsRoles] = useState<I.OperationRole[]>([]);

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setOperationsRoles([]);
            }
        }, [route.params?.actionNavigation])
    );
    
    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
            return;
        }

        if (operationsRoles.length === 0) {
            setPageNumber(1);
            loadOperationsRoles(1);
        }
    }, [operationsRoles]);

    useEffect(() => {
        if (operationsRoles.length !== 0) {
            setIsLoadInternal(true);
            loadOperationsRoles(pageNumber);
        }
    }, [pageNumber]);
    
    /*Se clicar várias vezes na troca de tipos essa lógica faz com que não seja efetuado a busca em todas as trocas, 
    o "debounce" faz com que aguarde para executar a função e se for chamada novamente enquanto o tempo não acabou
    cancela a chamada anterior e começa a aguardar novamente.*/
    const updateOperationsRoles = _.debounce(() => {
        setOperationsRoles([]);
    }, 500);
    
    const appendOperations = (data: I.OperationRole[]) => {
        let operationsRolesNew = operationsRoles;
        if (data.length > 0) {
            data.map((item, key) => {
                operationsRolesNew.push(item);
            });
            setOperationsRoles(operationsRolesNew);
        }
    };
    
    const loadOperationsRoles = async (page: number) => {
        setLoading(true);

        let responseOperationsRoles = null;

        if (isLoadInternal) {
            responseOperationsRoles = await loadAllOperationRoleInternal(page, null);
        } else {
            responseOperationsRoles = await loadAllOperationRole(page, null);
            validateLogin(responseOperationsRoles, navigation);
        }

        setTotalPages(responseOperationsRoles?.totalPages ?? 1);
        appendOperations(responseOperationsRoles?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };
    
    const handleNewClick = () => {
        navigation.navigate("OperationRoleCreate", {
            params: { isEditing: false, data: null }
        });
    }
    
    const handleItemClick = (data: I.OperationRole) => {
        if (!isScrolling)
            navigation.navigate("OperationRoleCreate", {
                params: { isEditing: true, data: data }
            });
    }

    const onSwipeLeft = (data: I.OperationRole) => {
        
    }

    const onSwipeRight = (data: I.OperationRole) => {
        Alert.alert("Atenção!",
            "Este papel de operação será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeOperationRole(data.Id, data.InternalId);
                        validateLogin(response, navigation);

                        if (response.success) {
                            setIsLoadInternal(true);
                            setOperationsRoles([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const filterData = (operationsRoles: I.OperationRole[]): I.OperationRole[] => {
        let result = operationsRoles;

        if (filter.Search && filter.Search !== "") {
            result = result.filter(item => {
                return item.Name.toLowerCase().includes(filter.Search);
            })
        }

        return result;
    };
    
    return(
        <PageProcess
            headerType={pageConstants.headerType.processReduced}
            bodyType={pageConstants.bodyType.processLarger}
            title={"Papeis de Operações"}
            helpType={"operation_role"}
            iconTitle={<RuleIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>}
            onNewClick={handleNewClick}
            renderFilters={(closeModal) => (
                <Filter filter={filter} setFilter={setFilter} onClose={closeModal}/>
            )}
            filterActivated={hasAnyFilter(filter)}
            headerContent={<TouchableOpacity
                style={style.buttonBack}
                onPress={() => {navigation.goBack()}}>
                <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
            </TouchableOpacity> }
        >
            <CustomScroll
                data={filterData(operationsRoles)}
                loading={loading}
                totalPages={totalPages}
                pageNumber={pageNumber}
                handlePageNumber={setPageNumber}
                handleScrolling={setIsScrolling}
                renderItem={({ item }) => (
                    <OperationRoleItem
                        data={item}
                        onPress={handleItemClick}
                        onSwipeLeft={onSwipeLeft}
                        onSwipeRight={onSwipeRight}/>
                )}
            />
        </PageProcess>
    );
}

export default OperationRole;