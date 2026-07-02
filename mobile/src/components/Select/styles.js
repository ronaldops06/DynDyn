import { StyleSheet } from "react-native";

export const getSelectStyle = (theme) => StyleSheet.create({
    container: {
        marginTop: 10,
        alignSelf: "center",
    },
    containerInput: {
        display: "flex",
        flexDirection: "row",
        width: "100%",
        justifyContent: "space-between",
        backgroundColor: theme.colors.secondaryBaseColor,
        paddingHorizontal: 10,
        borderBottomWidth: 1,
        borderBottomColor: theme.colors.primaryBorderColor,
        borderRadius: 3,
        alignItems: "flex-end",
        height: 48
    },
    icon: {
        marginBottom: 0,
        justifyContent: "flex-end",
        alignItems: "flex-end"
    },
    input: {
        fontSize: 16,
        height: 45,
        color: theme.colors.primaryTextColor,
        flex: 1
        
    },
    label: {
        color: theme.colors.secondaryTextColor,
        fontSize: 14,
        marginLeft: 6
    },
    buttonAdd: {
        backgroundColor: theme.colors.primaryBaseColor,
        height: 40,
        marginTop: 20,
        paddingHorizontal: 20,
        borderRadius: 5,
        flexDirection: "row",
        justifyContent: "flex-start",
        alignItems: "center"
    }
});