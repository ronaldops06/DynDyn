import { StyleSheet } from 'react-native';

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
    viewBodyConsulta: {
        display: "flex",
        width: "100%",
        height: "80%",
        backgroundColor: "#F1F1F1",
        borderTopLeftRadius: 60
    },
    loadingIcon: {
        marginTop: 50
    },
});