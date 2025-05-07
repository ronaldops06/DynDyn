import * as I from "../../../interfaces/interfaces.tsx";
import React, {useState} from "react";
import {Text, View} from "react-native";
import {constants} from "../../../constants";
import {cardStyle} from "../../../styles/styles.card"
import DoneIcon from "../../../assets/done.svg";
import PaymentIcon from "../../../assets/payments.svg";
import CurrencyExchangeIcon from "../../../assets/currency_exchange.svg";

interface OperationItemProps {
    data: I.Operation,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const OperationItem = (props: OperationItemProps) => {

    const [touchX, setTouchX] = useState(0);
    const [touchY, setTouchY] = useState(0);
    const [moveX, setMoveX] = useState(0);
    const [moveY, setMoveY] = useState(0);
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
        let auxMoveY = touchY - e.nativeEvent.pageY;
        
        if (move >= 0) {
            executeSwipeLeft(move);
        } else {
            executeSwipeRight(move);
        }

        setMoveY(auxMoveY);
    };

    const onTouchEnd = async (e: any) => {
        setExecuteSwipe(false);
        
        if ((moveX > -5 && moveX < 5) && (moveY > -1 && moveY < 1)){
            props.onPress(props.data)
        }
        setMoveX(0);
        setMoveY(0);
    };

    return (
        <View
            style={cardStyle.cardBackground}>
            <View
                style={[cardStyle.card, {marginLeft: moveX * -1, marginRight: moveX}]}
                onTouchStart={e => {
                    setTouchX(e.nativeEvent.pageX);
                    setTouchY(e.nativeEvent.pageY);
                }}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}
            >
                <View style={cardStyle.rowInfo}>
                    <Text style={cardStyle.textName}>{props.data.Name}</Text>
                </View>
                <View style={cardStyle.rowFooter}>
                    <Text style={cardStyle.textFooter}>
                        {props.data.Category.Name}
                    </Text>
                    <View style={cardStyle.boxEnd}>
                        <CurrencyExchangeIcon style={{opacity: props.data.Recurrent ? 1 : 0}} width="20" height="20" fill="#00A519"/>
                        <PaymentIcon style={{opacity: props.data.Salary ? 1 : 0}} width="20" height="20" fill="#00A519"/>
                        <DoneIcon style={{opacity: 1}} width="20" height="20"
                                  fill={(props.data.Status === constants.status.active.Id) ? "#00A519" : "#A4BCE3"}/>
                    </View>
                </View>
            </View>
        </View>
    );
}

export default OperationItem;