import { StyleSheet } from 'react-native';

import { getStyle } from '../../styles/styles';

export const getSignUpStyle = (theme) => StyleSheet.create({
    container: {
        ...getStyle(theme).container,
        backgroundColor: theme.colors.primaryBaseColor
    },
    viewBodyCadastro: {
        ...getStyle(theme).viewBodyCadastro,
        flexDirection: "column",
        justifyContent: "center"
    },
    areaFields: {
        marginLeft: 5,
        marginRight: 5,
        style: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center"
    },
    areaButtonSave: {
        marginBottom: 15,
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "flex-end"
    },
    button: {
        marginTop: 40,
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: theme.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonText: {
        color: theme.colors.tertiaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    areaTextLogin: {
        marginTop: 20,
        marginBottom: 15,
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
    },
    loginText: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    loginTextLink: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    },
    text: {
        justifyContent: "center",
        marginTop: 80,
        margin: 20,
        color: theme.colors.primaryBaseColor,
        fontSize: 22,
        fontFamily: "Open Sans"
    },
    overlay: {
        marginTop: 20,
        alignItems: 'center',
    },
});