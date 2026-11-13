import {StyleSheet} from "react-native";

export const getPortfolioRegisterStyle = (theme) => StyleSheet.create({
    container: {
        paddingHorizontal: 18,
        paddingTop: 18,
        paddingBottom: 32
    },
    imagePicker: {
        height: 210,
        borderWidth: 1,
        borderStyle: "dashed",
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 14,
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: theme.colors.secondaryBaseColor
    },
    imageIcon: {
        width: 72,
        height: 72,
        borderRadius: 36,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor,
        alignItems: "center",
        justifyContent: "center",
        marginBottom: 16
    },
    imageText: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 16
    },
    imageHint: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 13,
        marginTop: 6
    },
    divider: {
        borderBottomColor: theme.colors.primaryBorderColor,
        borderBottomWidth: 1,
        marginBottom: 20,
        marginTop: 16
    },
    sectionTitle: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 18,
        fontWeight: "700",
        marginBottom: 12
    },
    areaButtonType: {
        marginTop: 10,
        marginBottom: 20, 
    },
    switchRow: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
        marginTop: 16,
        marginBottom: 20
    },
    switchLabel: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 15
    },
    helperText: {
        color: theme.colors.secondaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
        lineHeight: 20,
        marginBottom: 16
    },
    attributeButton: {
        height: 44,
        borderWidth: 1,
        borderStyle: "dashed",
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 8,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "center",
        gap: 8
    },
    attributeButtonText: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 15
    },
    examplesTitle: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 14,
        marginTop: 18,
        marginBottom: 10
    },
    chips: {
        flexDirection: "row",
        flexWrap: "wrap",
        gap: 8
    },
    chip: {
        backgroundColor: theme.colors.tertiaryBaseColor,
        borderRadius: 14,
        paddingHorizontal: 12,
        paddingVertical: 6
    },
    chipText: {
        color: theme.colors.primaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 12
    },
    actions: {
        marginTop: 26,
        alignItems: "center",
        gap: 14
    },
    saveButton: {
        width: "100%",
        height: 50,
        borderRadius: 10,
        backgroundColor: theme.colors.primaryBaseColor,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "center",
        gap: 8
    },
    saveText: {
        color: theme.colors.tertiaryTextColor,
        fontFamily: "Open Sans",
        fontSize: 16,
        fontWeight: "700"
    },
    deleteButton: {
        height: 36,
        alignItems: "center",
        justifyContent: "center"
    },
    deleteText: {
        color: theme.colors.dangerTextColor,
        fontFamily: "Open Sans",
        fontSize: 15
    },
    box: {
        marginVertical: 10,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor + "46",
        borderRadius: 20,
        padding: 10
    },
    infoRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        paddingVertical: 12,
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.primaryBorderColor,
    },
    infoLabel: {
        flexDirection: 'row',
        alignItems: 'center',
        flex: 0.8,
        gap: 9
    },
    infoLabelText: {
        fontSize: 14,
        color: theme.colors.primaryTextColor,
        fontFamily: 'Open Sans',
    },
    infoValue: {
        fontSize: 14,
        fontWeight: '600',
        color: theme.colors.primaryTextColor,
        fontFamily: 'Open Sans',
        textAlign: 'right',
        flex: 0.8
    },
    infoAction: {
        flex: 0.2,
        alignItems: 'flex-end',
        marginRight: 5
    }
});
