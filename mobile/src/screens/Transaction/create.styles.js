import { StyleSheet } from "react-native";

import { style } from '../../styles/styles';

export const transactionCreateStyle = StyleSheet.create({
    scroll: {
        height: "100%"
    },
    viewHeaderCadastro: {
        ...style.viewHeaderCadastro,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        height: 80,
        borderBottomLeftRadius: 40
    },
    viewBodyCadastro: {
        ...style.viewBodyCadastro,
        flex: 1
    },
    buttonBack: {
        width: 40,
        height: 40,
        marginTop: 20,
        marginLeft: 15

    },
    buttonTrash: {
        width: 40,
        height: 40,
        marginTop: 20,
        marginRight: 15
    },
    areaFields: {
        flex: 1,
        marginTop: 10,
        marginLeft: 5,
        marginRight: 5,
        style: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
    },
    areaValue: {
        flexDirection: "row",
        width: "60%",
        justifyContent: "space-between"
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
        fontSize: 18,
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
        marginBottom: 20
    },
    areaDateTime: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        width: "90%"
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
    areaCheckbox: {
        flexDirection: "row",
        alignContent: "center",
        alignItems: "center",
        width: "90%",
        marginBottom: 5
    },
    checkbox:{
        color: "#6E8BB8",
    },
    textCheckbox: {
        fontSize: 16,
        fontFamily: "Open Sans",
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
    areaButtonSave: {
        flex: 1,
        marginBottom: 15,
        flexDirection: "column",
        alignItems: "center",   
        justifyContent: "flex-end"
    },
    buttonSave: {
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: "#6E8BB8",
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonSave: {
        color: "#F5F5F5",
        fontSize: 16,
        fontFamily: "Open Sans"
    },
});