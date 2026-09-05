import React from "react";
import {Text, View} from "react-native";

import {useTheme} from "../../../contexts/ThemeContext";
import {getCardStyle} from "../../../styles/styles.card";

export interface SelectItemRow {
    Id: number | string;
    Name: string;
}

interface SelectItemProps {
    item: SelectItemRow,
    onPress: (item: SelectItemRow) => void,
}

const SelectItem = (props: SelectItemProps) => {
    const { theme } = useTheme();
    const cardStyle = getCardStyle(theme);
    
    return (
        <View
            style={cardStyle.cardBackground}>
            <View
                style={cardStyle.cardNarrow}
                onTouchEndCapture={props.onPress}>
                <View style={cardStyle.rowFooter}>
                    <Text style={cardStyle.textName}>
                        {props.item.Name}
                    </Text>
                </View>
            </View>
        </View>
    );
}

export default SelectItem;