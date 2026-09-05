import { StyleSheet } from "react-native";

export const getTransactionCreateStyle = (theme) => StyleSheet.create({
    areaButtonType: {
        marginTop: 10,
        marginLeft: 25,
        marginRight: 25,  
    },
    areaValue: {
        flexDirection: "row",
        width: "100%",
        justifyContent: "center",
    },
    areaCalculator: {
        flexDirection: "row",
        justifyContent: "center",
    },
    buttonMinus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: theme.colors.primaryBaseColor,
        backgroundColor: theme.colors.secondaryBaseColor, 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonMinus: {
        color: theme.colors.primaryBaseColor
    },
    inputValue: {
        marginTop: -20,
        fontSize: 24,
        fontFamily: "Open Sans",
        alignSelf: "center"
    },
    inputValueRevenue: {
        color: theme.colors.primaryMonetaryColor
    },
    inputValueTransfer: {
        color: theme.colors.primaryTextColor
    },
    inputValueExpense: {
        color: theme.colors.secondaryMonetaryColor
    },
    buttonPlus: {
        width: 35,
        height: 35,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: theme.colors.secondaryBaseColor,
        backgroundColor: theme.colors.primaryBaseColor, 
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonPlus: {
        color: theme.colors.secondaryBaseColor
    },
    textListOperations: {
        color: theme.colors.primaryBaseColor,
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
        backgroundColor: theme.colors.primaryBaseColor,
    },
    buttonSelected: {
        backgroundColor: theme.colors.secondaryBaseColor,
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonDefault: {
        color: theme.colors.secondaryBaseColor,
    },
    textButtonSelected: {
        color: theme.colors.primaryBaseColor,
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
        color: theme.colors.primaryTextColor,
    },
    areaTimes: {
        flexDirection: "row",
        justifyContent: "space-between",
        marginTop: 10,
        marginHorizontal: 20
    },
    areaChecks: {
        marginTop: 15,
    },
    areaMoreActions: {
        marginHorizontal: 30,
        marginBottom: 20
    },
    textTotal: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
        textAlign: "left",
        width: "40%",
        alignSelf: "center"
    }
});