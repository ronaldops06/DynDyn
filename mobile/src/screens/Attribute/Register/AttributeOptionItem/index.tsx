import React from "react";
import {Text, View} from "react-native";

import * as I from "../../../../interfaces/interfaces.tsx";
import {constants} from "../../../../constants";
import {useTheme} from "../../../../contexts/ThemeContext";
import Tag from "../../../../components/Tag";
import {getDescriptionStatus} from "../../../../utils.ts";
import {getAttributeOptionItemStyle} from "./styles";

interface AttributeOptionItemProps {
    data: I.AttributeOption;
    onEdit: (option: I.AttributeOption) => void;
    onDelete: (option: I.AttributeOption) => void;
}

const AttributeOptionItem = ({data, onEdit, onDelete}: AttributeOptionItemProps) => {
    const {theme} = useTheme();
    const styles = getAttributeOptionItemStyle(theme);
    const isActive = data.Status === constants.status.active.Id;

    return (
        <View style={styles.card} onTouchEndCapture={() => onEdit(data)}>
            <Text style={styles.label} numberOfLines={1}>{data.Label}</Text>
            {data.IsDefault === 1 &&
                <Tag
                    text="Default"
                    icon=""
                    color={theme.colors.tertiaryBaseColor + "72"}
                    textColor={theme.colors.primaryTextColor}
                />
            }
            <Tag
                text={getDescriptionStatus(data.Status)}
                icon=""
                color={theme.colors.tertiaryBaseColor + "72"}
                textColor={isActive ? theme.colors.sextenaryTextColor : theme.colors.dangerTextColor}
            />
        </View>
    );
};

export default AttributeOptionItem;
