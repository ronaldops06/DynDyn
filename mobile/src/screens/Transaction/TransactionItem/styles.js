import { StyleSheet } from "react-native";
import { constants } from "../../../constants";

export const transactionItemStyle = StyleSheet.create({
    cardBackground: {
        backgroundColor: constants.colors.primaryBorderColor,
    },
    card: {
        flexDirection: "row",
        backgroundColor: constants.colors.secondaryBaseColor,
        borderColor: constants.colors.primaryBorderColor,
        borderBottomWidth: 1,
        padding: 15,
        paddingLeft: 5,
        paddingBottom: 5,
        minHeight: 70
    },
    cardType: {
        height: 70,
        width: "12%",
        alignItems: "center",
        justifyContent: "center"
    },
    cardContent: {
        flexDirection: "column",
        width: "88%"
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"
    },
    textHeader: {
        color: constants.colors.secondaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    rowInfo: {
        flexDirection: "row",
        justifyContent: "space-between"
    },
    rowFooter: {
        flexDirection: "row",        
        justifyContent: "space-between"
    },
    textFooter: {
        color: constants.colors.secondaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    textTransactionName: {
        color: constants.colors.primaryTextColor,
        fontWeight: "bold",
        fontSize: 15,
        fontFamily: "Open Sans",
        maxWidth: 230
    },
    textTransactionValue: {
        color: constants.colors.primaryMonetaryColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionNameExpense: {
        color: constants.colors.secondaryMonetaryColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionNameTransfer: {
        color: constants.colors.secondaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionValueExpense: {
        color: constants.colors.secondaryMonetaryColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionValueTransfer: {
        color: constants.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    }
});