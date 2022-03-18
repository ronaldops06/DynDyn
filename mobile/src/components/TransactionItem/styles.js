import { StyleSheet } from "react-native";

export const transactionItemStyle = StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: "#F5F5F5",
        borderColor: "#E4E9F0",
        borderWidth: 1,
        borderRadius: 8,
        padding: 15,
        minHeight: 70
    },
    cardBackground: {
        marginBottom: 20,
        borderRadius: 8,
        backgroundColor: "#E4E9F0",
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    textHeader: {
        color: "#A4BCE3",
        fontSize: 12,
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
        color: "#A4BCE3",
        fontSize: 12,
        fontFamily: "Open Sans"
    },
    textTransactionName: {
        color: "#3C66F0",
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionValue: {
        color: "#3C66F0",
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionNameExpense: {
        color: "#F06868",
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionNameTransfer: {
        color: "#F06868",
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionValueExpense: {
        color: "#F06868",
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionValueTransfer: {
        color: "#F06868",
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    }
});