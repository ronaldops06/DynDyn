import { StyleSheet } from "react-native";

export const customPickerStyle = StyleSheet.create({
    container: {
        marginBottom: 15,
        marginTop: 10,
        backgroundColor: "#F1F1F1",
        paddingTop: 5,
        paddingHorizontal: 10,
        borderBottomWidth: 1,
        borderColor: "#D4DBE6",
        borderRadius: 3,
        width: "100%",
        alignSelf: "center",
    },
    icon: {
        width: 40,
        justifyContent: "center",
        alignItems: "center",
    },
    picker: {
        border: 0,
        borderColor: "red",
        width: "100%",
        fontSize: 16,
        height: 40,
        color: "#6E8BB8",
    },
    label: {
        color: "#99ABC9",
        fontSize: 14,
    },
    animatedStyle: {
        top: 10,
        left: 10,
        position: 'absolute',
        borderRadius: 90,
        zIndex: 10000,
    },
});