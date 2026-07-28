import { StyleSheet } from "react-native";

export const getAttributeCreateStyle = (theme) => StyleSheet.create({
    areaCard: {
        marginTop: 24
    },
    optionsSection: {
        marginTop: 26,
        borderWidth: 1,
        borderStyle: "dashed",
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 16,
        padding: 14,
        backgroundColor: theme.colors.secondaryBaseColor,
    },
    optionsHeader: {
        flexDirection: "row",
        alignItems: "flex-start",
        justifyContent: "space-between",
        gap: 12,
        marginBottom: 14,
    },
    optionsHeaderText: {
        flex: 1,
        paddingRight: 4,
    },
    optionsTitle: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 18,
        fontWeight: "700",
        marginBottom: 2,
    },
    optionsDescription: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 12,
        lineHeight: 17,
    },
    areaButtonAdd: {
        marginBottom: 15  
    },
    columnLabel: {
        flex: 1.3,
    },
    columnValue: {
        flex: 0.7,
    },
    columnStatus: {
        flex: 0.9,
    },
    columnActions: {
        width: 72,
    },
    emptyState: {
        backgroundColor: theme.colors.primaryBaseColor + "12",
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 12,
        paddingVertical: 16,
        paddingHorizontal: 14,
        marginBottom: 10,
    },
    emptyStateText: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 13,
        textAlign: "center",
    },
    scrollOptions: {
        maxHeight: 150
    },
});
