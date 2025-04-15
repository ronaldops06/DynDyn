import { StyleSheet } from "react-native";
import { constants } from '../../constants';

export const accountStyle = StyleSheet.create({
    buttonPlus: {
        width: 50,
        height: 50,
        borderRadius: 100,
        position: "absolute",
        bottom: 15,
        right: 15,
        borderWidth: 2,
        borderColor: constants.colors.primaryBaseColor,
        backgroundColor: constants.colors.secondaryBaseColor,
        alignItems: "center",
        justifyContent: "center",
    },
});