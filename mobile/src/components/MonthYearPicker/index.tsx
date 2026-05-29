import React, {useMemo} from 'react';
import {Text, View,} from 'react-native';
import {getMonthYearPickerStyle} from "./styles";
import {useTheme} from "../../contexts/ThemeContext.tsx";
import {Picker} from "@react-native-picker/picker";

interface MonthYearSelectorProps {
    text: string;
    month: number;
    year: number;
    onMonthChange: (month: number) => void;
    onYearChange: (year: number) => void;
    minYear?: number;
    maxYear?: number;
}

const MONTHS = [
    {label: 'Janeiro', value: 0},
    {label: 'Fevereiro', value: 1},
    {label: 'Março', value: 2},
    {label: 'Abril', value: 3},
    {label: 'Maio', value: 4},
    {label: 'Junho', value: 5},
    {label: 'Julho', value: 6},
    {label: 'Agosto', value: 7},
    {label: 'Setembro', value: 8},
    {label: 'Outubro', value: 9},
    {label: 'Novembro', value: 10},
    {label: 'Dezembro', value: 11},
];

export default function MonthYearSelector({
                                              text,
                                              month,
                                              year,
                                              onMonthChange,
                                              onYearChange,
                                              minYear = 2000,
                                              maxYear = new Date().getFullYear() + 5,
                                          }: MonthYearSelectorProps) {
    const {theme} = useTheme();
    const monthYearPickerStyle = getMonthYearPickerStyle(theme);

    const years = useMemo(() => {
        const result: number[] = [];

        for (let y = maxYear; y >= minYear; y--) {
            result.push(y);
        }

        return result;
    }, [minYear, maxYear]);

    return (
        <View style={monthYearPickerStyle.container}>
            <Text style={monthYearPickerStyle.label}>{text}</Text>
            <View style={monthYearPickerStyle.fields}>
                <View style={monthYearPickerStyle.field}>
                    <Picker
                        style={monthYearPickerStyle.picker}
                        selectedValue={month}
                        onValueChange={onMonthChange}
                    >
                        {MONTHS.map(item => (
                            <Picker.Item
                                key={item.value}
                                label={item.label}
                                value={item.value}
                            />
                        ))}
                    </Picker>
                </View>

                <View style={monthYearPickerStyle.field}>
                    <Picker
                        style={monthYearPickerStyle.picker}
                        selectedValue={year}
                        onValueChange={onYearChange}
                    >
                        {years.map(item => (
                            <Picker.Item
                                key={item}
                                label={item.toString()}
                                value={item}
                            />
                        ))}
                    </Picker>
                </View>
            </View>
        </View>
    );
}
