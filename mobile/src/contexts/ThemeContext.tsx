import React, { createContext, useContext, useEffect, useState } from 'react';
import { Appearance } from 'react-native';
import { LightTheme, DarkTheme } from '../styles/themes.ts';

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
        const subscription = Appearance.addChangeListener(({ colorScheme }) => {
            setTheme(colorScheme === 'dark' ? DarkTheme : LightTheme);
        });

        return () => subscription.remove();
    }, []);

    const toggleTheme = () => {
        setTheme((prev) => (prev.dark ? LightTheme : DarkTheme));
    };

    return (
        <ThemeContext.Provider value={{ theme, toggleTheme }}>
            {children}
        </ThemeContext.Provider>
    );
};

export const useTheme = () => useContext(ThemeContext);
