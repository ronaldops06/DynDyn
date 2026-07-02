import { StyleSheet } from "react-native";

export const getMonthYearPickerStyle = (theme) => StyleSheet.create({
    container: {
        flexDirection: 'column',
        //gap: 9,
        height: 50
    },
    fields: {
        flexDirection: 'row',
        marginTop: 10,
        justifyContent: 'space-between',
    },
    field: {
        flex: 1,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 8,
        overflow: 'hidden',
        //backgroundColor: theme.colors.primaryBaseColor,
        paddingBottom: 10,
        backgroundColor: theme.colors.secondaryBaseColor,
    },
    picker: {
        border: 0,
        borderColor: "red",
        fontSize: 16,
        height: 40,
        color: theme.colors.primaryTextColor,
    },
    label: {
        fontSize: 14,
        paddingHorizontal: 12,
        paddingTop: 8,
        color: theme.colors.primaryTextColor
    },
});