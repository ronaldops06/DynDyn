import { StyleSheet } from "react-native";

export const getAccountStyle = (theme) => StyleSheet.create({
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
});