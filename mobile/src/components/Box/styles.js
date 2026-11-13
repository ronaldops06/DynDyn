import { StyleSheet } from "react-native";

export const getBoxStyle = (theme) => StyleSheet.create({
    container: {
        borderWidth: 1,
        borderStyle: "dashed",
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 16,
        padding: 14,
        backgroundColor: theme.colors.secondaryBaseColor,
    },
    noBorder: {
        borderWidth: 0,
    },
    solidBorder: {
        borderStyle: "solid",
    },
    header: {
        flexDirection: "row",
        alignItems: "flex-start",
        justifyContent: "space-between",
        gap: 12,
        marginBottom: 14,
    },
    headerText: {
        flex: 1,
        paddingRight: 4,
    },
    title: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 18,
        fontWeight: "700",
        marginBottom: 2,
    },
    description: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 12,
        lineHeight: 17,
    },
    contentArea: {
        marginBottom: 15,
    },
});
