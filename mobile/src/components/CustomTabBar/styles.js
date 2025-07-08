import { StyleSheet } from "react-native";

export const getCustomTabBarStyle = (theme) => StyleSheet.create({
    tabArea: {
        height: 60,
        backgroundColor: theme.colors.tertiaryBaseColor,
        flexDirection: "row"
    },
    tabItem: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center"
    },
    tabItemCenter: {
        width: 85,
        height: 85,
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: theme.colors.tertiaryBaseColor,
        borderRadius: 42,
        borderWidth: 1,
        borderColor: theme.colors.primaryBaseColor,
        marginTop: -30,
    },
    textItem: {
        color: theme.colors.primaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
});