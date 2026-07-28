import {StyleSheet} from 'react-native';

export const getStyle = (theme) => StyleSheet.create({
    row: {
        flexDirection: "row",
        justifyContent: "space-between",
    },
    container: {
        display: "flex",
        flex: 1,
        flexDirection: "column",
        fontFamily: "Open Sans"
    },
    containerCadastro: {
        backgroundColor: theme.colors.secondaryBaseColor
    },
    containerConsulta: {
        backgroundColor: theme.colors.primaryBaseColor
    },
    containerAuxiliary: {
        backgroundColor: theme.colors.tertiaryBaseColor
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
    },
    viewBodyConsultaLarger: {
        width: "100%",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderTopLeftRadius: 60,
        flex: 1
    },
    viewAreaMessage: {
        minHeight: 60,
        marginLeft: 20,
        marginRight: 20,
        marginTop: 15,
        padding: 10,
        backgroundColor: theme.colors.dangerBaseColor,
        borderRadius: 10
    },
    textMessage: {
        color: theme.colors.secondaryBaseColor,
        fontSize: 15,
        fontFamily: "Open Sans"
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
        paddingHorizontal: 20,
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
        alignItems: "flex-end",
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
    buttonBack: {
        width: 40,
        height: 40,
        marginTop: 10,
        marginLeft: 15
    },
    headerScreenActions: {
        flexDirection: "row",
        alignItems: "center"
    },
    titleScreenMoreInfo: {
        flexDirection: "row",
        alignItems: "center",
        marginRight: 10
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
        backgroundColor: theme.colors.secondaryBaseColor,
        color: "red"
    },
    areaFooterModal: {
        padding: 10,
    },
    overlay: {
        marginTop: 20,
        alignItems: 'center',
    },
    buttonPlus: {
        width: 50,
        height: 50,
        borderRadius: 100,
        position: "absolute",
        bottom: 15,
        right: 15,
        borderWidth: 2,
        borderColor: theme.colors.tertiaryBorderColor,
        backgroundColor: theme.colors.secondaryBaseColor + "21",
        alignItems: "center",
        justifyContent: "center",
    },
    inputValue: {
        fontSize: 18,
        fontFamily: "Open Sans",
        alignSelf: "flex-end"
    },
    textPrimary24Bold: {
        color: theme.colors.primaryTextColor,
        fontSize: 24,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    textPrimary28Bold: {
        color: theme.colors.primaryTextColor,
        fontSize: 28,
        fontWeight: "bold",
        fontFamily: "Open Sans"
    },
    textPrimary18: {
        color: theme.colors.primaryTextColor,
        fontSize: 18,
        fontFamily: "Open Sans"
    },
    textPrimary16: {
        color: theme.colors.primaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    },
    textSecondary16: {
        color: theme.colors.secondaryTextColor,
        fontSize: 16,
        fontFamily: "Open Sans"
    }
});