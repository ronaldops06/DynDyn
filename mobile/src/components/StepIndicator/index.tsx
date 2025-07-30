import React from 'react';
import {Text, View} from 'react-native';

import {useTheme} from '../../contexts/ThemeContext';
import {getStepIndicatorStyle} from './styles';

interface StepIndicatorProps {
    currentStep: number;
    steps: string[];
}

const StepIndicator = (props: StepIndicatorProps) => {
    const {theme} = useTheme();
    const stepIndicatorStyle = getStepIndicatorStyle(theme);

    const getStyleStep = (isActive, isCompleted, position) => {
        let styles = [];
        styles.push(stepIndicatorStyle.step);

        if (isActive) {
            styles.push(stepIndicatorStyle.stepActive);
        } else {
            if (isCompleted) {
                if (position === 1) {
                    styles.push(stepIndicatorStyle.firstStepCompleted);
                } else {
                    styles.push(stepIndicatorStyle.stepCompleted);
                }
                if (props.currentStep > position + 1) {
                    styles.push(stepIndicatorStyle.secondaryStepCompleted);
                }
            } else {
                styles.push(theme.colors.secondaryBaseColor);
            }
        }

        return styles;
    }

    const getBackgroundColor = (isActive, isCompleted) => {
        return isActive
            ? theme.colors.tertiaryBaseColor
            : isCompleted
                ? theme.colors.primaryBaseColor
                : theme.colors.secondaryBaseColor;
    }

    const getTextColor = (isCompleted) => {
        return isCompleted
            ? theme.colors.secondaryBaseColor
            : theme.colors.primaryBaseColor;
    }

    return (
        <View style={stepIndicatorStyle.container}>
            {props.steps.map((label, index) => {
                const position = index + 1;

                const isActive = position === props.currentStep;
                const isCompleted = position < props.currentStep;

                return (
                    <View y={index}
                          style={getStyleStep(isActive, isCompleted, position)}>
                        <View
                            style={[stepIndicatorStyle.stepNumber, {backgroundColor: getBackgroundColor(isActive, isCompleted)}]}>
                            <Text
                                style={[stepIndicatorStyle.stepNumberText, {color: getTextColor(isCompleted)}]}>{position}</Text>
                        </View>
                        <Text
                            style={[stepIndicatorStyle.stepText, {color: getTextColor(isCompleted)}]}>{label}</Text>
                    </View>
                );
            })}
        </View>
    );
}

export default StepIndicator;