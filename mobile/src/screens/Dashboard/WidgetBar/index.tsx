import React, {ReactNode, useState} from "react";
import {Text, TouchableOpacity, View} from "react-native";
import {VictoryAxis, VictoryBar, VictoryChart, VictoryTheme, VictoryTooltip} from "victory-native";

import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getStyle} from "../../../styles/styles.ts";
import {getDashboardStyle} from "../styles";
import * as I from "../../../interfaces/interfaces.tsx";
import MoreIcon from "../../../assets/more.svg";
import CenterModal from "../../../components/Modal/CenterModal";

interface WidgetBarProps {
    description: string;
    data: I.DashboardItem[];
    renderFilters?: ReactNode;
    onCloseFilters?: () => void;
}

const WidgetBar = (props: WidgetBarProps) => {
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
            <VictoryChart
                style={{
                    background: {fill: theme.colors.secondaryBaseColor},
                }}
                padding={{top: 40, bottom: 180, left: 50, right: 50}}
                theme={VictoryTheme.material}
                domainPadding={{x: 30}}>
                <VictoryAxis
                    tickValues={props.data?.sort((a, b) => b?.Value - a?.Value).map(d => d?.Label)}
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
                <VictoryAxis dependentAxis/>

                <VictoryBar
                    data={props.data?.sort((a, b) => b?.Value - a?.Value)}
                    x="Label"
                    y="Value"
                    labels={({datum}) =>
                        typeof datum?.Value === 'number'
                            ? `R$ ${datum?.Value?.toFixed(2)?.toString()}`
                            : ''
                    }
                    barWidth={10}
                    alignment="middle"
                    style={{
                        data: {fill: theme.colors.secondaryMonetaryColor},
                        labels: {
                            fill: theme.colors.primaryMonetaryColor,
                            fontSize: 14,
                        },
                    }}
                    animate={{
                        duration: 600,
                        easing: 'quadInOut',
                    }}
                    labelComponent={<VictoryTooltip/>}
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

export default WidgetBar;