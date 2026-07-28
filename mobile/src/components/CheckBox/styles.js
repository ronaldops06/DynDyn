import { StyleSheet } from "react-native";

export const getCheckBoxStyle = (theme) => StyleSheet.create({
    areaCard: {
        padding: 15,
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        borderRadius: 10,
        backgroundColor: theme.colors.tertiaryBaseColor + "46",
        minHeight: 80,
        gap: 10
    },
    areaCheckbox: {
        alignItems: 'center',
        justifyContent: 'center',
        height: 30,
        width: 52,
        borderRadius: 40,
        marginLeft: 'auto',
    },
    areaCheckboxChecked: {
        backgroundColor: theme.colors.primaryBaseColor
    },
    areaCheckboxUnchecked: {
        backgroundColor: theme.colors.secondaryBaseColor
    },
    areaText: {
        flexDirection: 'column',
        maxWidth: "65%"
    },
    label: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
        fontWeight: 'bold',
    },
    description: {
        color: theme.colors.primaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans",
        textAlign: "justify",
    }
});