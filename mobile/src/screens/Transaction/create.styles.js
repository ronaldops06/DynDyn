import { StyleSheet } from "react-native";

export const transactionCreateStyle = StyleSheet.create({
    areaValue: {
        flexDirection: "row",
        width: "100%",
        justifyContent: "center"
    },
    buttonMinus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: "#6E8BB8",
        backgroundColor: "#F5F5F5", 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonMinus: {
        color: "#6E8BB8"
    },
    inputValue: {
        fontSize: 24,
        fontFamily: "Open Sans",
        alignSelf: "center"
    },
    inputValueRevenue: {
        color: "#3C66F0"
    },
    inputValueTransfer: {
        color: "#4CA346"
    },
    inputValueExpense: {
        color: "#F06868"
    },
    buttonPlus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: "#F5F5F5",
        backgroundColor: "#6E8BB8", 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonPlus: {
        color: "#F5F5F5"
    },
    textListOperations: {
        color: "#6E8BB8",
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
        backgroundColor: "#6E8BB8",
    },
    buttonSelected: {
        backgroundColor: "#F1F1F1",
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonDefault: {
        color: "#F5F5F5",
    },
    textButtonSelected: {
        color: "#6E8BB8",
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
        color: "#6E8BB8",
    },
    areaTimes: {
        flexDirection: "column",
        width: "30%",
        marginLeft: 10
    },
});