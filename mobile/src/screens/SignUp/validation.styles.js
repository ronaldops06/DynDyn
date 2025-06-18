import {StyleSheet} from "react-native";
import {style} from "../../styles/styles";
import {constants} from "../../constants";

export const validationStyle = StyleSheet.create({
    container: {
        ...style.container,
        backgroundColor: constants.colors.primaryBaseColor
    },
    containerCadastro: {
        ...style.containerCadastro,
    },
    viewHeaderCadastro: {
        ...style.viewHeaderCadastro,
    },
    text: {
        textAlign: "justify",
        marginTop: 80,
        margin: 20,
        color: constants.colors.primaryBaseColor,
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
        color: constants.colors.primaryTextColor,
        borderBottomWidth: 1,
        borderBottomColor: constants.colors.primaryBaseColor
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
        color: constants.colors.primaryBaseColor,
        fontSize: 18,
        fontFamily: "Open Sans"
    },
    resendTextLink: {
        color: constants.colors.primaryBaseColor,
        fontSize: 18,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    },
    overlay: {
        marginTop: 20,
        alignItems: 'center',
    },
});