import {StyleSheet} from "react-native";
import {getStyleCadastro} from "../../../../styles/styles.cadastro";

export const getChangePasswordStyle = (theme) => StyleSheet.create({
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
    }
});