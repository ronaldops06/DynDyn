import {StyleSheet} from 'react-native';

import {style} from "./styles.ts";

export const styleCadastro = StyleSheet.create({
    viewHeaderCadastro: {
        ...style.viewHeaderCadastro,
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        height: 80,
        borderBottomLeftRadius: 40
    },
    viewBodyCadastro: {
        ...style.viewBodyCadastro,
        flex: 1
    },
    buttonBack: {
        width: 40,
        height: 40,
        marginTop: 20,
        marginLeft: 15

    },
    buttonTrash: {
        width: 40,
        height: 40,
        marginTop: 20,
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
        color: "#6E8BB8",
    },
    textCheckbox: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: "#6E8BB8",
    },
    areaButtonSave: {
        flex: 1,
        marginBottom: 15,
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "flex-end"
    },
    buttonSave: {
        width: 130,
        height: 40,
        borderRadius: 30,
        backgroundColor: "#6E8BB8",
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonSave: {
        color: "#F5F5F5",
        fontSize: 16,
        fontFamily: "Open Sans"
    },
});