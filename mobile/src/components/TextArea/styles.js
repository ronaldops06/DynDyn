import {StyleSheet} from "react-native";

export const getTextAreaStyle = (theme) => StyleSheet.create({
    field: {
        marginTop: 20
    },
    fieldLabel: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
        marginBottom: 8
    },
    textArea: {
        minHeight: 112,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 8,
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 16,
        paddingHorizontal: 14,
        paddingVertical: 12
    }
});
