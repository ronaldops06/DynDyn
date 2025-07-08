import { StyleSheet } from "react-native";

export const getOerationItemStyle = (theme) => StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.quaternaryBaseColor,
        borderWidth: 1,
        borderRadius: 8,
        padding: 15,
        minHeight: 70
    },
    cardBackground: {
        marginBottom: 20,
        borderRadius: 8,
        backgroundColor: theme.colors.quaternaryBaseColor,
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    textHeader: {
        color: theme.colors.primaryTextColor,
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
        color: theme.colors.primaryTextColor,
        fontSize: 12,
        fontFamily: "Open Sans"
    },
    textName: {
        color: theme.colors.secondaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
});