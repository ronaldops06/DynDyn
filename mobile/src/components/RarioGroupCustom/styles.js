import {StyleSheet} from "react-native";

export const getRadioGroupStyles = (theme) => StyleSheet.create({
    radioLabel: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
    },
    radioActionType: {
        borderBottomColor: theme.colors.primaryBaseColor + "21",
        borderBottomWidth: 1,
        justifyContent: "space-between",
        padding: 10,
        marginTop: 6
    },
    labelOption: {
        
    }
});