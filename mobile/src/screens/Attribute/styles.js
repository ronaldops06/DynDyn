import {StyleSheet} from "react-native";

export const getAttributeStyle = (theme) => StyleSheet.create({
    areaButtonAdd: {
        marginVertical: 10,
        paddingVertical: 10,
        borderTopWidth: 1,
        borderBottomWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor + "21",
    },
    scroll: {
        flex: 1,
    }
});