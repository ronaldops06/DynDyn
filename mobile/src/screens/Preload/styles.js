import { StyleSheet } from 'react-native';
import { constants } from '../../constants';

export const preloadStyle = StyleSheet.create({
    content: {
        display: "flex",
        flex: 1,
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: constants.colors.primaryBaseColor
    },
    imageArea: {
        backgroundColor: constants.colors.secondaryBaseColor,
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