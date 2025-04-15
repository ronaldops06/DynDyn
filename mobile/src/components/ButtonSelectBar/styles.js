import { StyleSheet } from "react-native";
import { constants } from "../../constants";

export const buttonSelectStyle = StyleSheet.create({
    areaButtonSelect: {
        flex: 1,
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        padding: 5,
        marginTop: 10,
        marginLeft: 25,
        marginRight: 25,
        height: 40,
        backgroundColor: constants.colors.primaryBaseColor,
        borderRadius: 30
    },
    button: {
        flex: 1,
        alignItems: "center",
        justifyContent: "center",
        height: 35,
        borderRadius: 30
    },
    buttonSelected: {
        backgroundColor: constants.colors.secondaryBaseColor,
    },
    buttonDefault: {
        backgroundColor: constants.colors.primaryBaseColor,
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonSelected: {
        color: constants.colors.primaryBaseColor,
    },
    textButtonDefault: {
        color: constants.colors.secondaryBaseColor,
    },
});