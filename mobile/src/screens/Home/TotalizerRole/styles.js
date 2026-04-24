import {StyleSheet} from "react-native";

export const getTotalizerRoleStyles = (theme) => StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: theme.colors.tertiaryBaseColor
    },
    viewBody: {
        width: "100%",
        flex: 1,
        backgroundColor: theme.colors.tertiaryBaseColor,
        marginTop: 30,
        paddingHorizontal: 5
    },
    titleScreen: {
        flexDirection: "row"
    },
    buttonBack: {
        width: 40,
        height: 40
    },
    card: {
        marginVertical: 2,
        padding: 5,
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 5,
    },
    areaTypes: {
        paddingHorizontal: 10
    },
    areaAdd: {
        borderTopWidth: 1,
        borderBottomWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor + "21",
        paddingTop: 10,
        paddingBottom: 10,
        marginTop: 10,
        marginBottom: 10
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