import { StyleSheet } from "react-native";

export const getCustomPickerStyle = (theme) => StyleSheet.create({
    container: {
        marginBottom: 15,
        marginTop: 10,
        backgroundColor: theme.colors.secondaryBaseColor,
        paddingTop: 5,
        borderBottomWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
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
        color: theme.colors.primaryTextColor,
    },
    label: {
        color: theme.colors.secondaryTextColor,
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