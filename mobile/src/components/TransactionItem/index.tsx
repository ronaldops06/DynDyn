import Moment from 'moment';
import React, { useState } from 'react';
import { Text, View } from 'react-native';

import DoneIcon from '../../assets/done.svg';
import { TypesTransaction } from '../../enums/enums';
import * as I from '../../interfaces/interfaces';

import { transactionItemStyle } from './styles';

interface TransactionItemParms {
    data: I.Transaction,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const TransactionItem = (props: TransactionItemParms) => {

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
        setMoveX(0);
        if (moveX > -5 && moveX < 5) {
            props.onPress(props.data)
        }
    };

    return (
        <View
            style={transactionItemStyle.cardBackground}>
            <View
                style={[transactionItemStyle.card, { marginLeft: moveX * -1, marginRight: moveX }]}
                // onTouchEndCapture={() => onTouchEnd}
                onTouchStart={e => setTouchX(e.nativeEvent.pageX)}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}
            >
                <View style={transactionItemStyle.rowHeader}>
                    <Text style={transactionItemStyle.textHeader}>{Moment(props.data.dataCriacao).format('DD/MM/YYYY')}</Text>
                    <Text style={transactionItemStyle.textHeader}>{props.data.totalInstallments > 1 && props.data.installment + "/" + props.data.totalInstallments}</Text>
                    {/* <InfoIcon width="20" height="20" fill="#CCC84E" /> */}
                </View>
                <View style={transactionItemStyle.rowInfo}>
                    <Text style={(props.data.operation?.type == 1) ?
                        transactionItemStyle.textTransactionName :
                        (props.data.operation?.type == 2) ?
                            transactionItemStyle.textTransactionNameExpense :
                            transactionItemStyle.textTransactionNameTransfer}>
                        {props.data.operation?.name}
                    </Text>
                    <Text style={(props.data.operation?.type == 1) ?
                        transactionItemStyle.textTransactionValue :
                        (props.data.operation?.type == 1) ?
                            transactionItemStyle.textTransactionValueExpense :
                            transactionItemStyle.textTransactionValueTransfer}>
                        R$ {props.data.value?.toFixed(2)}
                    </Text>
                </View>
                <View style={transactionItemStyle.rowFooter}>
                    <Text style={transactionItemStyle.textFooter}>
                        {props.data.account?.name}
                        {props.data.operation?.type == TypesTransaction.Transference && " -->> " + props.data.destinationAccount?.name}
                    </Text>
                    <DoneIcon width="20" height="20" fill={(props.data.consolidated == 1) ? "#00A519" : "#A4BCE3"} />
                </View>
            </View>
        </View>
    );
}

export default TransactionItem;