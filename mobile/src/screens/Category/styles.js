import { StyleSheet } from "react-native";
import { constants } from '../../constants';

export const categoryStyle = StyleSheet.create({
    viewSelectType: {
        flexDirection: "row",
        marginTop: 30
    },
    buttonPrev: {
        flex: 1,
        justifyContent: "flex-end",
        alignItems: "flex-end"
    },
    buttonNext: {
        flex: 1,
        alignItems: "flex-start"
    },
    viewTypeTitle: {
        width: 140,
        justifyContent: "center",
        alignItems: "center"
    },
    textTypeTitle: {
        color: constants.colors.secondaryBaseColor,
        fontSize: 17,
        fontWeight: "bold"
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
    viewList: {
        flex: 1,
        marginTop: 5,
        marginBottom: 5,
    }
});