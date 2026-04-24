import {StyleSheet} from "react-native";

export const getTotalizerRoleRegisterStyles = (theme) => StyleSheet.create({
    buttonAddRole:{
        backgroundColor: theme.colors.primaryBaseColor,
        height: 40,
        marginTop: 20,
        paddingHorizontal: 20,
        borderRadius: 5,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center"
    },
    scrollRoles: {
        minHeight: 50,
        maxHeight: 150
    },
    areaRoles: {
        minHeight: 50,
        marginTop: 10,
        flexDirection: "row",
        flexWrap: "wrap",
        paddingHorizontal: 10,
        borderTopWidth: 1,
        borderTopColor: theme.colors.tertiaryBorderColor + "21",
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.tertiaryBorderColor + "21",
    }
});