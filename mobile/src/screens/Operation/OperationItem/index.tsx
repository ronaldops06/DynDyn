import * as I from "../../../interfaces/interfaces.tsx";
import React, {useState} from "react";
import {Text, View} from "react-native";
import DoneIcon from "../../../assets/done.svg";
import {constants} from "../../../constants";
import {operationItemStyle} from "./styles";

interface OperationItemProps {
    data: I.Operation,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const OperationItem = (props: OperationItemProps) => {

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
            style={operationItemStyle.cardBackground}>
            <View
                style={[operationItemStyle.card, {marginLeft: moveX * -1, marginRight: moveX}]}
                // onTouchEndCapture={() => onTouchEnd}
                onTouchStart={e => setTouchX(e.nativeEvent.pageX)}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}
            >
                <View style={operationItemStyle.rowInfo}>
                    <Text style={operationItemStyle.textName}>{props.data.Name}</Text>
                </View>
                <View style={operationItemStyle.rowFooter}>
                    <Text style={operationItemStyle.textFooter} >
                        {props.data.Category.Name}
                    </Text>
                    <Text style={operationItemStyle.textFooter}>
                        {props.data.Recurrent ? "Recorrente" : "Esporádico"}
                    </Text>
                    <Text style={operationItemStyle.textFooter}>
                        {props.data.Salary ? "Salário" : ""}
                    </Text>
                    <DoneIcon width="20" height="20" fill={(props.data.Status === constants.status.active.Id) ? "#00A519" : "#A4BCE3"}/>
                </View>
            </View>
        </View>
    );
}

export default OperationItem;