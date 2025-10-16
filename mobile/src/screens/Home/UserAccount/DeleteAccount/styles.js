import {StyleSheet} from 'react-native';

export const getDeleteAccountStyle = (theme) => StyleSheet.create({
    areaFields: {
        marginTop: 40,
        marginLeft: 5,
        marginRight: 5,
        style: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        paddingHorizontal: 20,
    },
    text: {
        textAlign: "justify",
        marginTop: 40,
        margin: 20,
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans"
    },
    areaButtonExcluir: {
        marginTop: 60,
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "flex-end"
    },
});