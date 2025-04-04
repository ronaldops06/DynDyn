import React, {useEffect} from 'react';
import {Image, Pressable, SafeAreaView, View} from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';

import {login} from './preload.api';
import {preloadStyle} from './styles';

const Preload = ({navigation}) => {
    
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