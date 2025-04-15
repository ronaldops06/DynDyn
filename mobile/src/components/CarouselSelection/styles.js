import {StyleSheet} from "react-native";
import { constants} from "../../constants";

export const carouselSelectionStyles = StyleSheet.create({
    viewSelect: {
        flexDirection: "row",
        marginTop: 10
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
        color: constants.colors.secondaryBaseColor,
        fontSize: 17,
        fontWeight: "bold"
    },
});