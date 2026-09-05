import React, {useState} from "react";
import {View} from "react-native";
import DateTimePicker from "@react-native-community/datetimepicker";
import Moment from "moment";
import ClockIcon from "../../assets/clock.svg";
import TodayIcon from "../../assets/today.svg";
import TextInput from "../CustomTextInput";

import {useTheme} from "../../contexts/ThemeContext";
import {getDateTimeInputStyle} from "./styles";

interface DateTimeInputProps {
    dateLabel?: string;
    dateValue: string;
    setDateValue: (value: string) => void;
    timeLabel?: string;
    timeValue?: string;
    setTimeValue?: (value: string) => void;
    showTime?: boolean;
}

const DateTimeInput = ({
    dateLabel = "Data",
    dateValue,
    setDateValue,
    timeLabel = "Hora",
    timeValue = "",
    setTimeValue,
    showTime = false
}: DateTimeInputProps) => {
    const {theme} = useTheme();
    const styles = getDateTimeInputStyle(theme);

    const [showPicker, setShowPicker] = useState(false);
    const [mode, setMode] = useState<"date" | "time">("date");

    const getPickerValue = () => {
        const [day, month, year] = dateValue.split("/").map(Number);
        const [hour, minute, second] = timeValue.split(":").map(Number);
        const date = new Date(
            year,
            (month || 1) - 1,
            day || 1,
            hour || 0,
            minute || 0,
            second || 0
        );

        return isNaN(date.getTime()) ? new Date() : date;
    };

    const handlePickerClick = (pickerMode: "date" | "time") => {
        setMode(pickerMode);
        setShowPicker(true);
    };

    const handleChange = (_event: any, date?: Date) => {
        setShowPicker(false);

        if (!date)
            return;

        if (mode === "date")
            setDateValue(Moment(date).format("DD/MM/YYYY"));

        if (mode === "time" && setTimeValue)
            setTimeValue(Moment(date).format("HH:mm:ss"));
    };

    return (
        <View style={showTime ? styles.areaDateTime : undefined}>
            <TextInput
                text={dateLabel}
                isMoveText={false}
                value={dateValue}
                setValue={setDateValue}
                icon={<TodayIcon width={30} fill={theme.colors.quintenaryIcon}/>}
                onPressIcon={() => handlePickerClick("date")}
                width={showTime ? "49%" : "100%"}
                keyboardType="numeric"
            />

            {showTime &&
                <TextInput
                    text={timeLabel}
                    isMoveText={false}
                    value={timeValue}
                    setValue={setTimeValue ?? (() => {})}
                    icon={<ClockIcon width={30} fill={theme.colors.quintenaryIcon}/>}
                    onPressIcon={() => handlePickerClick("time")}
                    width="49%"
                    keyboardType="numeric"
                />}

            {showPicker &&
                <DateTimePicker
                    testID="dateTimePicker"
                    value={getPickerValue()}
                    mode={mode}
                    is24Hour={true}
                    display="default"
                    onChange={handleChange}
                />}
        </View>
    );
};

export default DateTimeInput;
