import { StyleSheet } from 'react-native';

import { getStyle } from '../../styles/styles';

export const getSignInStyle = (theme) => StyleSheet.create({
    container: {
        ...getStyle(theme).container,
        backgroundColor: theme.colors.secondaryBaseColor
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
    button: {
        marginTop: 40,
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: theme.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonFingerprint:{
        marginTop: 40,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonText: {
        color: theme.colors.tertiaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerText: {
        marginTop: 120,
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerTextLink: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    }
});