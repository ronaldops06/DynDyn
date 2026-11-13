import {StyleSheet} from "react-native";

export const getPortfolioAttributeStyle = (theme) => StyleSheet.create({
    labelActionType: {
        
    },
    radioActionType: {
        borderColor: theme.colors.primaryBaseColor + "21",
        borderWidth: 1,
        justifyContent: "space-between",
        padding: 10
    },
    areaValue: {
        marginVertical: 10,
        padding: 10,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor + "21",
        borderRadius: 10
    },
    areaFields: {
        marginBottom: 8,
    },
    areaButtonApply: {
        marginBottom: 2,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-around",
    },
});