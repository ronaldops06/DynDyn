import {StyleSheet} from "react-native";

export const getAccountUserStyles = (theme) => StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: theme.colors.tertiaryBaseColor
    },
    viewBody: {
        width: "100%",
        flex: 1,
        backgroundColor: theme.colors.tertiaryBaseColor,
        marginTop: 30,
        paddingHorizontal: 25
    },
    titleScreen: {
        flexDirection: "row"
    },
    titleScreemText: {
        color: theme.colors.primaryTextColor,
        fontSize: 24,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    textSettings: {
        marginTop: 20,
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans"
    },
    userProfileText: {
        color: theme.colors.primaryTextColor,
        fontSize: 24,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    userNameText: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontWeight: "bold",
        fontFamily: "Open Sans",
        marginLeft: 10,
        marginRight: 30,
        flexWrap: 'wrap',
    },
    userLoginText: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        marginLeft: 10,
        marginRight: 30,
        flexWrap: 'wrap',
    },
    menuItemText: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
        marginLeft: 10,
    },
    menuItemTextDanger: {
        color: theme.colors.dangerTextColor,
    },
    abboutText: {
        color: theme.colors.tertiaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        marginTop: 15
    },
    abboutSairButtonText: {
        color: theme.colors.tertiaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans",
        textDecorationLine: 'underline',        
    },
    buttonBack: {
        width: 40,
        height: 40
    },
    viewBlock: {
        marginTop: 30,
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 8,
    },
    viewUser: {
        flexDirection: "row",
        alignItems: "center",
        minHeight: 80,
        padding: 15,
        paddingLeft: 20
    },
    viewSettings: {
        minHeight: 183
    },
    viewAccount: {
        minHeight: 122
    },
    viewUserProfile: {
        backgroundColor: theme.colors.tertiaryBaseColor,
        height: 50,
        width: 50,
        borderRadius: 25,
        alignItems: "center",
        justifyContent: "center",
    },
    viewAbout: {
        height: 120,
        backgroundColor: theme.colors.primaryBaseColor,
        flexDirection: "column",
        alignItems: "center"
    },
    row: {
        flexDirection: "column",
    },
    iconNext: {
        marginLeft: 'auto'
    },
    menuItem: {
        flexDirection: "row",
        alignItems: "center",
        minHeight: 60,
        padding: 15,
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.secondaryBorderColor
    },
    abboutSairButton:{
        marginVertical: 20,
        flexDirection: "row",
        alignItems: "center",
    }
})