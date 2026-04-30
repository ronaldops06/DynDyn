import { StyleSheet } from 'react-native';

export const getCustomMarkdownStyle = (theme) => StyleSheet.create({
    blockquote: {
        backgroundColor: 'transparent',
        borderLeftColor: theme.colors.primaryBorderColor,
        borderLeftWidth: 4,
        paddingLeft: 12,
        marginVertical: 8,
    },
    blockquote_text: {
        color: theme.colors.primaryTextColor,
        fontStyle: 'italic',
    },
    text: {
        color: theme.colors.primaryTextColor,
    }
});