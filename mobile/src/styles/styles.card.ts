import { StyleSheet } from "react-native";
export const getCardStyle = (theme) => StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 8,
        padding: 15,
        minHeight: 70
    },
    cardBackground: {
        marginBottom: 5,
        borderRadius: 8,
        backgroundColor: theme.colors.primaryBorderColor,
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    textHeader: {
        color: theme.colors.secondaryTextColor,
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
        color: theme.colors.secondaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    textName: {
        color: theme.colors.primaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    boxEnd: {
        flexDirection: "row",
        justifyContent: "space-between",
        width: 80
    }
});