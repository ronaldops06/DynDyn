import React, { createContext, useContext, useEffect, useState } from 'react';
import { Appearance } from 'react-native';
import { LightTheme, DarkTheme } from '../styles/themes';
import SecureStorage from "../secureStorage";

const ThemeContext = createContext({
    theme: LightTheme,
    toggleTheme: () => {},
});

export const ThemeContextProvider = ({ children }: {children: any}) => {
    const colorScheme = Appearance.getColorScheme();
    const [theme, setTheme] = useState(
        colorScheme === 'dark' ? DarkTheme : LightTheme
    );
 
    useEffect(() => {
        const validateTheme = async () => {
            const userTheme = await SecureStorage.get("theme");

            if (userTheme)
                setTheme(userTheme === 'dark' ? DarkTheme : LightTheme);
            else
                setTheme(colorScheme === 'dark' ? DarkTheme : LightTheme);
        }

        validateTheme();
        
        const subscription = Appearance.addChangeListener( async ({ colorScheme }) => {
            const userTheme = await SecureStorage.get("theme");
            
            if (userTheme)
                setTheme(userTheme === 'dark' ? DarkTheme : LightTheme);
            else
                setTheme(colorScheme === 'dark' ? DarkTheme : LightTheme);
        });

        return () => subscription.remove();
    }, []);

    useEffect(() => {
        SecureStorage.set("theme", theme.dark ? "dark" : "light");
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
