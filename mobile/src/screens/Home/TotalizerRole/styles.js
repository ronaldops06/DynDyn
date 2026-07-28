import {StyleSheet} from "react-native";

export const getTotalizerRoleStyles = (theme) => StyleSheet.create({
    areaAdd: {
        borderTopWidth: 1,
        borderBottomWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor + "21",
        paddingTop: 10,
        paddingBottom: 10,
        marginTop: 10,
        marginBottom: 10,
    },
    buttonAdd: {
        backgroundColor: theme.colors.secondaryBaseColor,
        height: 40,
        paddingHorizontal: 20,
        borderRadius: 5,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center"
    },
    card: {
        marginVertical: 5,
        padding: 5,
        borderColor: theme.colors.tertiaryBorderColor + "21",
        borderWidth: 1,
        borderRadius: 5,
    },
    areaTypes: {
        paddingHorizontal: 10
    },
    scrollRoles: {
        minHeight: 50,
        maxHeight: 150,
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