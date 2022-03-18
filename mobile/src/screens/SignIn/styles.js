import { StyleSheet } from 'react-native';

import { style } from '../../styles/styles';

export const signInStyle = StyleSheet.create({
    container: {
        ...style.container,
        backgroundColor: "#F1F1F1" 
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
        backgroundColor: "#6E8BB8",
        justifyContent: "center",
        alignItems: "center"
    },
    buttonText: {
        color: "#F5F5F5",
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerText: {
        marginTop: 120,
        color: "#6E8BB8",
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    registerTextLink: {
        color: "#6E8BB8",
        fontSize: 16,
        fontFamily: "Open Sans",
        textDecorationLine: "underline"
    }
});