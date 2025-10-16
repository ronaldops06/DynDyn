import {StyleSheet} from 'react-native';

export const getStyle = (theme) => StyleSheet.create({
    container: {
        display: "flex",
        flex: 1,
        flexDirection: "column",
    },
    containerCadastro: {
        backgroundColor: theme.colors.secondaryBaseColor
    },
    containerConsulta: {
        backgroundColor: theme.colors.primaryBaseColor
    },
    viewHeaderCadastro: {
        width: "100%",
        height: 130,
        backgroundColor: theme.colors.primaryBaseColor,
        borderBottomLeftRadius: 60
    },
    viewBodyCadastro: {
        display: "flex",
        width: "100%",
        height: "100%",
        backgroundColor: theme.colors.secondaryBaseColor
    },
    viewHeaderConsulta: {
        display: "flex",
        height: "20%",
        width: "100%",
        backgroundColor: theme.colors.primaryBaseColor
    },
    viewHeaderConsultaReduced: {
        display: "flex",
        height: 90,
        width: "100%",
        backgroundColor: theme.colors.primaryBaseColor
    },
    viewBodyConsulta: {
        display: "flex",
        width: "100%",
        height: "80%",
        backgroundColor: theme.colors.secondaryBaseColor,
        //borderTopLeftRadius: 60
    },
    viewBodyConsultaLarger: {
        display: "flex",
        width: "100%",
        backgroundColor: theme.colors.secondaryBaseColor,
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
        borderBottomColor: theme.colors.tertiaryBaseColor,
    },
    textHeaderConsultaTitle: {
        color: theme.colors.secondaryBaseColor,
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
        color: theme.colors.tertiaryTextColor,
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
    },
    areaModal: {
        marginTop: "auto",
        height: "65%",
        backgroundColor: theme.colors.primaryBaseColor,
        justifyContent: "flex-end",
        borderTopLeftRadius: 35,
        borderTopRightRadius: 35
    },
    buttonCloseModal: {
        width: 40,
        height: 40,
        marginTop: 10,
        marginLeft: 15,
    },
    areaContentModal: {
        flex: 1,
        margin: 20,
        marginTop: 5,
        marginBottom: 10,
        padding: 10,
        borderRadius: 20,
        backgroundColor: theme.colors.secondaryBaseColor
    },
    overlay: {
        marginTop: 20,
        alignItems: 'center',
    },
});