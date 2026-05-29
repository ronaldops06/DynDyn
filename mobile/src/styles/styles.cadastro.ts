import {StyleSheet} from 'react-native';

export const getStyleCadastro = (theme) => StyleSheet.create({
    viewHeaderCadastro: {
        width: "100%",
        backgroundColor: theme.colors.primaryBaseColor,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        height: 80,
        borderBottomLeftRadius: 40
    },
    viewBodyCadastro: {
        display: "flex",
        width: "100%",
        height: "100%",
        backgroundColor: theme.colors.secondaryBaseColor,
        flex: 1
    },
    buttonBack: {
        width: 40,
        height: 40,
        marginTop: 20,
        marginLeft: 15
    },
    buttonTrash: {
        marginRight: 15
    },
    areaFields: {
        flex: 1,
        marginTop: 10,
        marginLeft: 5,
        marginRight: 5,
        flexDirection: "column",
        justifyContent: "center",
        padding: 20
    },
    areaGroupCheckbox: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        width: "90%",
    },
    areaCheckbox: {
        flexDirection: "row",
        alignContent: "center",
        alignItems: "center",
        marginBottom: 5
    },
    checkbox: {
        color: theme.colors.primaryTextColor,
    },
    textCheckbox: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: theme.colors.primaryTextColor,
    },
    areaButtonSave: {
        marginTop: 5,
        marginBottom: 15,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-around"
    },
    buttonSave: {
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: theme.colors.primaryBaseColor,
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonSave: {
        color: theme.colors.tertiaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    secondaryButton: {
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: theme.colors.secondaryBaseColor,
        borderColor: theme.colors.tertiaryBorderColor,
        borderWidth: 1,
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center"
    },
    textSecondaryButton: {
        color: theme.colors.quaternaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    primaryAuxiliaryButton: {
        backgroundColor: theme.colors.primaryBaseColor,
        height: 40,
        paddingHorizontal: 20,
        borderRadius: 5,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center"
    },
    secondaryAuxiliaryButton: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor,
        height: 40,
        paddingHorizontal: 20,
        borderRadius: 5,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center",
        elevation: 2,
        shadowColor: '#000',
    }
});