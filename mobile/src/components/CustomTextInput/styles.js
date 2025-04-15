import {StyleSheet} from "react-native";
import {constants} from "../../constants";

export const customTextInputStyle = StyleSheet.create({
    container: {
        marginTop: 10,
        alignSelf: "center",
    },
    containerInput: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        backgroundColor: constants.colors.secondaryBaseColor,
        paddingTop: 5,
        paddingHorizontal: 10,
        borderBottomWidth: 1,
        borderColor: constants.colors.primaryBorderColor,
        borderRadius: 3,
        alignSelf: "center",
        width: "100%"
    },
    icon: {
        marginTop: 0,
        justifyContent: "flex-end",
        alignItems: "flex-end"
    },
    input: {
        fontSize: 16,
        height: 40,
        color: constants.colors.primaryTextColor,
        flex: 1,
    },
    label: {
        color: constants.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
    },
    textMessage: {
        fontSize: 13,
        fontFamily: "Open Sans",
        color: constants.colors.secondaryTextColor,
        justifyContent: "center",
        alignSelf: "center"
    },
    animatedStyle: {
        top: 10,
        left: 10,
        position: 'absolute',
        borderRadius: 90,
        zIndex: 10000,
    },
});