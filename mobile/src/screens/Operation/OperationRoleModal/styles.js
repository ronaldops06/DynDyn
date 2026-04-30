import {StyleSheet} from 'react-native';

export const getOperationRoleModalStyle = (theme) => StyleSheet.create({
    areaContent: {
        flex: 1
    },
    areaFooter: {
        flexDirection: 'row',
        alignContent: 'space-between',
        marginBottom: 8
    },
    buttonAdd: {
        flexDirection: 'column',
        alignItems: 'center',
        alignContent: 'center',
        justifyContent: 'center',
        height: 50,
        width: 40,
    },
    inputName: {
        fontSize: 16,
        fontFamily: "Open Sans",
        color: theme.colors.primaryTextColor,
        width: '90%',
        height: 50,
        borderRadius: 5,
        borderColor: theme.colors.primaryTextColor,
        borderWidth: 1,
    }
});