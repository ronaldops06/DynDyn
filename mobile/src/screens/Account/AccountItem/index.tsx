import * as I from "../../../interfaces/interfaces.tsx";
import React, {useState} from "react";
import {Text, View} from "react-native";
import DoneIcon from "../../../assets/done.svg";
import {constants} from "../../../constants";
import {cardStyle} from "../../../styles/styles.card";

interface AccountItemProps {
    data: I.Account,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const AccountItem = (props: AccountItemProps) => {

    const [touchX, setTouchX] = useState(0);
    const [moveX, setMoveX] = useState(0);
    const [executeSipe, setExecuteSwipe] = useState(false);
    
    const executeSwipeLeft = (move: number) => {
        if (moveX <= 40) {
            setMoveX(move);
        } else if (moveX > 40 && !executeSipe) {
            setExecuteSwipe(true);
            props.onSwipeLeft(props.data);
        }
    };

    const executeSwipeRight = (move: number) => {
        if (moveX >= -40) {
            setMoveX(move);
        } else if (moveX < -40 && !executeSipe) {
            setExecuteSwipe(true);
            props.onSwipeRight(props.data);
        }
    };

    const onTouchMove = (e: any) => {
        let move = touchX - e.nativeEvent.pageX;
        if (move >= 0) {
            executeSwipeLeft(move);
        } else {
            executeSwipeRight(move);
        }
    };

    const onTouchEnd = async (e: any) => {
        setExecuteSwipe(false);
        if (moveX > -5 && moveX < 5) {
            props.onPress(props.data)
        }
        setMoveX(0);
    };

    return (
        <View
            style={cardStyle.cardBackground}>
            <View
                style={[cardStyle.card, {marginLeft: moveX * -1, marginRight: moveX}]}
                onTouchStart={e => setTouchX(e.nativeEvent.pageX)}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}
            >
                <View style={cardStyle.rowHeader}>
                    <Text style={cardStyle.textHeader}>{props.data.ParentAccount?.Name}</Text>
                </View>
                <View style={cardStyle.rowInfo}>
                    <Text style={cardStyle.textName}>{props.data.Name}</Text>
                </View>
                <View style={cardStyle.rowFooter}>
                    <Text style={cardStyle.textFooter} >
                        {props.data.Category.Name}
                    </Text>
                    <DoneIcon width="20" height="20" fill={(props.data.Status === constants.status.active.Id) ? "#00A519" : "#A4BCE3"}/>
                </View>
            </View>
        </View>
    );
}

export default AccountItem;