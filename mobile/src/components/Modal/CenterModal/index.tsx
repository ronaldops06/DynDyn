import {ReactNode} from "react";
import {Modal, Pressable, View} from "react-native";
import Button from "../../Button";

import {useTheme} from "../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../styles/styles.cadastro";
import {getCenterModalStyle} from "./styles";

interface CenterModalProps {
    show: boolean;
    setShow: (value: boolean) => void;
    textButtonOk: string;
    onPressOk: () => void;
    children: ReactNode;
}

const CenterModal = (props: CenterModalProps) => {

    const {theme} = useTheme();
    const centerModalStyle = getCenterModalStyle(theme);
    const styleCadastro = getStyleCadastro(theme);

    return (
        <Modal
            visible={props.show}
            transparent
            animationType="fade"
        >
            <Pressable
                style={centerModalStyle.overlay}
                onPress={() => props.setShow(false)}
            >
                <Pressable style={centerModalStyle.modal}>
                    <View style={centerModalStyle.content}>
                        {props.children}
                    </View>
                    <View style={styleCadastro.areaButtonSave}>
                        <Button
                            label="Cancelar"
                            onPress={() => props.setShow(false)}
                            type="quaternary"
                        />
                        <Button
                            label={props.textButtonOk}
                            onPress={props.onPressOk}
                            type="tertiary"
                        />
                    </View>
                </Pressable>
            </Pressable>
        </Modal>
    );
}

export default CenterModal;