import { StyleSheet } from "react-native";
import { constants } from "../constants";
export const cardStyle = StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: constants.colors.secondaryBaseColor,
        borderColor: constants.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 8,
        padding: 15,
        minHeight: 70
    },
    cardBackground: {
        marginBottom: 5,
        borderRadius: 8,
        backgroundColor: constants.colors.primaryBorderColor,
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    textHeader: {
        color: constants.colors.secondaryTextColor,
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
        color: constants.colors.secondaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    textName: {
        color: constants.colors.primaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    boxEnd: {
        flexDirection: "row",
        justifyContent: "space-between",
        width: 80
    }
});