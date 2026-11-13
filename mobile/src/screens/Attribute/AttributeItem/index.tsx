import React from "react";
import {Text, View} from "react-native";
import Icon from "../../../components/Icon";
import Tag from "../../../components/Tag";

import * as I from "../../../interfaces/interfaces";
import {constants} from "../../../constants";
import {getDescriptionStatus} from "../../../utils";

import {useTheme} from '../../../contexts/ThemeContext';
import {getCardStyle} from "../../../styles/styles.card";

interface AttributeItemParms {
    data: I.Attribute,
    onPress: any
}

const AttributeItem = (props: AttributeItemParms) => {
    const {theme} = useTheme();
    const cardStyle = getCardStyle(theme);

    const getFormatedDescription = () => {
        return `${props.data.Description?.substring(0, 42)}${props.data.Description?.length > 42 ? '...' : ''}`;
    }

    const getDescriptionDataType = (dataTypeId: number) => {
        return Object.values(constants.attributeDataType)?.find(x => x.Id === dataTypeId)?.Name;
    }
    
    return (
        <View
            style={cardStyle.cardBackground}>
            <View
                style={cardStyle.cardHorizontal}
                onTouchEndCapture={() => props.onPress(props.data)}
            >
                <View style={cardStyle.areaContent}>
                    <View style={cardStyle.rowInfo}>
                        <Text style={cardStyle.textName}>
                            {props.data.Name}
                        </Text>
                        <Tag
                            text={getDescriptionStatus(props.data.Status)}
                            icon=""
                            color={theme.colors.tertiaryBaseColor + "21"}
                            textColor={props.data.Status === constants.status.active.Id ? theme.colors.sextenaryTextColor : theme.colors.dangerTextColor}
                        />
                    </View>
                    {props.data.Description &&
                        <View style={cardStyle.rowFooter}>
                            <Text style={cardStyle.textFooter}>
                                {getFormatedDescription()}
                            </Text>
                        </View>
                    }
                    <View style={cardStyle.rowFooter}>
                        <Tag
                            text={getDescriptionDataType(props.data.DataType)}
                            icon={getDescriptionDataType(props.data.DataType).toLowerCase()}
                            color={theme.colors.tertiaryBaseColor + "21"}
                            textColor={theme.colors.primaryTextColor}
                        />

                    </View>
                </View>
                <View>
                    <Icon name="next" size={35} color={theme.colors.quaternaryIcon}/>
                </View>
            </View>
        </View>
    );
}

export default AttributeItem;