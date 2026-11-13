import {StyleSheet} from "react-native";
import {getStyleCadastro} from "../../../styles/styles.cadastro";

export const getOperationCreateStyle = (theme) => StyleSheet.create({
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
        marginTop: 30,
        marginBottom: 30  
    },
    scrollRoles: {
        minHeight: 50,
        maxHeight: 150
    },
    areaRoles: {
        minHeight: 50,
        marginTop: 10,
        flexDirection: "row",
        flexWrap: "wrap",
        paddingHorizontal: 10,
        borderTopWidth: 1,
        borderTopColor: theme.colors.tertiaryBorderColor + "21",
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.tertiaryBorderColor + "21",
    }
});