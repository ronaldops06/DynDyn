import Keychain from 'react-native-keychain';

export default class SecureStorage {
    static async set(key: string, value: string) {
        await Keychain.setGenericPassword(key, value, {
            service: `sagemoney.${key}`,
        });
    }

    static async get(key: string): Promise<string | null> {
        const result = await Keychain.getGenericPassword({
            service: `sagemoney.${key}`,
        });

        return result ? result.password : null;
    }

    static async remove(key: string) {
        await Keychain.resetGenericPassword({
            service: `sagemoney.${key}`,
        });
    }
    
}