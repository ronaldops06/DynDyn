import {StyleSheet} from 'react-native';

export const style = StyleSheet.create({
    container: {
        display: "flex",
        flex: 1,
        flexDirection: "column",
    },
    containerCadastro: {
        backgroundColor: "#F1F1F1"
    },
    containerConsulta: {
        backgroundColor: "#6E8BB8"
    },
    viewHeaderCadastro: {
        width: "100%",
        height: 130,
        backgroundColor: "#6E8BB8",
        borderBottomLeftRadius: 60
    },
    viewBodyCadastro: {
        display: "flex",
        width: "100%",
        height: "100%",
        backgroundColor: "#F1F1F1"
    },
    viewHeaderConsulta: {
        display: "flex",
        height: "20%",
        width: "100%",
        backgroundColor: "#6E8BB8"
    },
    viewHeaderConsultaReduced: {
        display: "flex",
        height: 90,
        width: "100%",
        backgroundColor: "#6E8BB8"
    },
    viewBodyConsulta: {
        display: "flex",
        width: "100%",
        height: "80%",
        backgroundColor: "#F1F1F1",
        //borderTopLeftRadius: 60
    },
    viewBodyConsultaLarger: {
        display: "flex",
        width: "100%",
        backgroundColor: "#F1F1F1",
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
        borderBottomColor: "#D4DBE6",
    },
    textHeaderConsultaTitle: {
        color: "#F1F1F1",
        fontSize: 16,
        fontFamily: "Open Sans",
        marginTop: 10,
        marginLeft: 22,
    },
    titleScreen: {
        flexDirection: "row",
        alignItems: "flex-end",
        paddingLeft: 15,
        height: 35
    },
    titleScreemText: {
        color: "#F1F1F1",
        fontSize: 18,
        marginLeft: 8,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    }
});