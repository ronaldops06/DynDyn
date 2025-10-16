import {StyleSheet} from "react-native";

const CIRCLE_SIZE = 25;

export const getStepIndicatorStyle = (theme) => StyleSheet.create({
    container: {
        display: "flex",
        flexDirection: "row",
        margin: 12,
        alignItems: 'center',
        justifyContent: "space-between",
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 14,
        borderColor: theme.colors.primaryBaseColor,
        borderWidth: 0.5,
        height: 31,
        opacity: 0.8
    },
    step: {
        flex: 1,
        flexDirection: 'row',
        alignItems: 'center',
        height: 28,
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 12,
    },
    stepActive: {
        backgroundColor: theme.colors.tertiaryBaseColor,
    },
    firstStepCompleted: {
        backgroundColor: theme.colors.primaryBaseColor,
        borderTopRightRadius: 12,
        borderBottomRightRadius: 12,
    },
    stepCompleted: {
        backgroundColor: theme.colors.primaryBaseColor,
        borderTopLeftRadius: 0,
        borderBottomLeftRadius: 0,
        borderTopRightRadius: 12,
        borderBottomRightRadius: 12,
    },
    secondaryStepCompleted: {
        borderTopRightRadius: 0,
        borderBottomRightRadius: 0,
    },
    stepNumberText: {
        color: theme.colors.primaryBaseColor,
        fontSize: 15,
        fontFamily: "Open Sans",
    },
    stepNumber: {
        alignItems: 'center',
        width: CIRCLE_SIZE,
        height: CIRCLE_SIZE,
        borderRadius: CIRCLE_SIZE / 2,
        borderColor: theme.colors.primaryBaseColor,
        borderWidth: 1,
        backgroundColor: theme.colors.secondaryBaseColor,
        marginHorizontal: 3,
    },
    stepText: {
        color: theme.colors.primaryBaseColor,
        fontSize: 13,
        fontFamily: "Open Sans",
        marginLeft: 2
    }
});