import React, { useEffect } from 'react';
import { SafeAreaView, View, Image, Pressable } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { useNavigation } from '@react-navigation/native';
import {StackNavigationProp} from '@react-navigation/stack';

import { login } from './preload.api';

import { RootStackParamList } from '../RootStackParams';
import { preloadStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Preload'>;

const Preload = () => {

    const navigation = useNavigation<homeScreenProp>();

    const checkToken = async () => {
        const session = await EncryptedStorage.getItem("user_session");

        if (!session || !JSON.parse(session).token)
            navigation.navigate('SignIn');
        
        await login(navigation);
    };

    useEffect(()=>{
        checkToken();
    },[])

    const handleRefreshClick = () => {
        checkToken();
    };

    return(
        <SafeAreaView style={preloadStyle.content}>
            <Pressable
                onPress={handleRefreshClick}>
                <View
                    style={preloadStyle.imageArea}>
                    <Image
                        style={preloadStyle.image}
                        source={require('../../assets/house.png')}
                    />
                </View>
            </Pressable>
        </SafeAreaView>
    );
}

export default Preload;