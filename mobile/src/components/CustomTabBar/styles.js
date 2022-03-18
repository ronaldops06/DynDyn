import { StyleSheet } from "react-native";

export const customTabBarStyle = StyleSheet.create({
    tabArea: {
        height: 60,
        backgroundColor: "#D4DBE6",
        flexDirection: "row"
    },
    tabItem: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center"
    },
    tabItemCenter: {
        width: 70,
        height: 70,
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "#D4DBE6",
        borderRadius: 35,
        borderWidth: 1,
        borderColor: "#A3BBE0",
        marginTop: -25
    }
});