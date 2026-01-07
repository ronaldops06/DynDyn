import { StyleSheet } from "react-native";

export const getCategoryStyle = (theme) => StyleSheet.create({
    viewSelectType: {
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
    viewTypeTitle: {
        width: 140,
        justifyContent: "center",
        alignItems: "center"
    },
    textTypeTitle: {
        color: theme.colors.secondaryBaseColor,
        fontSize: 17,
        fontWeight: "bold"
    },
    buttonPlus: {
        width: 50,
        height: 50,
        borderRadius: 100,
        position: "absolute",
        bottom: 15,
        right: 15,
        borderWidth: 2,
        borderColor: theme.colors.primaryBaseColor,
        backgroundColor: theme.colors.secondaryBaseColor + "21",
        alignItems: "center",
        justifyContent: "center",
    },
    viewList: {
        flex: 1,
        marginTop: 5,
        marginBottom: 5,
    }
});