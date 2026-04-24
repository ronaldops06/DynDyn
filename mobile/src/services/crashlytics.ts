import crashlytics from '@react-native-firebase/crashlytics';

export const crashService = {
    log: (message: string) => {
        crashlytics().log(message);
    },

    recordError: (error: unknown) => {
        if (error instanceof Error) {
            crashlytics().recordError(error);
        }
    },

    setUser: (id: string) => {
        crashlytics().setUserId(id);
    },

    setAttribute: (key: string, value: string) => {
        crashlytics().setAttribute(key, value);
    },

    setAttributes: (attributes: Record<string, string>) => {
        crashlytics().setAttributes(attributes);
    }
};
