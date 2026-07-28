import {StyleSheet} from 'react-native';

export const getStyleSpecial = (theme) => StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: theme.colors.secondaryBaseColor
    },
    viewBody: {
        width: "100%",
        flex: 1,
        backgroundColor: theme.colors.secondaryBaseColor,
        paddingTop: 20,
        paddingHorizontal: 15
    },
    titleScreen: {
        flexDirection: "row"
    },
    buttonBack: {
        width: 40,
        height: 40
    },
})