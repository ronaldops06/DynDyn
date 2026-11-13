import React, {useCallback, useState} from "react";
import {Text, TouchableOpacity, View} from "react-native";
import {useFocusEffect} from "@react-navigation/native";
import {StyleProp} from "react-native/Libraries/StyleSheet/StyleSheet";
import {ViewStyle} from "react-native/Libraries/StyleSheet/StyleSheetTypes";
import BottomModal from "../BottomModal";
import Button from "../Button";

import {useTheme} from "../../contexts/ThemeContext";
import {getStyle} from "../../styles/styles";
import {getStyleCadastro} from "../../styles/styles.cadastro";
import {getCalculatorStyle} from "./style";

interface CalculatorProps {
    value: number;
    setValue: (value: number) => void;
    show: boolean;
    setShow: (value: boolean) => void;
}

const Calculator = (props: CalculatorProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);
    const calculatorStyle = getCalculatorStyle(theme);

    const [value, setValue] = useState("");

    useFocusEffect(
        useCallback(() => {
            setValue(props.value.toString());
        }, [props.show === true])
    );

    const precedence = {
        '+': 1,
        '-': 1,
        '*': 2,
        '/': 2,
        '%': 3
    };

    const operators = ['+', '-', '*', '/', ','];

    const appendValue = (newValue: string): void => {
        let finalValue = (value === '0') ? newValue : value + newValue;

        setValue(finalValue);
    }
    
    const appendOperator = (operator: string): void => {
        let values = value.split("");
        let lastValue = values[values.length - 1];
        let finalValue = "";

        if (operators.includes(lastValue))
            finalValue = value.slice(0, -1) + operator;
        else
            finalValue = value + operator;

        setValue(finalValue);
    }

    const appendParentheses = (): void => {
        let left = value.split("(").length - 1;
        let right = value.split(")").length - 1;
        let finalValue = value;

        if (left === right)
            finalValue += "(";
        else
            finalValue += ")";

        setValue(finalValue);
    }
    
    function calculate() {
        try {
            const expr = value.replace(/,/g, '.');
            const tokens = tokenize(expr);
            const rpn = toRPN(tokens);
            const result = evaluateRPN(rpn);
            const resultFormated = result.toString().replace('.', ',');
            setValue(resultFormated);
        } catch (error) {
            setValue('Erro: ' + error.message);
        }
    }

    const tokenize = (expr: string) => {
        return expr.match(/(\d+\.?\d*|\+|\-|\*|\/|\%|\(|\))/g)
            ?.map(t => isNaN(t) ? t : parseFloat(t));
    }

    const toRPN = (tokens) => {
        const output = [];
        const operators = [];

        for (const token of tokens) {
            if (typeof token === 'number') {
                output.push(token);
            } else if (token in precedence) {
                while (
                    operators.length > 0 &&
                    operators[operators.length - 1] in precedence &&
                    precedence[operators[operators.length - 1]] >= precedence[token]
                    ) {
                    output.push(operators.pop());
                }
                operators.push(token);
            } else if (token === '(') {
                operators.push(token);
            } else if (token === ')') {
                let foundLeftParen = false;

                while (operators.length > 0) {
                    const op = operators.pop();

                    if (op === '(') {
                        foundLeftParen = true;
                        break;
                    }

                    output.push(op);
                }

                if (!foundLeftParen) {
                    throw new Error("Parênteses desbalanceados");
                }
            }
        }

        while (operators.length > 0) {
            const op = operators.pop();

            if (op === '(' || op === ')') {
                throw new Error("Parênteses desbalanceados");
            }

            output.push(op);
        }

        return output;
    };

    const evaluateRPN = (rpn) => {
        const stack = [];

        for (let i = 0; i < rpn.length; i++) {
            const token = rpn[i];

            if (typeof token === 'number') {
                stack.push(token);
            } else if (token === '%') {
                const percent = stack.pop();

                // Se tiver base → percentual relativo
                if (stack.length > 0) {
                    const base = stack[stack.length - 1];

                    // Olha operador seguinte
                    const nextOperator = rpn[i + 1];

                    if (nextOperator === '+' || nextOperator === '-') {
                        stack.push((base * percent) / 100);
                    }
                    else {
                        stack.push(percent / 100);
                    }
                }
                else {
                    stack.push(percent / 100);
                }
            } else {
                const b = stack.pop();
                const a = stack.pop();

                switch (token) {
                    case '+':
                        stack.push(a + b);
                        break;
                    case '-':
                        stack.push(a - b);
                        break;
                    case '*':
                        stack.push(a * b);
                        break;
                    case '/':
                        stack.push(a / b);
                        break;
                }
            }
        }

        return stack[0];
    }
    
    const _renderButton = (text: string, onPress: any, extraStyleButton?: StyleProp<ViewStyle> | undefined, extraStyleText?: StyleProp<ViewStyle> | undefined) => {

        return (
            <TouchableOpacity
                style={[calculatorStyle.operationButton, extraStyleButton]}
                onPress={onPress}
            >
                <Text style={[style.textPrimary28Bold, extraStyleText]}>{text}</Text>
            </TouchableOpacity>
        );
    }

    const handleApply = () => {
        props.setValue(parseFloat(value.replace(',', '.')));
        props.setShow(false);
    }

    return (
        <BottomModal
            show={props.show}
            setShow={props.setShow}>
            <View style={calculatorStyle.areaValueDefault}><Text style={style.textPrimary18}>Valor
                Base: {props.value}</Text></View>
            <View style={calculatorStyle.areaResult}><Text style={style.textPrimary28Bold}>{value}</Text></View>
            <View style={calculatorStyle.areaOperation}>
                <View style={calculatorStyle.areaOperationRow}>
                    {_renderButton("C", () => setValue(""), calculatorStyle.buttonBackspace, calculatorStyle.textBackspace)}
                    {_renderButton("()", () => appendParentheses(), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                    {_renderButton("%", () => appendOperator("%"), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                    {_renderButton("/", () => appendOperator("/"), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                </View>
                <View style={calculatorStyle.areaOperationRow}>
                    {_renderButton("7", () => appendValue("7"))}
                    {_renderButton("8", () => appendValue("8"))}
                    {_renderButton("9", () => appendValue("9"))}
                    {_renderButton("X", () => appendOperator("*"), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                </View>
                <View style={calculatorStyle.areaOperationRow}>
                    {_renderButton("4", () => appendValue("4"))}
                    {_renderButton("5", () => appendValue("5"))}
                    {_renderButton("6", () => appendValue("6"))}
                    {_renderButton("-", () => appendOperator("-"), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                </View>
                <View style={calculatorStyle.areaOperationRow}>
                    {_renderButton("1", () => appendValue("1"))}
                    {_renderButton("2", () => appendValue("2"))}
                    {_renderButton("3", () => appendValue("3"))}
                    {_renderButton("+", () => appendOperator("+"), calculatorStyle.buttonOperator, calculatorStyle.textOperator)}
                </View>
                <View style={calculatorStyle.areaOperationRow}>
                    {_renderButton("0", () => appendValue("0"))}
                    {_renderButton(",", () => appendOperator(","))}
                    {_renderButton("⌫", () => {setValue(value.slice(0, -1))}, calculatorStyle.buttonBackspace, calculatorStyle.textBackspace)}
                    {_renderButton("=", () => calculate(), calculatorStyle.buttonCalculate, calculatorStyle.textCalculate)}
                </View>
            </View>
            <View style={styleCadastro.areaButtonSave}>
                <Button
                    label={"Cancelar"}
                    onPress={() => props.setShow(false)}
                    type={"secondary"}
                />
                <Button
                    label={"Aplicar"}
                    onPress={handleApply}
                />
            </View>
        </BottomModal>
    );
}

export default Calculator;