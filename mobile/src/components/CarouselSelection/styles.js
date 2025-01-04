import {StyleSheet} from "react-native";

export const carouselSelectionStyles = StyleSheet.create({
    viewSelect: {
        flexDirection: "row",
        marginTop: 30
    },
    buttonPrev: {
        flex: 1,
        justifyContent: "flex-end",
        alignItems: "flex-end"
    },
    buttonNext: {
        flex: 1,
        alignItems: "flex-start"
    },
    viewTitle: {
        width: 140,
        justifyContent: "center",
        alignItems: "center"
    },
    textTitle: {
        color: "#F1F1F1",
        fontSize: 17,
        fontWeight: "bold"
    },
});