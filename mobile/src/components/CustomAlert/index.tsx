import { Alert } from 'react-native';


export const CustomAlert = (title: string, message: string, onPress: any) => {
    return (
        Alert.alert(title,
            message,
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: () => { onPress }
                }
            ],
            { cancelable: false }
        )
    );
}