import { StyleSheet } from "react-native";
import { constants} from "../../constants";

export const operationItemStyle = StyleSheet.create({
    card: {
        flexDirection: "column",
        backgroundColor: constants.colors.secondaryBaseColor,
        borderColor: constants.colors.primaryBorderColor,
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
        color: constants.colors.primaryTextColor,
        fontSize: 15,
        fontFamily: "Open Sans"
    },
    textCategoriaName: {
        color: constants.colors.secondaryTextColor,
        fontSize: 13,
        fontFamily: "Open Sans"
    }
});