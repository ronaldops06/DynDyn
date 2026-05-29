import { StyleSheet } from "react-native";

export const getCenterModalStyle = (theme) => StyleSheet.create({
    overlay: {
        flex: 1,
        backgroundColor: 'rgba(0,0,0,0.4)',
        justifyContent: 'center',
        alignItems: 'center',
        padding: 24,
    },
    modal: {
        width: '100%',
        minHeight: 400,
        maxHeight: '80%',
        borderColor: theme.colors.secondaryBaseColor,
        borderWidth: 1,
        backgroundColor: theme.colors.primaryBaseColor,
        borderRadius: 16,
        elevation: 2,
        shadowColor: '#FFF',
    },
    content: {
        flex: 1,
        backgroundColor: theme.colors.secondaryBaseColor,
        margin: 15,
        padding: 15,
        borderRadius: 14,
    },
    footer: {
        flexDirection: 'row',
        justifyContent: 'flex-end',
        gap: 12,
        marginTop: 20,
    },
    cancelButton: {
        paddingVertical: 12,
        paddingHorizontal: 16,
    },
    confirmButton: {
        backgroundColor: '#222',
        paddingVertical: 12,
        paddingHorizontal: 16,
        borderRadius: 8,
    },
});