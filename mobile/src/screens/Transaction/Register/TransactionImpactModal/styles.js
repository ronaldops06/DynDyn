import {StyleSheet} from "react-native";

export const getTransactionImpactModalStyle = (theme) => StyleSheet.create({
    areaFields: {
        marginBottom: 8,
    },
    areaValue: {
        borderWidth: 1,
        borderColor: theme.colors.primaryBorderColor,
        borderRadius: 8,
        marginVertical: 10,
        paddingHorizontal: 12,
        paddingVertical: 4,
    },
    labelValue: {
        color: theme.colors.secondaryTextColor,
        fontSize: 12,
        fontFamily: "Open Sans",
    },
    inputValue: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        paddingVertical: 10,
    },
    areaButtonApply: {
        marginBottom: 2,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-around",
    },
});
