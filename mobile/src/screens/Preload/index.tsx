import React, {useEffect, useState} from 'react';
import {Image, Pressable, SafeAreaView, View} from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';

import {fetchApiUrl} from '../../services/api.ts';
import {login} from './preload.api';
import {preloadStyle} from './styles';

const Preload = ({navigation}) => {
    const [apiUrl, setApiUrl] = useState<string>("");
    
    useEffect(()=>{
        getBaseUrl();
    },[])
    
    useEffect(()=>{
        checkToken();
    },[apiUrl])

    const handleRefreshClick = async () => {
        await getBaseUrl();
        await checkToken();
    };

    const getBaseUrl = async () => {
        const resultApiUrl = await fetchApiUrl();
        if (resultApiUrl) {
            await EncryptedStorage.setItem("API_URL", resultApiUrl);
        }

        setApiUrl(resultApiUrl);
    }
    
    const checkToken = async () => {
        const session = await EncryptedStorage.getItem("user_session");

        if (!session || !JSON.parse(session).token)
            navigation.navigate('SignIn');

        await login(navigation);
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