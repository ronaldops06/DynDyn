import React, {useEffect, useState} from "react";
import {Alert, View} from "react-native";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {validateLogin, validateSuccess} from "../../../utils.ts";
import {
    alterOperationRole,
    createOperationRole,
    excludeOperationRole
} from "../../../controller/operation.role.controller.ts";
import {PageRegister} from "../../../components/Page";
import * as I from "../../../interfaces/interfaces.tsx";
import TextInput from "../../../components/CustomTextInput";
import {getOperationRoleCreateStyle} from "./create.styles";

const OperationRoleCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const styleOperationRole = getOperationRoleCreateStyle(theme);

    const operationRoleId = route.params?.data?.Id ?? 0;
    const operationRoleInternalId = route.params?.data?.InternalId ?? 0;
    const isEditing = route.params?.isEditing ?? false;

    const [loading, setLoading] = useState(false);
    const [name, setName] = useState<string>("");

    useEffect(() => {
        if (isEditing) {
            loadDataSreen();
        }
    }, [])
    
    const loadDataSreen = () => {
        const data = route.params?.data;
        if (data != undefined) {
            setName(data.Name);
        }
    };
    
    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        Alert.alert("Atenção!",
            "Este papel de operação será excluído. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeOperationRole(operationRoleId, operationRoleInternalId);
                        validateLogin(response, navigation);
                        validateSuccess(response, navigation, 'OperationRole');
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

        return true;
    }

    const handleSaveClick = async () => {

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

        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'OperationRole');
    };
    
    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"operation_role_register"}
            isEditing={isEditing}
            isLoading={loading}>
            <View style={styleOperationRole.areaFields}>
                <TextInput
                    text={"Nome"}
                    isMoveText={false}
                    value={name}
                    setValue={setName}
                />
            </View>
        </PageRegister>
    );
}

export default OperationRoleCreate;