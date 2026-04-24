import React, { createContext, useContext, useEffect, useState } from 'react';
import { Appearance } from 'react-native';
import { LightTheme, DarkTheme } from '../styles/themes.ts';
import EncryptedStorage from "react-native-encrypted-storage";

const ThemeContext = createContext({
    theme: LightTheme,
    toggleTheme: () => {},
});

export const ThemeContextProvider = ({ children }) => {
    const colorScheme = Appearance.getColorScheme();
    const [theme, setTheme] = useState(
        colorScheme === 'dark' ? DarkTheme : LightTheme
    );
 
    useEffect(() => {
        const validateTheme = async () => {
            const userTheme = await EncryptedStorage.getItem("theme");

            if (userTheme)
                setTheme(userTheme === 'dark' ? DarkTheme : LightTheme);
            else
                setTheme(colorScheme === 'dark' ? DarkTheme : LightTheme);
        }

        validateTheme();
        
        const subscription = Appearance.addChangeListener( async ({ colorScheme }) => {
            const userTheme = await EncryptedStorage.getItem("theme");
            
            if (userTheme)
                setTheme(userTheme === 'dark' ? DarkTheme : LightTheme);
            else
                setTheme(colorScheme === 'dark' ? DarkTheme : LightTheme);
        });

        return () => subscription.remove();
    }, []);

    useEffect(() => {
        EncryptedStorage.setItem("theme", theme.dark ? "dark" : "light");
    }, [theme]);

    const toggleTheme = async () => {
        setTheme((prev) => (prev.dark ? LightTheme : DarkTheme));
    };

    return (
        <ThemeContext.Provider value={{ theme, toggleTheme }}>
            {children}
        </ThemeContext.Provider>
    );
};

export const useTheme = () => useContext(ThemeContext);
