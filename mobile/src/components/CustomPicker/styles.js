import { StyleSheet } from "react-native";
import {constants} from "../../constants";

export const customPickerStyle = StyleSheet.create({
    container: {
        marginBottom: 15,
        marginTop: 10,
        backgroundColor: constants.colors.secondaryBaseColor,
        paddingTop: 5,
        borderBottomWidth: 1,
        borderColor: constants.colors.primaryBorderColor,
        borderRadius: 3,
        width: "100%",
        alignSelf: "center",
    },
    icon: {
        width: 40,
        justifyContent: "center",
        alignItems: "center",
    },
    picker: {
        border: 0,
        borderColor: "red",
        width: "100%",
        fontSize: 16,
        height: 40,
        color: constants.colors.primaryTextColor,
    },
    label: {
        color: constants.colors.secondaryTextColor,
        fontSize: 14,
    },
    animatedStyle: {
        top: 10,
        left: 10,
        position: 'absolute',
        borderRadius: 90,
        zIndex: 10000,
    },
});