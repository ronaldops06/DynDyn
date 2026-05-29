import React, {useEffect, useState} from "react";
import {validateLogin, validateSuccess} from "../../../../utils.ts";
import {CustomAlert} from "../../../../components/CustomAlert";
import {
    alterTotalizerRole,
    createTotalizerRole,
    excludeTotalizerRole,
    loadTotalizerRoleByCodeAndType
} from "../../../../controller/totalizer.role.controller.ts";
import * as I from "../../../../interfaces/interfaces.tsx";
import {SymbolNumber, SymbolString} from "../../../../interfaces/interfaces.tsx";
import {PageRegister} from "../../../../components/Page";
import {useTheme} from "../../../../contexts/ThemeContext.tsx";
import {getStyle} from "../../../../styles/styles.ts";
import {getStyleCadastro} from "../../../../styles/styles.cadastro.ts";
import {Alert, ScrollView, Text, TouchableOpacity, View} from "react-native";
import Picker from "../../../../components/CustomPicker";
import {constants} from "../../../../constants";
import RuleIcon from "../../../../assets/rule.svg";
import {getTotalizerRoleRegisterStyles} from "./styles";
import OperationRoleModal from "../../../Operation/OperationRoleModal";
import {Chip} from "react-native-paper";

const TotalizerRoleCreate = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const totalizerRoleCreateStyle = getTotalizerRoleRegisterStyles(theme);
    
    let totalizerId = route.params?.data?.Id ?? 0;
    let totalizerInternalId = route.params?.data?.InternalId ?? 0;
    let isEditing = route?.params?.isEditing ?? false;
    
    const [loading, setLoading] = useState(false);
    const [totalizerCode, setTotalizerCode] = useState<string>("");
    const [totalizerType, setTotalizerType] = useState<number>(0);
    const [showModalRole, setShowModalRole] = useState(false);
    const [operationRoles, setOperationRoles] = useState<I.OperationRole[]>([]);

    useEffect(() => {
        if (isEditing) {
            loadDataSreen();
        }
    }, []);
    
    const loadDataSreen = () => {
        const data = route.params?.data;
        
        if (data != undefined) {
            setTotalizerCode(data.Code);
            setTotalizerType(data.Type);
            setOperationRoles(data.OperationRoles);
        }
    }
    
    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleTrashClick = async () => {
        await CustomAlert("Atenção!",
            "Esta configuração será excluída. Deseja continuar?",
            async () => {
                let response = await excludeTotalizerRole(totalizerId, totalizerInternalId);
                validateLogin(response, navigation);
                validateSuccess(response, navigation, 'TotalizerRole');
            }
        );
    };

    const getTotalizerCodes = (): SymbolString[] => {
        const keys: SymbolString[] = Object.values(constants.totalizerCode);
        return keys;
    }

    const getTotalizerTypes = (): SymbolNumber[] => {
        const keys: SymbolNumber[] = Object.values(constants.totalizerType);
        return keys;
    }
    
    const validadeExistRecord = async (code, type) => {
        let result = await loadTotalizerRoleByCodeAndType(code, type);

        if (result !== undefined){
            setOperationRoles(result.OperationRoles);
            totalizerId = result.Id;
            totalizerInternalId = result.InternalId;
            isEditing = true;
        }
    }
    
    const onSelectedCode = async (value) => {
        setTotalizerCode(value);

        if (value !== "" && totalizerType !== 0)
            await validadeExistRecord(value, totalizerType);
    }

    const onSelectedType = async (value) => {
        setTotalizerType(value);

        if (totalizerCode !== "" && value !== 0)
            await validadeExistRecord(totalizerCode, value);
    }

    const validateRequiredFields = () => {

        if (totalizerCode === "" || totalizerCode === null || totalizerCode === undefined) {
            Alert.alert("Atenção!", "O código deve ser informado.");
            return false;
        }

        if (totalizerType === 0 || totalizerType === null || totalizerType === undefined) {
            Alert.alert("Atenção!", "O tipo deve ser selecionado.");
            return false;
        }

        if (operationRoles.length === 0) {
            Alert.alert("Atenção!", "Deve ser selecionado pelo menos um papel de operação.");
            return false;
        }

        return true;
    }

    const handleSaveClick = async () => {
        setLoading(true);
        
        if (!validateRequiredFields()) return;
        
        let totalizerRoleDTO = {} as I.TotalizerRole;
        totalizerRoleDTO.Id = totalizerId;
        totalizerRoleDTO.InternalId = totalizerInternalId;
        totalizerRoleDTO.Code = totalizerCode;
        totalizerRoleDTO.Type = totalizerType;
        totalizerRoleDTO.OperationRoles = operationRoles;

        let response: I.Response = {} as I.Response;
        if (isEditing)
            response = await alterTotalizerRole(totalizerRoleDTO);
        else
            response = await createTotalizerRole(totalizerRoleDTO);
        
        setLoading(false);
        validateLogin(response, navigation);
        validateSuccess(response, navigation, 'TotalizerRole');
    }

    const addOperationRole = (item: I.OperationRole) => {
        let exists = operationRoles.findIndex(x => x.InternalId === item.InternalId);

        if (exists < 0) {
            let operationsRolesAux = operationRoles;
            operationsRolesAux.push(item);
            setOperationRoles(operationsRolesAux);
        }
    }

    const removeOperationRole = (item: I.OperationRole) => {
        setOperationRoles(prev =>
            prev.filter(x => x.InternalId !== item.InternalId)
        );
    }

    return (
        <PageRegister
            onTrashClick={handleTrashClick}
            onBackClick={handleBackClick}
            onSaveClick={handleSaveClick}
            helpType={"totalizer_register"}
            isEditing={isEditing}
            isLoading={loading}>
            <View style={styleCadastro.areaFields}>
                <Picker
                    text={"Totalizador"}
                    value={totalizerCode}
                    setValue={onSelectedCode}
                    data={getTotalizerCodes()}
                />
                <Picker
                    text={"Tipo Totalizador"}
                    value={totalizerType}
                    setValue={onSelectedType}
                    data={getTotalizerTypes()}
                />
                <TouchableOpacity
                    style={totalizerRoleCreateStyle.buttonAddRole}
                    onPress={() => {
                        setShowModalRole(true)
                    }}>
                    <RuleIcon width="30" height="30" fill={theme.colors.primaryIcon}/>
                    <Text style={styleCadastro.textButtonSave}>Papéis de Operações</Text>
                </TouchableOpacity>
                <ScrollView style={totalizerRoleCreateStyle.scrollRoles} nestedScrollEnabled>
                    <View style={totalizerRoleCreateStyle.areaRoles}>
                        {operationRoles.map(item => (
                            <Chip
                                key={item.InternalId}
                                onClose={() => removeOperationRole(item)}
                                mode="flat"
                                style={{marginVertical: 4, marginLeft: 8}}
                                iconClose="close"
                            >
                                {item.Name}
                            </Chip>
                        ))
                        }
                    </View>
                </ScrollView>
            </View>
            <OperationRoleModal
                show={showModalRole}
                setShow={setShowModalRole}
                navigation={navigation}
                addOperationRole={addOperationRole}
            />
        </PageRegister>
    );
}

export default TotalizerRoleCreate;