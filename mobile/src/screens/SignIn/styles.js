import { StyleSheet } from 'react-native';

import { style } from '../../styles/styles';
import { constants } from '../../constants';

export const signInStyle = StyleSheet.create({
    container: {
        ...style.container,
        backgroundColor: constants.colors.secondaryBaseColor
    },
    viewBodyCadastro: {
        ...style.viewBodyCadastro,
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
        backgroundColor: constants.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonFingerprint:{
        marginTop: 40,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonText: {
        color: constants.colors.secondaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerText: {
        marginTop: 120,
        color: constants.colors.primaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerTextLink: {
        color: constants.colors.primaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    }
});