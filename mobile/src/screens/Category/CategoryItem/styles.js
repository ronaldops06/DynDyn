import { StyleSheet } from "react-native";
import { constants } from '~/constants';

export const categoryItemStyle = StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: constants.colors.secondaryBaseColor,
        borderColor: constants.colors.quaternaryBaseColor,
        borderWidth: 1,
        borderRadius: 8,
        padding: 15,
        minHeight: 70
    },
    cardBackground: {
        marginBottom: 20,
        borderRadius: 8,
        backgroundColor: constants.colors.quaternaryBaseColor,
    },
    rowHeader: {
        flexDirection: "row",
        justifyContent: "space-between"

    },
    textHeader: {
        color: constants.colors.primaryTextColor,
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
        color: constants.colors.primaryTextColor,
        fontSize: 12,
        fontFamily: "Open Sans"
    },
    textCategoryName: {
        color: constants.colors.secondaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
});