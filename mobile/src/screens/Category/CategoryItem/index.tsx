import React from "react";
import {Text, View} from "react-native";
import DoneIcon from "../../../assets/done.svg";

import {constants} from "../../../constants";
import * as I from "../../../interfaces/interfaces";

import {useTheme} from "../../../contexts/ThemeContext";
import SwipeItem from "../../../components/SwipeItem";
import {getCardStyle} from "../../../styles/styles.card";

interface CategoryItemParms {
    data: I.Category,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const CategoryItem = (props: CategoryItemParms) => {
    const {theme} = useTheme();
    const cardStyle = getCardStyle(theme);

    return (
        <SwipeItem
            onPress={() => props.onPress(props.data)}
            onSwipeLeft={props.onSwipeLeft ? () => props.onSwipeLeft(props.data) : undefined}
            onSwipeRight={props.onSwipeRight ? () => props.onSwipeRight(props.data) : undefined}
        >
            <View style={cardStyle.rowInfo}>
                <Text style={cardStyle.textName}>
                    {props.data.Name}
                </Text>
            </View>
            <View style={cardStyle.rowFooter}>
                <Text style={cardStyle.textFooter}>
                    {props.data.Type === constants.categoryType.account.Id ? "Conta" : "Operação"}
                </Text>
                <DoneIcon width="20" height="20" fill={(props.data.Status === constants.status.active.Id) ? theme.colors.tertiaryIcon : theme.colors.secondaryIcon}/>
            </View>
        </SwipeItem>
    );
}

export default CategoryItem;