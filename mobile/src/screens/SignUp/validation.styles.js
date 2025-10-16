import {StyleSheet} from "react-native";
import {getStyle} from "../../styles/styles";

export const getValidationStyle = (theme) => StyleSheet.create({
    container: {
        ...getStyle(theme).container,
        backgroundColor: theme.colors.primaryBaseColor
    },
    containerCadastro: {
        ...getStyle(theme).containerCadastro,
    },
    viewHeaderCadastro: {
        ...getStyle(theme).viewHeaderCadastro,
    },
    text: {
        textAlign: "justify",
        marginTop: 80,
        margin: 20,
        color: theme.colors.primaryTextColor,
        fontSize: 22,
        fontFamily: "Open Sans"
    },
    areaFields: {
        marginTop: 10,
        marginLeft: 5,
        marginRight: 5,
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        paddingHorizontal: 70
    },
    input: {
        fontSize: 32,
        height: 80,
        color: theme.colors.primaryTextColor,
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.tertiaryBorderColor
    },
    areaButtonValidate: {
        marginTop: 50,
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "flex-end"
    },
    areaResend: {
        marginTop: 100,
        margin: 20,
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
    },
    resendText: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans"
    },
    resendTextLink: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    }
});