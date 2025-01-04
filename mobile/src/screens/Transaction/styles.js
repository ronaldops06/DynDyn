import { StyleSheet } from "react-native";

export const transactionStyle = StyleSheet.create({
    viewSelectDate: {
        flexDirection: "row",
        marginTop: 30
    },
    buttonPrev: {
        flex: 1,
        justifyContent: "flex-end",
        alignItems: "flex-end"
    },
    buttonPlus: {
        width: 50,
        height: 50,
        borderRadius: 100,
        position: "absolute",
        bottom: 15,
        right: 15,
        borderWidth: 2,
        borderColor: "#6E8BB8",
        backgroundColor: "#F1F1F1",
        alignItems: "center",
        justifyContent: "center",
    },
    viewDateTitle: {
        width: 140,
        justifyContent: "center",
        alignItems: "center"
    },
    textDateTitle: {
        color: "#F1F1F1",
        fontSize: 17,
        fontWeight: "bold"
    },
    buttonNext: {
        flex: 1,
        alignItems: "flex-start"
    },
    viewTotais: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        height: 60,
        marginTop: -30,
        marginLeft: 25,
        marginRight: 25
    },
    cardTotais: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: 110,
        backgroundColor: "#F1F1F1" ,
        borderRadius: 15  
    },
    textLabelTotais: {
        fontSize: 13,
        fontFamily: "Open Sans"
    },
    textTotais: {
        fontSize: 16,
        fontFamily: "Open Sans",
        fontWeight: "bold"
    },
    textLabelReceita: {
        color: "#3C66F0",
    },
    textReceita: {
        color: "#3C66F0"
    },
    textLabelDespesa: {
        color: "#F06868",
    },
    textDespesa: {
        color: "#F06868"
    },
    textLabelSaldo: {
        color: "#4CA346",
    },
    textSaldo: {
        color: "#4CA346"
    },
    viewList: {
        flex: 1,
        marginTop: 5,
        marginBottom: 5,
    },
});