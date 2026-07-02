import * as I from "../../../interfaces/interfaces.tsx";
import {Text, TouchableOpacity, View} from "react-native";
import MoreIcon from "../../../assets/more.svg";
import {VictoryAxis, VictoryChart, VictoryLine, VictoryTheme} from "victory-native";
import React, {ReactNode, useState} from "react";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getDashboardStyle} from "../styles";
import {getStyle} from "../../../styles/styles.ts";
import CenterModal from "../../../components/Modal/CenterModal";

interface WidgetLineProps {
    description: string;
    data: I.DashboardItem[];
    renderFilters?: ReactNode;
    onCloseFilters?: () => void;
}

const WidgetLine = (props: WidgetLineProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const dashboardStyle = getDashboardStyle(theme);
    
    const [showModalFilter, setShowModalFilter] = useState<boolean>(false);
    
    if (!props.data) return;

    const handleCloseModal = () => {
        if (props.onCloseFilters)
            props.onCloseFilters();
        setShowModalFilter(false);
    }

    return(
        <View style={dashboardStyle.widgetArea}>
            <View style={dashboardStyle.widgetHeader}>
                <Text style={style.textPrimary18}>{props.description}</Text>
                <TouchableOpacity style={dashboardStyle.widgetHeaderMore}
                                  onPress={() => setShowModalFilter(true)}>
                    <MoreIcon width="25" height="25" fill={theme.colors.primaryIcon}/>
                </TouchableOpacity>
            </View>
            <VictoryChart theme={VictoryTheme.material}
                          domainPadding={{x: 30, y: [20, 20]}}>
                <VictoryAxis
                    style={{
                        tickLabels: {
                            fontSize: 14,
                            padding: 5,
                            angle: -90,
                            fill: theme.colors.primaryTextColor,
                            textAnchor: 'end',
                            verticalAnchor: 'start',
                            dy: -10,
                        }
                    }}
                />
                <VictoryAxis
                    dependentAxis
                    tickFormat={(x) => `${x}`}
                    style={{
                        tickLabels: {fontSize: 12, padding: 5},
                    }}
                />

                <VictoryLine
                    data={props.data}
                    x="Label"
                    y="Value"
                    style={{
                        data: {stroke: theme.colors.secondaryMonetaryColor, strokeWidth: 3},
                    }}
                    interpolation="monotoneX"
                />

            </VictoryChart>
            <CenterModal 
                show={showModalFilter} 
                setShow={handleCloseModal} 
                textButtonOk="Aplicar" 
                onPressOk={handleCloseModal}>
                {props.renderFilters}
            </CenterModal>
        </View>
    );
}

export default WidgetLine;