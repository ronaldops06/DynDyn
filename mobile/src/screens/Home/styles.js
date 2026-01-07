import {StyleSheet} from "react-native";

export const getHomeStyle = (theme) => StyleSheet.create({
    textUserArea: {
        flexDirection: "row",
        alignItems: "flex-end",
        paddingLeft: 15,
        height: 35,
        marginTop: 35
    },
    textUser: {
        color: theme.colors.secondaryTextDashboard,
        fontSize: 20,
        marginLeft: 8,
        fontWeight: "bold"
    },
    iconUser: {
        padding: 10,
        height: 35,
        marginTop: 20,
        marginRight: 10
    },
    areaBalance: {
        height: 180,
        marginTop: 30,
        marginHorizontal: 30,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderDashboard,
        borderRadius: 15,
        backgroundColor: theme.colors.primaryAreaDashboard,
        padding: 15
    },
    balanceRow: {
        flexDirection: "row",
        justifyContent: "space-between",
        marginBottom: 10,
    },
    balanceCard: {
        height: 50,
        width: 96,
        padding: 5,
        borderRadius: 10,
        backgroundColor: theme.colors.primaryBaseColor + "21",
    },
    valueHidden: {
        height: 30,
        width: 120,
        backgroundColor: theme.colors.tertiaryAreaDashboard,
    },
    valueProjectionHidden: {
        height: 15,
        width: 70,
        backgroundColor: theme.colors.tertiaryAreaDashboard,
    },
    balanceLabelItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 16
    },
    balanceLabelCardItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 14
    },
    balanceTextInfo: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 14,
        marginBottom: 2
    },
    balanceTextItem: {
        marginLeft: 5,
        fontSize: 22,
    },
    balanceTextItemSecondary: {
        marginLeft: 5,
        fontSize: 12,
    },
    balanceTextPositive: {
        color: theme.colors.primaryMonetaryColor,
    },
    balanceTextNegative: {
        color: theme.colors.secondaryMonetaryColor,
    },
    areaFeatures: {
        marginTop: 5,
        marginHorizontal: 30,
        justifyContent: "space-between"
    },
    featureRow: {
        justifyContent: "space-between",
        flexDirection: "row",
        marginTop: 20
    },
    auxiliaryItem: {
        justifyContent: "center",
        alignItems: "center",
    },
    auxiliaryIconItem: {
        width: 70,
        height: 70,
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: theme.colors.primaryAreaDashboard,
        borderRadius: 35,
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderDashboard
    },
    auxiliaryTextItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 14,
    },
    textArea: {
        marginTop: 30,
        marginLeft: 35,
        color: theme.colors.primaryTextDashboard,
        fontSize: 20,
    },
    featureItem: {
        backgroundColor: theme.colors.primaryAreaDashboard,
        padding: 15,
        borderRadius: 10,
        width: 157,
    },
    featureTextItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 18,
        fontWeight: "bold",
    },
    featureSubTextItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 14,
    },
    areaChart: {
        marginHorizontal: 20
    }
});