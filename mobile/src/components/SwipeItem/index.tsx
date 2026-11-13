import React, {ReactNode, useState} from "react";
import {NativeSyntheticEvent, NativeTouchEvent, View} from "react-native";
import TrashIcon from "../../assets/trash.svg";

import {useTheme} from "../../contexts/ThemeContext";
import {getCardStyle} from "../../styles/styles.card";
import {StyleProp} from "react-native/Libraries/StyleSheet/StyleSheet";
import {ViewStyle} from "react-native/Libraries/StyleSheet/StyleSheetTypes";

interface SwipeItemProps {
    key?: string | undefined;
    children: ReactNode;
    onPress: () => void;
    onSwipeLeft?: () => void;
    onSwipeRight?: () => void;
    style?: StyleProp<ViewStyle> | undefined; 
}

const SwipeItem = (props: SwipeItemProps) => {
    const {theme} = useTheme();
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
            props.onSwipeLeft?.();
        }
    };

    const executeSwipeRight = (move: number) => {
        if (moveX >= -40) {
            setMoveX(move);
        } else if (moveX < -40 && !executeSwipe) {
            setExecuteSwipe(true);
            props.onSwipeRight?.();
        }
    };

    const onTouchMove = (event: NativeSyntheticEvent<NativeTouchEvent>) => {
        const moveX = touchX - event.nativeEvent.pageX;
        const moveY = touchY - event.nativeEvent.pageY;

        if (moveX >= 0) {
            executeSwipeLeft(moveX);
        } else {
            executeSwipeRight(moveX);
        }

        setMoveY(moveY);
    };

    const onTouchEnd = () => {
        setExecuteSwipe(false);

        if (moveX > -5 && moveX < 5 && moveY > -1 && moveY < 1) {
            props.onPress();
        }

        setMoveX(0);
        setMoveY(0);
    };

    return (
        <View key={props.key ? props.key : ""} style={props.style ? props.style : cardStyle.cardBackground}>
            {moveX > 0 &&
                <View
                    pointerEvents="none"
                    style={{
                        position: "absolute",
                        top: 0,
                        right: 0,
                        bottom: 0,
                        width: 64,
                        alignItems: "center",
                        justifyContent: "center",
                        borderRadius: 8,
                        backgroundColor: theme.colors.dangerBaseColor
                    }}
                >
                    <TrashIcon width={24} height={24} fill={theme.colors.primaryIcon}/>
                </View>
            }
            <View
                style={[cardStyle.card, {marginLeft: moveX * -1, marginRight: moveX}]}
                onTouchStart={event => {
                    setTouchX(event.nativeEvent.pageX);
                    setTouchY(event.nativeEvent.pageY);
                }}
                onTouchEnd={onTouchEnd}
                onTouchCancel={onTouchEnd}
                onTouchMove={onTouchMove}
            >
                {props.children}
            </View>
        </View>
    );
};

export default SwipeItem;
