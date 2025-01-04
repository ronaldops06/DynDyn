import { StyleSheet } from "react-native";

export const buttonSelectStyle = StyleSheet.create({
    areaButtonSelect: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        padding: 5,
        marginTop: 10,
        marginLeft: 25,
        marginRight: 25,
        height: 40,
        backgroundColor: "#6E8BB8",
        borderRadius: 30
    },
    button: {
        alignItems: "center",
        justifyContent: "center",
        width: 100,
        height: 35,
        borderRadius: 30
    },
    buttonSelected: {
        backgroundColor: "#F1F1F1",
    },
    buttonDefault: {
        backgroundColor: "#6E8BB8",
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonSelected: {
        color: "#6E8BB8",
    },
    textButtonDefault: {
        color: "#F5F5F5",
    },
});