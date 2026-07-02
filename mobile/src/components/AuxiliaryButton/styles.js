import { StyleSheet } from "react-native";

export const getAuxiliaryButtonStyle = (theme) => StyleSheet.create({
    button: {
        backgroundColor: theme.colors.primaryBaseColor,
        height: 40,
        marginTop: 20,
        paddingHorizontal: 20,
        borderRadius: 5,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center"
    },
});