import { StyleSheet } from "react-native";

export const getDashboardStyle = (theme) => StyleSheet.create({
    areaBody: {
        padding: 10
    },
    widgetArea: {
        backgroundColor: theme.colors.tertiaryBaseColor,
        padding: 15,
        borderRadius: 20,
        marginTop: 20
    },
    widgetHeader: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    widgetHeaderMore: {
        width: 40,
        height: 40,  
    },
    widgetActions: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    widgetActionButton: {
        backgroundColor: theme.colors.secondaryBaseColor,
        alignItems: 'center',
        justifyContent: 'center',
        width: 60,
        height: 30,
    },
    textInfoBlock: {
        marginTop: 20,
        marginBottom: 10,
    }
});