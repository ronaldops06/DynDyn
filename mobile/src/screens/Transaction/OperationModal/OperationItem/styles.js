import { StyleSheet } from "react-native";

export const getOperationItemStyle = (theme) => StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 8,
        marginBottom: 5,
        padding: 10,
        paddingLeft: 15,
        minHeight: 50
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    rowInfo: {
        flexDirection: "row",
        justifyContent: "space-between"
    },
    textOperationName: {
        color: theme.colors.primaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textCategoriaName: {
        color: theme.colors.secondaryTextColor,
        fontSize: 13,
        fontFamily: "Open Sans"
    }
});