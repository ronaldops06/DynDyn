import { StyleSheet } from 'react-native';

export const getPreloadStyle = (theme) => StyleSheet.create({
    content: {
        display: "flex",
        flex: 1,
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: theme.colors.primaryBaseColor
    },
    imageArea: {
        backgroundColor: theme.colors.secondaryBaseColor,
        borderTopLeftRadius: 130,
        borderTopRightRadius: 130,
        borderBottomLeftRadius: 160,
        borderBottomRightRadius: 160
    },
    image: {
        width: 380,
        height: 254,
    }
})