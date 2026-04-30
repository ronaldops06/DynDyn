import React, {useEffect, useRef, useState} from "react";
import {ActivityIndicator, SafeAreaView, ScrollView, Text, TouchableOpacity, View} from 'react-native'
import {useFocusEffect} from "@react-navigation/native";

import PrevIcon from "../../../assets/nav_prev.svg";
import * as I from "../../../interfaces/interfaces.tsx";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getStyle} from "../../../styles/styles.ts";
import {getTotalizerRoleStyles} from "./styles";
import {constants} from "../../../constants";
import {loadAllTotalizerRole} from "../../../controller/totalizer.role.controller.ts";
import AddIcon from "../../../assets/plus.svg";
import {Chip} from "react-native-paper";

const TotalizerRole = ({navigation, route}) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const totalizerRoleStyle = getTotalizerRoleStyles(theme);

    const isFirstRender = useRef(true);
    const [loading, setLoading] = useState(false);
    const [totalizerRole, setTotalizerRole] = useState<I.TotalizerRole[]>([]);

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setTotalizerRole([]);
            }
        }, [route.params?.actionNavigation])
    );

    useEffect(() => {
        if (totalizerRole.length === 0) {
            loadData();
        }
    }, [totalizerRole]);

    const loadData = async () => {
        setLoading(true);
        let result = await loadAllTotalizerRole(null);
        setTotalizerRole(result.data);
        setLoading(false);
    }

    const handleBackClick = () => {
        navigation.goBack();
    };

    const getTotalizerCodes = (): I.SymbolString[] => {
        const keys: I.SymbolString[] = Object.values(constants.totalizerCode);
        return keys;
    }

    const getTotalizerTypes = (): I.SymbolNumber[] => {
        const keys: I.SymbolNumber[] = Object.values(constants.totalizerType);
        return keys;
    }

    const handleNewClick = () => {
        navigation.navigate("TotalizerRoleCreate", {
            isEditing: false, data: null
        });
    }

    const handleItemClick = (data: I.TotalizerRole) => {
        navigation.navigate("TotalizerRoleCreate", {
            isEditing: true, data: data
        });
    }

    const _renderTypes = (totalizerKey: string, totalizerType: I.TotalizerRole) => {
        
        return (
            <View key={`v3.${totalizerKey}`} onTouchEndCapture={() => handleItemClick(totalizerType)}>
                <Text key={`t2.${totalizerKey}`}
                      style={style.textPrimary16}>{getTotalizerTypes().find(x => x.Id === totalizerType.Type).Name}</Text>
                <ScrollView key={`s1.${totalizerKey}`} style={totalizerRoleStyle.scrollRoles} nestedScrollEnabled>
                    <View key={`v4.${totalizerKey}`} style={totalizerRoleStyle.areaRoles}>
                        {totalizerType.OperationRoles.map((itemOperation, key) => (
                            <Chip
                                key={`${totalizerKey}_${key}`}
                                mode="flat"
                                style={{marginVertical: 4, marginLeft: 8}}
                                iconClose="close"
                            >
                                {itemOperation.Name}
                            </Chip>
                        ))
                        }
                    </View>
                </ScrollView>
            </View>
        );
    }

    const _renderTotalizer = (totalizerKey: number, totalizer: I.SymbolString) => {
        if (!totalizerRole) return;
        
        return (
            <View key={`v1.${totalizerKey}`} style={totalizerRoleStyle.card} >
                <Text key={`t1.${totalizerKey}`} style={style.textPrimary18}>{totalizer.Name}</Text>

                <View key={`v2.${totalizerKey}`} style={totalizerRoleStyle.areaTypes}>
                    {totalizerRole.filter(x => x.Code === totalizer.Id).length > 0 ?
                        totalizerRole.filter(x => x.Code === totalizer.Id).map((item, key) => (
                            _renderTypes(`${totalizerKey}_${key}`, item)
                        )) :
                        <Text key={`t3.${totalizerKey}`} style={style.textSecondary16}>Nenhuma regra aplicada para este
                            totalizador</Text>
                    }
                </View>
            </View>
        );
    }

    return (
        <SafeAreaView style={totalizerRoleStyle.container}>
            <View style={totalizerRoleStyle.viewBody}>
                <View style={totalizerRoleStyle.titleScreen}>
                    <TouchableOpacity
                        style={totalizerRoleStyle.buttonBack}
                        onPress={handleBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIconDashboard}/>
                    </TouchableOpacity>
                    <View style={style.titleScreenTitle}>
                        <Text style={style.textPrimary24Bold}>Regras de Totalizadores</Text>
                    </View>
                </View>
                <View style={totalizerRoleStyle.areaAdd}>
                    <TouchableOpacity
                        style={totalizerRoleStyle.buttonAdd}
                        onPress={() => handleNewClick()}>
                        <AddIcon width="30" height="30" fill={theme.colors.quaternaryIcon}/>
                        <Text style={style.textPrimary16}>Adicionar Regra</Text>
                    </TouchableOpacity>
                </View>
                <ScrollView style={style.scrollCadastro}>
                    {loading ? 
                        (<ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8"/>) :
                        getTotalizerCodes().map((item, key) => (
                            _renderTotalizer(key, item)
                        ))
                    }
                </ScrollView>
            </View>
        </SafeAreaView>
    );
}

export default TotalizerRole;