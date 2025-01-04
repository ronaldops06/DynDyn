import { StyleSheet } from "react-native";

export const operationItemStyle = StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: "#F5F5F5",
        borderColor: "#E4E9F0",
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
        color: "#3C66F0",
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textCategoriaName: {
        color: "#A4BCE3",
        fontSize: 13,
        fontFamily: "Open Sans"
    }
});