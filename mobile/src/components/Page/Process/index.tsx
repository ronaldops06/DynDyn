import {SafeAreaView, Text, TouchableOpacity, View} from "react-native";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getStyle} from "../../../styles/styles.ts";
import {constants} from "../constants";
import React, {ReactNode, useState} from "react";
import PlusIcon from "../../../assets/plus.svg";
import HelpIcon from "../../../assets/help_outline.svg";
import FilterIcon from "../../../assets/filter_list.svg";
import Help from "../../Help";
import CustomModal from "../../CustomModal";
import DeselectIcon from "../../../assets/deselect.svg";

interface PageProcessProps {
    headerType: number,
    bodyType: number,
    onNewClick: any,
    helpType: string,
    title: string,
    iconTitle: ReactNode,
    isSelectionMode?: boolean | undefined,
    onDeselectClick?: () => void | undefined,
    titleActions?: ReactNode | undefined,
    headerContent?: ReactNode | undefined,
    children: ReactNode,
}

const PageProcess = ({
                         headerType,
                         bodyType,
                         onNewClick,
                         helpType,
                         title,
                         iconTitle,
                         isSelectionMode,
                         onDeselectClick,
                         titleActions,
                         headerContent,
                         children
                     }: PageProcessProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);

    const [showModalHelp, setShowModalHelp] = useState(false);

    const getStyleHeader = () => {
        if (headerType === constants.headerType.process)
            return style.viewHeaderConsulta;
        else if (headerType === constants.headerType.processReduced)
            return style.viewHeaderConsultaReduced;
    }

    const getStyleBody = () => {
        if (bodyType === constants.bodyType.process)
            return style.viewBodyConsulta;
        else if (bodyType === constants.bodyType.processLarger)
            return style.viewBodyConsultaLarger;
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={getStyleHeader()}>
                <View style={style.titleScreen}>
                    <View style={style.titleScreenTitle}>
                        {iconTitle}
                        <Text style={style.titleScreemText}>{title}</Text>
                    </View>
                    <View style={style.headerScreenActions}>
                        {isSelectionMode &&
                            <TouchableOpacity
                                style={style.titleScreenMoreInfo}
                                onPress={onDeselectClick}>
                                <DeselectIcon width="26" height="26" fill={theme.colors.primaryIcon}/>
                            </TouchableOpacity>
                        }
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => setShowModalHelp(true)}>
                            <HelpIcon width="28" height="28" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => {}}>
                            <FilterIcon width="28" height="28" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                        {titleActions}
                    </View>
                </View>
                {headerContent}
            </View>
            <View style={getStyleBody()}>
                {children}
                {!isSelectionMode &&
                    <TouchableOpacity
                        style={style.buttonPlus}
                        onPress={onNewClick}>
                        <PlusIcon width="35" height="35" fill={theme.colors.primaryBaseColor}/>
                    </TouchableOpacity>
                }
                <CustomModal show={showModalHelp} setShow={setShowModalHelp}>
                    <Help helpType={helpType}/>
                </CustomModal>
            </View>
        </SafeAreaView>
    )
}

export default PageProcess;