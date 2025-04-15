import { StyleSheet } from "react-native";
import { constants } from "../../constants";
export const transactionCreateStyle = StyleSheet.create({
    areaValue: {
        flexDirection: "row",
        width: "100%",
        justifyContent: "center",
        marginBottom: 20,
    },
    buttonMinus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: constants.colors.primaryBaseColor,
        backgroundColor: constants.colors.secondaryBaseColor, 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonMinus: {
        color: constants.colors.primaryBaseColor
    },
    inputValue: {
        fontSize: 24,
        fontFamily: "Open Sans",
        alignSelf: "center"
    },
    inputValueRevenue: {
        color: constants.colors.primaryMonetaryColor
    },
    inputValueTransfer: {
        color: constants.colors.primaryTextColor
    },
    inputValueExpense: {
        color: constants.colors.secondaryMonetaryColor
    },
    buttonPlus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: constants.colors.secondaryBaseColor,
        backgroundColor: constants.colors.primaryBaseColor, 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonPlus: {
        color: constants.colors.secondaryBaseColor
    },
    textListOperations: {
        color: constants.colors.primaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline",
        marginBottom: 20,
        textAlign: "center"
    },
    areaDateTime: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        width: "100%"
    },
    button: {
        alignItems: "center",
        justifyContent: "center",
        width: 100,
        height: 35,
        borderRadius: 30
    },
    buttonDefault: {
        backgroundColor: constants.colors.primaryBaseColor,
    },
    buttonSelected: {
        backgroundColor: constants.colors.secondaryBaseColor,
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonDefault: {
        color: constants.colors.secondaryBaseColor,
    },
    textButtonSelected: {
        color: constants.colors.primaryBaseColor,
    },
    areaRepeat: {
        flexDirection: "row",
        width: "90%",
        margin: 0,
        marginBottom: 20
        //alignItems: "flex-start"
    },
    radioRepeat: {
        marginLeft: 10
    },
    labelRadioRepeat: {
        fontSize: 14,
        fontFamily: "Open Sans",
        color: constants.colors.primaryTextColor,
    },
    areaTimes: {
        flexDirection: "column",
        width: "30%",
        marginLeft: 10
    },
});