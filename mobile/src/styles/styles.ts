import {StyleSheet} from 'react-native';
import {constants} from '../constants';

export const style = StyleSheet.create({
    container: {
        display: "flex",
        flex: 1,
        flexDirection: "column",
    },
    containerCadastro: {
        backgroundColor: constants.colors.secondaryBaseColor
    },
    containerConsulta: {
        backgroundColor: constants.colors.primaryBaseColor
    },
    viewHeaderCadastro: {
        width: "100%",
        height: 130,
        backgroundColor: constants.colors.primaryBaseColor,
        borderBottomLeftRadius: 60
    },
    viewBodyCadastro: {
        display: "flex",
        width: "100%",
        height: "100%",
        backgroundColor: constants.colors.secondaryBaseColor
    },
    viewHeaderConsulta: {
        display: "flex",
        height: "20%",
        width: "100%",
        backgroundColor: constants.colors.primaryBaseColor
    },
    viewHeaderConsultaReduced: {
        display: "flex",
        height: 90,
        width: "100%",
        backgroundColor: constants.colors.primaryBaseColor
    },
    viewBodyConsulta: {
        display: "flex",
        width: "100%",
        height: "80%",
        backgroundColor: constants.colors.secondaryBaseColor,
        //borderTopLeftRadius: 60
    },
    viewBodyConsultaLarger: {
        display: "flex",
        width: "100%",
        backgroundColor: constants.colors.secondaryBaseColor,
        borderTopLeftRadius: 60,
        flex: 1
    },
    loadingIcon: {
        marginTop: 5,
        marginBottom: 50,
    },
    scrollCadastro: {
        height: "100%"
    },
    scroll: {
        flex: 1,
        paddingLeft: 20,
        paddingRight: 20,
        marginTop: 15
    },
    viewTitle: {
        borderBottomWidth: 1,
        borderBottomColor: constants.colors.tertiaryBaseColor,
    },
    textHeaderConsultaTitle: {
        color: constants.colors.secondaryBaseColor,
        fontSize: 16,
        fontFamily: "Open Sans",
        marginTop: 10,
        marginLeft: 22,
    },
    titleScreen: {
        flexDirection: "row",
        justifyContent: "space-between",
    },
    titleScreemText: {
        color: constants.colors.secondaryBaseColor,
        fontSize: 18,
        marginLeft: 8,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    titleScreenTitle: {
        flexDirection: "row",
        alignItems: "flex-end",
        paddingLeft: 15,
        height: 35
    },
    titleScreenMoreInfo: {
        padding: 10 
    }
});