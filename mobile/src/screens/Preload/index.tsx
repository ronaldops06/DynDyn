import React, { useEffect } from 'react';
import { Alert, SafeAreaView, View, Image, Pressable } from 'react-native';
import { MMKV } from 'react-native-mmkv'
import { useNavigation } from '@react-navigation/native';
import {StackNavigationProp} from '@react-navigation/stack';

import { login } from './preload.api';

import { RootStackParamList } from '../RootStackPrams';
import { preloadStyle } from './styles';

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Preload'>;

const Preload = () => {

    const navigation = useNavigation<homeScreenProp>();

    const checkToken = async () => {
        const storage = new MMKV();
        const token = storage.getString('token')
        
        if (token){
            await login(navigation);
        } else {
            navigation.navigate('SignIn');
        }
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