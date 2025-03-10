import { StyleSheet } from "react-native";

export const transactionItemStyle = StyleSheet.create({
    cardBackground: {
        backgroundColor: "#E4E9F0",
    },
    card: {
        flexDirection: "row",
        backgroundColor: "#F5F5F5",
        borderColor: "#E4E9F0",
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
        color: "#A4BCE3",
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
        color: "#A4BCE3",
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    textTransactionName: {
        color: "gray",
        fontWeight: "bold",
        fontSize: 15,
        fontFamily: "Open Sans",
        maxWidth: 230
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
        color: "gray",
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    }
});