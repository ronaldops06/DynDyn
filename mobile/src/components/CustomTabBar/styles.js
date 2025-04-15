import { StyleSheet } from "react-native";
import {constants} from "../../constants";

export const customTabBarStyle = StyleSheet.create({
    tabArea: {
        height: 60,
        backgroundColor: constants.colors.tertiaryBaseColor,
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
        backgroundColor: constants.colors.tertiaryBaseColor,
        borderRadius: 35,
        borderWidth: 1,
        borderColor: constants.colors.secondaryTextColor,
        marginTop: -25
    }
});