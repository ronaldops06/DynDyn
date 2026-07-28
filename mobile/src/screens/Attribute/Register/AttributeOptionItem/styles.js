import {StyleSheet} from "react-native";

export const getAttributeOptionItemStyle = (theme) => StyleSheet.create({
    card: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 10,
        paddingVertical: 8,
        paddingHorizontal: 8,
        marginBottom: 8,
    },
    label: {
        flex: 0.8,
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
    },
    value: {
        flex: 0.8,
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
    },
    actions: {
        flexDirection: "row",
        alignItems: "center",
        gap: 4,
    },
    actionButton: {
        padding: 4,
    },
});
