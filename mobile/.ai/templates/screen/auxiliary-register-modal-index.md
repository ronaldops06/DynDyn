# Template – Modal utilizada para cadastros auxiliares

Use este template quando for solicitado a criação de uma modal para cadastro auxiliar

```ts
import React, {useEffect, useState} from "react";
import {ScrollView, View} from "react-native";
import BottomModal from "../../../components/BottomModal";
import Button from "../../../components/Button";

import {useTheme} from "../../../contexts/ThemeContext";
import {getStyleCadastro} from "../../../styles/styles.cadastro";
import {getXxxStyle} from "./styles";

interface XxxModalProps {
    show: boolean;
    setShow: (value: boolean) => void;
    addXxx: (item: Xxx) => void;
    Xxx?: Xxx | undefined;
    isEditing: boolean;
};

const XxxModal = (props: XxxModalProps) => {
    const {theme} = useTheme();
    const styleCadastro = getStyleCadastro(theme);
    const XxxStyle = getXxxStyle(theme);

    const [loading, setLoading] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    

    useEffect(() => {
        clearEditing();
        setPageNumber(1);

        loadDataScreen();
    }, [props.show]);

    useEffect(() => {
        if (attributes.length === 0) {
            setPageNumber(1);

        }
    }, [attributes]);
    
    const loadDataScreen = () => {

        if (props.Xxx !== undefined && props.Xxx !== null) {
            
        }
    }

    const handleApply = () => {
        const XxxResponse = props.Xxx ?? {} as Xxx;

        props.addXxx(XxxResponse);
        props.setShow(false);
    }

    const clearEditing = () => {

    }



    return (
        <BottomModal show={props.show} setShow={props.setShow}>
            <ScrollView style={XxxStyle.areaFields}>
               
            </ScrollView>
            <View style={XxxStyle.areaButtonApply}>
                <Button
                    label={"Concluir"}
                    onPress={handleApply}
                />
            </View>
        </BottomModal>
    );
}

export default XxxModal;
```    