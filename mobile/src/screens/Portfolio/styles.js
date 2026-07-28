import {StyleSheet} from "react-native";

export const getPortfolioStyle = (theme) => StyleSheet.create({
    areaCardTotals: {
        width: '100%',
        flexDirection: 'row',
        justifyContent: 'space-between',
        paddingVertical: 15,
        gap: 10
    },
    cardTotal: {
        flex: 1,
        borderRadius: 20,
        height: 85,
        padding: 10
    },
    cardTotalLiability: {
        backgroundColor: theme.colors.quaternaryBaseColor + "0D",
        borderColor: theme.colors.quaternaryBaseColor + "80",
        borderWidth: 1,
    },
    cardTotalAsset: {
        backgroundColor: theme.colors.dangerBaseColor + "0D",
        borderColor: theme.colors.dangerBaseColor + "80",
        borderWidth: 1,
    },
    textTotalLiability: {
        color: theme.colors.sextenaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
    },
    textTotalAsset: {
        color: theme.colors.dangerTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
    },
    cardTotalLabel: {
        color: theme.colors.quaternaryTextColor,
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    areaButtonSelect: {
        flexDirection: "row",
        justifyContent: "space-between",
        alignItems: "center",
        height: 40,
        backgroundColor: theme.colors.secondaryBaseColor,
        marginTop: 15
    },
    button: {
        flex: 1,
        alignItems: "center",
        justifyContent: "center",
        height: 38
    },
    buttonSelected: {
        borderBottomColor: theme.colors.tertiaryBorderColor,
        borderBottomWidth: 1,
    },
    buttonDefault: {
        borderBottomColor: theme.colors.secondaryBorderColor,
        borderBottomWidth: 1,
    },
    textButton: {
        fontSize: 14,
        fontFamily: "Open Sans",
    },
    textButtonSelected: {
        color: theme.colors.quaternaryTextColor,
    },
    textButtonDefault: {
        color: theme.colors.secondaryTextColor,
    },
    scroll: {
        paddingVertical: 10,
    },
    portfolioGroup: {
        padding: 10,
        borderColor: theme.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 10,
        marginTop: 10,
    },
    portfolioGroupHeader: {
      flexDirection: "row",
      justifyContent: "space-between"
    },
    portfolioGroupHeaderRigth: {
        flexDirection: "row",
        justifyContent: "space-between",
        minWidth: 140
    },
    portfolioGroupHeaderLeft: {
        flexDirection: "row",
        gap: 5
    },
    portfolioGroupItens: {
        margin: 5,
        padding: 10,
        borderColor: theme.colors.primaryBorderColor,
        borderWidth: 1,
        borderRadius: 10,
    }
});