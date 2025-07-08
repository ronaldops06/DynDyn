import {StyleSheet} from "react-native";

export const getCarouselSelectionStyles = (theme) => StyleSheet.create({
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
        color: theme.colors.tertiaryTextColor,
        fontSize: 17,
        fontWeight: "bold"
    },
});