import Moment from 'moment';
import React, {useRef, useState} from 'react';
import {GestureResponderEvent, Pressable, Text, View} from 'react-native';

import {TypesTransaction} from '../../../enums/enums';
import * as I from '../../../interfaces/interfaces';
import DoneIcon from '../../../assets/done.svg';
import MoneyInIcon from '../../../assets/money_in.svg';
import MoneyOutIcon from '../../../assets/money_out.svg';
import MoneyTransfIcon from '../../../assets/money_transf.svg';
import WarningIcon from '../../../assets/warning.svg';
import EventBusyIcon from '../../../assets/event_busy.svg';

import {useTheme} from '../../../contexts/ThemeContext';
import {getTransactionItemStyle} from './styles';
import moment from "moment/moment";

interface TransactionItemParms {
    data: I.Transaction,
    isSelectionMode: boolean,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any,
    onLongPress?: any
}

const TransactionItem = React.memo((props: TransactionItemParms) => {
    const {theme} = useTheme();
    const transactionItemStyle = getTransactionItemStyle(theme);

    const [touchX, setTouchX] = useState(0);
    const [touchY, setTouchY] = useState(0);
    const [moveX, setMoveX] = useState(0);
    const [moveY, setMoveY] = useState(0);
    const [executeSipe, setExecuteSwipe] = useState(false);

    const longPressTimeout = useRef<ReturnType<typeof setTimeout> | null>(null);
    const longPressTriggered = useRef(false);

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

    const onTouchStart = (e: GestureResponderEvent) => {
        setTouchX(e.nativeEvent.pageX);
        setTouchY(e.nativeEvent.pageY);
        longPressTriggered.current = false;

        // Inicia o temporizador de toque longo
        longPressTimeout.current = setTimeout(() => {
            longPressTriggered.current = true;
            props.onLongPress?.(props.data);
        }, 1000);
    };

    const onTouchMove = (e: GestureResponderEvent) => {
        const deltaX = e.nativeEvent.pageX - touchX;
        const deltaY = e.nativeEvent.pageY - touchY;

        // Cancela o toque longo se houver movimento significativo
        if (Math.abs(deltaX) > 5 || Math.abs(deltaY) > 5) {
            if (longPressTimeout.current) {
                clearTimeout(longPressTimeout.current);
                longPressTimeout.current = null;
            }
        }
        
        if  (!props.isSelectionMode) {
            if (deltaX >= 0) {
                executeSwipeLeft(deltaX);
            } else {
                executeSwipeRight(deltaX);
            }
        }

        setMoveY(deltaY);
        if (!props.isSelectionMode)
            setMoveX(deltaX);
    };


    const onTouchEnd = (e: GestureResponderEvent) => {
        setExecuteSwipe(false);

        // Cancela o timer se ainda não disparou
        if (longPressTimeout.current) {
            clearTimeout(longPressTimeout.current);
            longPressTimeout.current = null;
        }

        // Só executa o clique se não foi long press e não houve movimento
        if (
            !longPressTriggered.current &&
            moveX > -5 &&
            moveX < 5 &&
            moveY > -1 &&
            moveY < 1
        ) {
            props.onPress?.(props.data);
        }

        setMoveX(0);
        setMoveY(0);
    };

    /*const onTouchMove = (e: any) => {
        let auxMoveX = touchX - e.nativeEvent.pageX;
        let auxMoveY = touchY - e.nativeEvent.pageY;

        if (auxMoveX >= 0) {
            executeSwipeLeft(auxMoveX);
        } else {
            executeSwipeRight(auxMoveX);
        }

        setMoveY(auxMoveY);
    };

    const onTouchEnd = async (e: any) => {
        setExecuteSwipe(false);

        if ((moveX > -5 && moveX < 5) && (moveY > -1 && moveY < 1)) {
            props.onPress(props.data)
        }
        setMoveX(0);
        setMoveY(0);
    };*/

    const renderDateAlert = (data: I.Transaction) => {
        let dateNow = new Date(moment().utc(true).format('YYYY-MM-DDTHH:mm:ss.SSS'));
        let alertDate = new Date(moment(dateNow.setDate(dateNow.getDate() - 5)).format('YYYY-MM-DDTHH:mm:ss.SSS'));

        let creationDate = data.DataCriacao;

        return (
            <>
                {!data.Consolidated && creationDate <= alertDate && creationDate > dateNow &&
                    <WarningIcon width="20" height="20" fill={theme.colors.sextenaryIcon}/>}
                {!data.Consolidated && creationDate <= dateNow &&
                    <EventBusyIcon width="20" height="20" fill={theme.colors.septenaryIcon}/>}
            </>
        );
    }

    return (
        <Pressable
            key={props.data.InternalId.toString()}
            style={transactionItemStyle.cardBackground}>
            <View
                style={[transactionItemStyle.card, {
                    marginLeft: moveX,
                    marginRight: moveX * -1
                }, props.data.IsSelectedItem ? transactionItemStyle.cardSelected : null]}
                /*onTouchStart={e => {
                    setTouchX(e.nativeEvent.pageX);
                    setTouchY(e.nativeEvent.pageY);
                }}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}*/
                onTouchStart={onTouchStart}
                onTouchMove={onTouchMove}
                onTouchEnd={onTouchEnd}
                onTouchCancel={onTouchEnd}

            >
                <View style={transactionItemStyle.cardType}>
                    {props.data.Operation?.Type === 1 ?
                        <MoneyInIcon width="35" height="35"/> :
                        props.data.Operation?.Type === 2 ?
                            <MoneyOutIcon width="35" height="35"/> :
                            <MoneyTransfIcon width="35" height="35"/>}
                </View>
                <View style={transactionItemStyle.cardContent}>
                    <View style={transactionItemStyle.rowHeader}>
                        <View style={transactionItemStyle.rowHeaderDate}>
                            <Text
                                style={transactionItemStyle.textHeader}>{Moment(props.data.DataCriacao).format('DD/MM')}</Text>
                            {renderDateAlert(props.data)}
                        </View>
                        <Text
                            style={transactionItemStyle.textHeader}>{props.data.TotalInstallments > 1 && props.data.Installment + "/" + props.data.TotalInstallments}</Text>
                        {/* <InfoIcon width="20" height="20" fill="#CCC84E" /> */}
                    </View>
                    <View style={transactionItemStyle.rowInfo}>
                        <Text style={transactionItemStyle.textTransactionName}>
                            {props.data.Operation?.Name}
                        </Text>
                        <Text style={(props.data.Operation?.Type === 1) ?
                            transactionItemStyle.textTransactionValue :
                            (props.data.Operation?.Type === 2) ?
                                transactionItemStyle.textTransactionValueExpense :
                                transactionItemStyle.textTransactionValueTransfer}>
                            R$ {props.data.Value?.toFixed(2)}
                        </Text>
                    </View>
                    <View style={transactionItemStyle.rowFooter}>
                        <Text style={transactionItemStyle.textFooter}>
                            {props.data.Portfolio?.Name}
                            {props.data.Operation?.Type == TypesTransaction.Transference && " para " + props.data.DestinationPortfolio?.Name}
                        </Text>
                        <DoneIcon width="20" height="20"
                                  fill={(props.data.Consolidated) ? theme.colors.tertiaryIcon : theme.colors.secondaryIcon}/>
                    </View>
                </View>
            </View>
        </Pressable>
    );
});

export default TransactionItem;