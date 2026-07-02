import {Modal, TouchableOpacity, View} from "react-native";
import ExpandIcon from "../../assets/expand.svg";
import React, {ReactNode} from "react";

import {useTheme} from '../../contexts/ThemeContext';
import {getBottomModalStyle} from "./styles";
import {getStyle} from "../../styles/styles.ts";

interface ModalProps {
    show: boolean;
    setShow: any;
    children: ReactNode;
}

const BottomModal = ({show, setShow, children}: ModalProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const bottomModalStyle = getBottomModalStyle(theme);

    const handleCloseClick = () => {
        setShow(false);
    };

    return (
        <Modal
            transparent={true}
            visible={show}
            animationType="slide">
            <View style={bottomModalStyle.areaModal}>
                <TouchableOpacity
                    style={bottomModalStyle.buttonClose}
                    onPress={handleCloseClick}>
                    <ExpandIcon width="40" height="40" fill="#F1F1F1"/>
                </TouchableOpacity>
                <View style={style.areaContentModal}>
                    {children}
                </View>
            </View>
        </Modal>
    )
}

export default BottomModal;