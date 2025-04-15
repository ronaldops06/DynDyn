import { StyleSheet } from 'react-native';
import {constants} from "../../constants";

export const operationModalStyle = StyleSheet.create({
    areaModal: {
        marginTop: "auto",
        height: "65%",
        backgroundColor: constants.colors.primaryBaseColor,
        justifyContent: "flex-end",
        borderTopLeftRadius: 35,
        borderTopRightRadius: 35
    },
    buttonClose: {
        width: 40,
        height: 40,
        marginTop: 10,
        marginLeft: 15,
    },
    areaContent: {
        flex: 1,
        margin: 20,
        marginTop: 5,
        marginBottom: 10,
        padding: 10,
        borderRadius: 20,
        backgroundColor: constants.colors.secondaryBaseColor
    }
});