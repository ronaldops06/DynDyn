import {Dimensions, StyleSheet} from "react-native";

export const getTagStyles = (theme) => StyleSheet.create({
    container: {
        flexDirection: 'row',
        borderRadius: 20,
        paddingVertical: 5,
        paddingHorizontal: 10,
        gap: 3,
    },
    text: {
        fontSize: 14,
        fontFamily: "Open Sans",
        //fontWeight: 'bold'
    },
});