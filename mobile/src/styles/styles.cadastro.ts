import {StyleSheet} from 'react-native';

import {style} from "./styles.ts";
import {constants} from "../constants";
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
        color: constants.colors.primaryTextColor,
    },
    textCheckbox: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: constants.colors.primaryTextColor,
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
        backgroundColor: constants.colors.primaryBaseColor,
        justifyContent: "center",
        alignItems: "center"
    },
    textButtonSave: {
        color: constants.colors.secondaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
});