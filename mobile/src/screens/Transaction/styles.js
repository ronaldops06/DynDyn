import { StyleSheet } from "react-native";
import { constants } from "../../constants";

export const transactionStyle = StyleSheet.create({
    viewSelectDate: {
        flexDirection: "row",
        marginTop: 10
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
        borderColor: constants.colors.primaryBaseColor,
        backgroundColor: constants.colors.secondaryBaseColor,
        alignItems: "center",
        justifyContent: "center",
    },
    viewDateTitle: {
        width: 140,
        justifyContent: "center",
        alignItems: "center"
    },
    textDateTitle: {
        color: constants.colors.secondaryBaseColor,
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
        backgroundColor: constants.colors.secondaryBaseColor ,
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
        color: constants.colors.primaryMonetaryColor,
    },
    textReceita: {
        color: constants.colors.primaryMonetaryColor
    },
    textLabelDespesa: {
        color: constants.colors.secondaryMonetaryColor,
    },
    textDespesa: {
        color: constants.colors.secondaryMonetaryColor
    },
    textLabelSaldo: {
        color: constants.colors.tertiaryMonetaryColor,
    },
    textSaldo: {
        color: constants.colors.tertiaryMonetaryColor
    },
    viewList: {
        flex: 1,
        marginTop: 5,
        marginBottom: 5,
    },
    scroll: {
        flex: 1,
        marginTop: 5
    }
});