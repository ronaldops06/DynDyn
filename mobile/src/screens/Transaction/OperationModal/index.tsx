import React, { useEffect, useState } from 'react';
import { ActivityIndicator, Modal, ScrollView, TouchableOpacity, View } from 'react-native';

import ExpandIcon from '../../../assets/expand.svg';
import * as I from '../../../interfaces/interfaces';
import TextItem from '../../../components/CustomTextInput';
import OperationItem from './OperationItem';

import {loadAllOperation} from "../../../controller/operation.controller";

import { useTheme } from '../../../contexts/ThemeContext';
import { getStyle } from '../../../styles/styles';

interface OperationModalParams {
    show: boolean,
    setShow: any,
    setOperation: any,
    tipoOperation: number
}

const OperationModal = (props: OperationModalParams) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    
    const [loading, setLoading] = useState(false);
    const [valueSearch, setValueSearch] = useState("");
    const [operations, setOperations] = useState<I.Operation[]>([]);

    const loadOperations = async () => {
        setLoading(true);

        let response = await loadAllOperation(props.tipoOperation, null);
        setOperations(response?.data ?? []);
        setLoading(false);
    };

    useEffect(() => {
        loadOperations();
    }, [props.show == true]);

    const handleCloseClick = () => {
        setOperations([]);
        props.setShow(false);
    };

    const handleItemClick = (item: I.Operation) => {
        props.setOperation(item);
        setOperations([]);
        props.setShow(false);
    };

    return (
        <Modal
            transparent={true}
            visible={props.show}
            animationType="slide">
            <View style={style.areaModal}>
                <TouchableOpacity
                    style={style.buttonCloseModal}
                    onPress={handleCloseClick}>
                    <ExpandIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                </TouchableOpacity>
                <View style={style.areaContentModal}>
                    <ScrollView>
                        <TextItem
                            text="Search"
                            value={valueSearch}
                            setValue={setValueSearch}
                            width="100%"
                        />
                        <View >
                            {loading &&
                                <ActivityIndicator style={style.loadingIcon} size="large" color={theme.colors.primaryBaseColor} />
                            }
                            {operations != null && operations.map((item, key) => {
                                return ( item.Name.toUpperCase().includes(valueSearch.toUpperCase()) &&
                                    <OperationItem
                                        key={key}
                                        data={item}
                                        onPress={handleItemClick}/>
                                    )
                            })
                            }
                        </View>
                    </ScrollView>
                </View>
            </View>
        </Modal>
    );
};

export default OperationModal;