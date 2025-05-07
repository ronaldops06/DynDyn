import Moment from 'moment';
import React, {useState} from 'react';
import {Text, View} from 'react-native';

import {TypesTransaction} from '../../../enums/enums';
import * as I from '../../../interfaces/interfaces';
import DoneIcon from '../../../assets/done.svg';
import MoneyInIcon from '../../../assets/money_in.svg';
import MoneyOutIcon from '../../../assets/money_out.svg';
import MoneyTransfIcon from '../../../assets/money_transf.svg';

import {transactionItemStyle} from './styles';

interface TransactionItemParms {
    data: I.Transaction,
    onPress: any,
    onSwipeLeft?: any,
    onSwipeRight?: any
}

const TransactionItem = React.memo((props: TransactionItemParms) => {

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
        
        if ((moveX > -5 && moveX < 5) && (moveY > -1 && moveY < 1)){
            props.onPress(props.data)
        }
        setMoveX(0);
        setMoveY(0);
    };

    return (
        <View
            key={props.data.Id.toString()}
            style={transactionItemStyle.cardBackground}>
            <View
                style={[transactionItemStyle.card, {marginLeft: moveX * -1, marginRight: moveX}]}
                onTouchStart={e => {
                    setTouchX(e.nativeEvent.pageX); 
                    setTouchY(e.nativeEvent.pageY);
                }}
                onTouchEnd={e => onTouchEnd(e)}
                onTouchCancel={e => onTouchEnd(e)}
                onTouchMove={e => onTouchMove(e)}
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
                        <Text
                            style={transactionItemStyle.textHeader}>{Moment(props.data.DataCriacao).format('DD/MM')}</Text>
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
                        <DoneIcon width="20" height="20" fill={(props.data.Consolidated) ? "#00A519" : "#A4BCE3"}/>
                    </View>
                </View>
            </View>
        </View>
    );
});

export default TransactionItem;