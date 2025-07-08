import { StyleSheet } from "react-native";

export const getTransactionItemStyle = (theme) => StyleSheet.create({
    cardBackground: {
        backgroundColor: theme.colors.primaryBorderColor,
    },
    card: {
        flexDirection: "row",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.primaryBorderColor,
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
        color: theme.colors.secondaryTextColor,
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
        color: theme.colors.secondaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    textTransactionName: {
        color: theme.colors.primaryTextColor,
        fontWeight: "bold",
        fontSize: 15,
        fontFamily: "Open Sans",
        maxWidth: 230
    },
    textTransactionValue: {
        color: theme.colors.primaryMonetaryColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionNameExpense: {
        color: theme.colors.secondaryMonetaryColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionNameTransfer: {
        color: theme.colors.secondaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textTransactionValueExpense: {
        color: theme.colors.secondaryMonetaryColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textTransactionValueTransfer: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    }
});