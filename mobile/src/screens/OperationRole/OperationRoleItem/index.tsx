import React, {useState} from "react";
import {Text, View} from "react-native";

import * as I from "../../../interfaces/interfaces.tsx";
import { useTheme } from '../../../contexts/ThemeContext.tsx';
import {getCardStyle} from "../../../styles/styles.card"

interface OperationRoleItemProps {
    data: I.OperationRole,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const OperationRoleItem = (props: OperationRoleItemProps) => {
    const { theme } = useTheme();
    const cardStyle = getCardStyle(theme);

    const [touchX, setTouchX] = useState(0);
    const [touchY, setTouchY] = useState(0);
    const [moveX, setMoveX] = useState(0);
    const [moveY, setMoveY] = useState(0);
    const [executeSwipe, setExecuteSwipe] = useState(false);

    const executeSwipeLeft = (move: number) => {
        if (moveX <= 40) {
            setMoveX(move);
        } else if (moveX > 40 && !executeSwipe) {
            setExecuteSwipe(true);
            props.onSwipeLeft(props.data);
        }
    };

    const executeSwipeRight = (move: number) => {
        if (moveX >= -40) {
            setMoveX(move);
        } else if (moveX < -40 && !executeSwipe) {
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
    
    return(
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
                <View style={cardStyle.rowFooter}>
                    <Text style={cardStyle.textFooter}>
                        {props.data.Name}
                    </Text>
                </View>
            </View>
        </View>
    );
}

export default OperationRoleItem;