import React, { useEffect, useState } from 'react';
import { ActivityIndicator, Modal, ScrollView, TouchableOpacity, View } from 'react-native';

import ExpandIcon from '../../../assets/expand.svg';
import * as I from '../../../interfaces/interfaces';
import TextItem from '../../../components/CustomTextInput';
import OperationItem from './OperationItem';

import { style } from '../../../styles/styles';
import { operationModalStyle } from './styles';
import {loadAllOperation} from "../../../controller/operation.controller";

interface OperationModalParams {
    show: boolean,
    setShow: any,
    setOperation: any,
    tipoOperation: number
}

const OperationModal = (props: OperationModalParams) => {

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
            <View style={operationModalStyle.areaModal}>
                <TouchableOpacity
                    style={operationModalStyle.buttonClose}
                    onPress={handleCloseClick}>
                    <ExpandIcon width="40" height="40" fill="#F1F1F1" />
                </TouchableOpacity>
                <View style={operationModalStyle.areaContent}>
                    <ScrollView>
                        <TextItem
                            text="Search"
                            value={valueSearch}
                            setValue={setValueSearch}
                            width="100%"
                        />
                        <View >
                            {loading &&
                                <ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8" />
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