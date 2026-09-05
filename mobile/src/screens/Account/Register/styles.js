import {StyleSheet} from "react-native";
import {getStyleCadastro} from "../../../styles/styles.cadastro";

export const getAccountCreateStyle = (theme) => StyleSheet.create({
    areaButtonType: {
        marginTop: 10,
        marginLeft: 25,
        marginRight: 25,  
    },
    areaFields: {
      ...getStyleCadastro(theme).areaFields,
      marginTop: 40,  
    },
    areaButtonSave: {
        ...getStyleCadastro(theme).areaButtonSave,
        marginTop: 40,
    },
    areaCard: {
        ...getStyleCadastro(theme).areaCard,
        marginTop: 30,
    },
    areaField: {
        marginTop: 40,
    }
});