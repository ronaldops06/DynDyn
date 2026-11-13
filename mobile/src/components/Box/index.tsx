import React from "react";
import { Text, View } from "react-native";
import {ViewStyle} from "react-native/Libraries/StyleSheet/StyleSheetTypes";
import {StyleProp} from "react-native/Libraries/StyleSheet/StyleSheet";

import { useTheme } from "../../contexts/ThemeContext";
import { getBoxStyle } from "./styles";

interface BoxProps {
    title: string;
    description?: string | undefined;
    isVisibleBorder?: boolean;
    borderType?: "solid" | "dashed";
    style?: StyleProp<ViewStyle> | undefined;
    children: React.ReactNode;
}

const Box = (props: BoxProps) => {
    const { theme } = useTheme();
    const boxStyle = getBoxStyle(theme);

    const {
        isVisibleBorder = true,
        borderType = "solid"
    } = props;

    const containerStyle = [
        boxStyle.container,
        !isVisibleBorder && boxStyle.noBorder,
        borderType === "solid" && boxStyle.solidBorder
    ];

    return (
        <View style={[containerStyle, props.style]}>
            <View style={boxStyle.header}>
                <View style={boxStyle.headerText}>
                    <Text style={boxStyle.title}>{props.title}</Text>
                    {props.description &&
                        <Text style={boxStyle.description}>{props.description}</Text>
                    }
                </View>
            </View>
            <View style={boxStyle.contentArea}>
                {props.children}
            </View>
        </View>
    );
};

export default Box;
