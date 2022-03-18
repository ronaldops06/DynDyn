import { StyleSheet } from "react-native";

export const customTextInputStyle = StyleSheet.create({
    container: {
      //marginBottom: 15,
      marginTop: 10
    },
      containerInput: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        backgroundColor: "#F1F1F1",
        paddingTop: 5,
        paddingHorizontal: 10,
        borderBottomWidth: 1,
        borderColor: "#D4DBE6",
        borderRadius: 3,
        alignSelf: "center",
        width: "100%",
      },
      icon: {
        marginTop: 0,
        justifyContent: "flex-end",
        alignItems: "flex-end"
      },
      input: {
        fontSize: 16,
        height: 40,
        color: "#6E8BB8",
        flex: 1,
      },
      label: {
        color: "#99ABC9",
        fontFamily: "Open Sans",
        fontSize: 14,
      },
      textMessage: {
        fontSize: 13,
        fontFamily: "Open Sans",
        color: "#A4BCE3",
        justifyContent: "center",
        alignSelf: "center"
      },
      animatedStyle: {
        top: 10,
        left: 10,
        position: 'absolute',
        borderRadius: 90,
        zIndex: 10000,
      },
});