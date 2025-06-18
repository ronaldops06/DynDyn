import { StyleSheet } from 'react-native';

import { style } from '../../styles/styles';
import {constants} from "../../constants";

export const signUpStyle = StyleSheet.create({
    container: {
        ...style.container,
        backgroundColor: constants.colors.primaryBaseColor
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
        backgroundColor: constants.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center"
    },
    buttonText: {
        color: constants.colors.secondaryBaseColor,
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
        color: constants.colors.primaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    loginTextLink: {
        color: constants.colors.primaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    },
    text: {
        justifyContent: "center",
        marginTop: 80,
        margin: 20,
        color: constants.colors.primaryBaseColor,
        fontSize: 22,
        fontFamily: "Open Sans"
    },
    overlay: {
        marginTop: 20,
        alignItems: 'center',
    },
});