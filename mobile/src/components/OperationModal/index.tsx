import React, { useState, useEffect } from 'react';
import { Modal, TouchableOpacity, ActivityIndicator, View, ScrollView } from 'react-native';

import TextItem from '../CustomTextInput';
import OperationItem from '../OperationItem';
import ExpandIcon from '../../assets/expand.svg';
import * as I from '../../interfaces/interfaces';
import { getOperations } from './operation.modal.api';

import { style } from '../../styles/styles';
import { operationModalStyle } from './styles';

interface OperationModalParams {
    show: boolean,
    setShow: any,
    setOperation: any,
    tipoOperation: number
}

const OperationModal = (props: OperationModalParams) => {

    const [ loading, setLoading ] = useState(false);
    const [ valueSearch, setValueSearch ] = useState("");
    const [ operations, setOperations ] = useState<I.Operacao[]>([]);

    const loadOperations = async () => {
        setLoading(true);

        let response = await getOperations(`Tipo=${props.tipoOperation}`);
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

    const handleItemClick = (item: I.Operacao) => {
        props.setOperation(item);
        setOperations([]);
        props.setShow(false);
    };

    return(
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
                            {operations != null && operations.map((item, key) => (
                                <OperationItem 
                                    key={key}
                                    data={item}
                                    onPress={handleItemClick}/>
                            ))
                            }
                        </View>
                    </ScrollView>
                </View>
            </View>
        </Modal>
    );
};

export default OperationModal;