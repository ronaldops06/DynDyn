import {StyleSheet} from "react-native";
import {styleCadastro} from "../../styles/styles.cadastro";

export const categoryCreateStyle = StyleSheet.create({
    areaFields: {
      ...styleCadastro.areaFields,
      marginTop: 40,  
    },
    areaButtonSave: {
        ...styleCadastro.areaButtonSave,
        marginTop: 40,
    },
    areaCheckbox: {
        ...styleCadastro.areaCheckbox,
        marginTop: 30,
    }
});