import { StyleSheet } from 'react-native';

export const preloadStyle = StyleSheet.create({
    content: {
        display: "flex",
        flex: 1,
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "#6E8BB8"
    },
    imageArea: {
        backgroundColor: "#F1F1F1",
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