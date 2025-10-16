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
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    iconUser: {
        padding: 10,
        height: 35,
        marginTop: 20,
        marginRight: 10
    },
    areaBalance: {
        height: 80,
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
    },
    valueHidden: {
        height: 20,
        width: 120,
        backgroundColor: theme.colors.tertiaryAreaDashboard,
    },
    balanceLabelItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    balanceTextItem: {
        marginLeft: 5,
        fontSize: 22,
        fontFamily: "Open Sans"
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
        fontFamily: "Open Sans"
    },
    textArea: {
        marginTop: 30,
        marginLeft: 35,
        color: theme.colors.primaryTextDashboard,
        fontSize: 20,
        fontFamily: "Open Sans"
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
        fontFamily: "Open Sans"
    },
    featureSubTextItem: {
        color: theme.colors.primaryTextDashboard,
        fontSize: 14,
        fontFamily: "Open Sans"
    },
    areaChart: {
        marginHorizontal: 20
    }
});