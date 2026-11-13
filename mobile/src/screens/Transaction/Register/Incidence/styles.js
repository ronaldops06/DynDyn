import {StyleSheet} from "react-native";

export const getIncidenceStyle = (theme) => StyleSheet.create({
    container: {
        flex: 1,
        paddingHorizontal: 15,
        paddingVertical: 10,
        backgroundColor: theme.colors.secondaryBaseColor,
    },

    transactionCard: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 12,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        padding: 15,
        marginBottom: 20,
    },
    cardHeader: {
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-between",
    },
    cardIconContainer: {
        width: 50,
        height: 50,
        borderRadius: 8,
        backgroundColor: theme.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center",
        marginRight: 12,
    },
    cardIcon: {
        fontSize: 24,
    },
    cardContent: {
        flex: 1,
        justifyContent: "center",
    },
    cardTitle: {
        fontSize: 14,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginBottom: 4,
    },
    cardDate: {
        fontSize: 12,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
    },
    cardValue: {
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginLeft: 12,
    },

    reuseSection: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 12,
        padding: 15,
        marginBottom: 15,
        flexDirection: "column",
    },
    reuseContent: {
        marginRight: 10,
        flexDirection: "row",
        justifyContent: "space-between",
    },
    reuseTitle: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginBottom: 4,
    },
    reuseSubtext: {
        maxWidth: "55%",
        fontSize: 11,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
    },
    reuseButton: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor,
        borderRadius: 5,
        paddingHorizontal: 12,
        paddingVertical: 6,
    },
    reuseButtonText: {
        fontSize: 12,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryBaseColor,
    },
    lineSeparetor: {
        borderTopColor: theme.colors.primaryBorderColor,
        borderTopWidth: 1,
        height: 1,
        marginTop: 8
    },
    otherTransactionsLink: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        marginTop: 8
    },
    otherTransactionsText: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryBaseColor,
        textDecorationLine: "underline",
    },

    section: {
        marginBottom: 20,
    },
    sectionHeader: {
        marginBottom: 15,
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "flex-start",
    },
    sectionTitle: {
        fontSize: 14,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginBottom: 4,
    },
    sectionSubtitle: {
        fontSize: 11,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
        maxWidth: "90%",
    },
    
    itemsList: {
        marginTop: 16,
        maxHeight: 200,
        marginBottom: 10,
    },

    abatimentoCard: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 8,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        padding: 12,
        marginBottom: 10,
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
    },
    abatimentoInfo: {
        flex: 1,
    },
    abatimentoMain: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginBottom: 4,
    },
    abatimentoSecondary: {
        fontSize: 11,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
    },
    abatimentoRight: {
        alignItems: "flex-end",
    },
    abatimentoValue: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
        marginTop: 6,
    },
    statusBadge: {
        paddingHorizontal: 8,
        paddingVertical: 4,
        borderRadius: 4,
        marginBottom: 6,
    },
    statusText: {
        fontSize: 10,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.secondaryBaseColor,
    },
    resumoBox: {
        marginBottom: 20
    },
    resumoContent: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center"
    },
    resumoLabel: {
        fontSize: 11,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
        marginBottom: 4,
        maxWidth: 80
    },
    resumoValue: {
        fontSize: 14,
        fontFamily: "Open Sans",
        fontWeight: "600"
    },
    resumoEquals: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
        marginHorizontal: 8
    },

    impactCard: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 8,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        padding: 12,
        marginBottom: 10,
        flexDirection: "column"
    },
    impactInfo: {
        flexDirection: "row",
        justifyContent: "space-between"
    },
    impactMain: {
        fontSize: 15,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor
    },
    impactRow: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center"
    },
    impactDetails: {
        gap: 2,
    },
    impactSecondary: {
        fontSize: 11,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
    },
    impactRight: {
        alignItems: "flex-end",
    },
    impactValue: {
        fontSize: 12,
        fontFamily: "Open Sans",
        fontWeight: "600",
        marginBottom: 6,
    },
    impactAmount: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryTextColor,
    },

    emptyText: {
        fontSize: 12,
        fontFamily: "Open Sans",
        color: theme.colors.secondaryTextColor,
        textAlign: "center",
        paddingVertical: 20,
    },

    buttonsContainer: {
        flexDirection: "row",
        justifyContent: "center",
        marginBottom: 30,
        marginTop: 20,
    },
    button: {
        flex: 1,
        paddingVertical: 14,
        borderRadius: 8,
        justifyContent: "center",
        alignItems: "center",
    },
    buttonCancel: {
        borderWidth: 1,
        borderColor: theme.colors.primaryBaseColor,
        backgroundColor: theme.colors.secondaryBaseColor,
    },
    buttonCancelText: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.primaryBaseColor,
    },
    buttonSave: {
        backgroundColor: theme.colors.primaryBaseColor,
    },
    buttonSaveText: {
        fontSize: 13,
        fontFamily: "Open Sans",
        fontWeight: "600",
        color: theme.colors.secondaryBaseColor,
    },
});
