import { StyleSheet } from 'react-native';

export const getCalculatorStyle = (theme) => StyleSheet.create({
    areaValueDefault: {
        flexDirection: 'row',
        minHeight: 10,
        borderBottomWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor
    },
    areaResult: {
        flexDirection: 'row',   
        justifyContent: 'flex-end',
        marginTop: 10,
        marginBottom: 10,
        borderBottomWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor
    },
    areaOperation: {
        flexDirection: 'column',
        marginHorizontal: 20
    },
    areaOperationRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 10
    },
    operationButton: {
        alignItems: 'center',
        justifyContent: 'center',
        height: 60,
        width: 60,
        borderWidth: 1,
        borderColor: theme.colors.tertiaryBorderColor,
        backgroundColor: theme.colors.secondaryBaseColor,
        borderRadius: 30
    },
    textOperator: {
        color: theme.colors.sextenaryTextColor
    },
    buttonOperator: {
        borderColor: theme.colors.sextenaryTextColor
    },
    textCalculate: {
        color: theme.colors.tertiaryTextColor
    },
    buttonCalculate: {
        backgroundColor: theme.colors.quaternaryBaseColor,
        borderColor: theme.colors.quaternaryBorderColor
    },
    textBackspace: {
        color: theme.colors.dangerTextColor
    },
    buttonBackspace: {
        borderColor: theme.colors.dangerBaseColor
    }
    
});