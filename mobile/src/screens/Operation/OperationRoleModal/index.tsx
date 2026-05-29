import React, {useEffect, useState} from "react";
import {Alert, TextInput, TouchableOpacity, View} from "react-native";
import BottomModal from "../../../components/BottomModal";
import OperationRoleItem from "./OperationRoleItem";
import CustomScroll from "../../../components/CustomScroll";
import * as I from "../../../interfaces/interfaces.tsx";
import {
    alterOperationRole,
    createOperationRole,
    excludeOperationRole,
    loadAllOperationRoleInternal
} from "../../../controller/operation.role.controller.ts";
import {validateLogin} from "../../../utils.ts";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getOperationRoleModalStyle} from "./styles";
import PlusIcon from "../../../assets/plus.svg"
import DoneIcon from "../../../assets/done.svg";
import {CustomAlert} from "../../../components/CustomAlert";

interface OperationRoleProps {
    show: boolean;
    setShow: (value: boolean) => void;
    navigation: any;
    addOperationRole: (item: I.OperationRole) => void;
}

const OperationRoleModal = (props: OperationRoleProps) => {
    const {theme} = useTheme();
    const operationRoleModal = getOperationRoleModalStyle(theme);

    const [loading, setLoading] = useState(false);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [operationsRoles, setOperationsRoles] = useState<I.OperationRole[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isEditing, setIsEditing] = useState(false);
    const [name, setName] = useState<string>("");
    const [operationRoleId, setOperationRoleId] = useState(0);
    const [operationRoleInternalId, setOperationRoleInternalId] = useState(0);

    useEffect(() => {
        clearEditing();
        setPageNumber(1);
        loadOperationsRoles();
    }, [props.show]);

    useEffect(() => {
        if (operationsRoles.length === 0) {
            setPageNumber(1);
            loadOperationsRoles();
        }
    }, [operationsRoles]);

    const loadOperationsRoles = async () => {
        setLoading(true);

        let response = await loadAllOperationRoleInternal(pageNumber);
        setOperationsRoles(response?.data ?? []);
        setLoading(false);
    };

    const handleItemClick = (item: I.OperationRole) => {
        if (!isScrolling) {
            props.addOperationRole(item);
            props.setShow(false);
        }
    }

    const validateRequiredFields = () => {

        if (name === "" || name === null || name === undefined) {
            Alert.alert("Atenção!", "O nome deve ser informado.");
            return false;
        }

        return true;
    }

    const handleAddClick = async () => {

        if (!validateRequiredFields()) return;

        setLoading(true);

        let operationRoleDTO = {} as I.OperationRole;
        operationRoleDTO.Id = operationRoleId;
        operationRoleDTO.InternalId = operationRoleInternalId;
        operationRoleDTO.Name = name;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterOperationRole(operationRoleDTO);
        else
            response = await createOperationRole(operationRoleDTO);

        setLoading(false);

        validateLogin(response, props.navigation);

        clearEditing();

        if (response.data !== null) {
            if (isEditing){
                setOperationsRoles((prevOperationsRoles) =>
                    prevOperationsRoles.map((item) =>
                        item.Id === response.data.Id ? response.data : item
                    )
                );
            } else {
                let operationsRolesAux = operationsRoles;
                operationsRolesAux.push(response.data);
                setOperationsRoles(operationsRolesAux);
            }
        }
    };

    const onSwipeLeft = async (item: I.OperationRole) => {
        await CustomAlert("Atenção!",
            "Este papel de operação será excluído. Deseja continuar?",
            async () => {
                let response = await excludeOperationRole(item.Id, item.InternalId);
                validateLogin(response, props.navigation);
                
                if (response.success) {
                    setPageNumber(1);
                    setOperationsRoles([]);
                }
            }
        );
    }

    const onSwipeRight = (data: I.OperationRole) => {
        setIsEditing(true);
        setOperationRoleInternalId(data.InternalId);
        setOperationRoleId(data.Id);
        setName(data.Name);
    }
    
    const clearEditing = () => {
        setIsEditing(false);
        setOperationRoleInternalId(0);
        setOperationRoleId(0);
        setName("");
    }

    return (
        <BottomModal show={props.show} setShow={props.setShow}>
            <View style={operationRoleModal.areaContent}>
                <CustomScroll
                    data={operationsRoles}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    renderItem={({item}) => (
                        <OperationRoleItem
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}
                            isEditing={operationRoleInternalId === item.InternalId}
                        />
                    )}
                />
            </View>
            <View style={operationRoleModal.areaFooter}>
                <TextInput
                    style={operationRoleModal.inputName}
                    autoCapitalize={"none"}
                    onChangeText={setName}
                    value={name}
                    placeholder="Informe o Nome"
                />
                <TouchableOpacity
                    style={operationRoleModal.buttonAdd}
                    onPress={handleAddClick}>
                    {isEditing ?
                        <DoneIcon width="30" height="30" fill={theme.colors.quaternaryIcon}/> :
                        <PlusIcon width="30" height="30" fill={theme.colors.quaternaryIcon}/>
                    }
                </TouchableOpacity>
            </View>
        </BottomModal>
    );
}

export default OperationRoleModal;