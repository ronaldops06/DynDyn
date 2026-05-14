import {SafeAreaView, Text, TouchableOpacity, View} from "react-native";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {getStyle} from "../../../styles/styles.ts";
import {constants} from "../constants";
import React, {ReactNode, useCallback, useState} from "react";
import PlusIcon from "../../../assets/plus.svg";
import HelpIcon from "../../../assets/help_outline.svg";
import FilterIcon from "../../../assets/filter_list.svg";
import FilterActivatedIcon from "../../../assets/filter_activated.svg";
import Help from "../../Help";
import CustomModal from "../../CustomModal";
import DeselectIcon from "../../../assets/deselect.svg";
import {isInternetConnected} from "../../../utils.ts";
import {useFocusEffect} from "@react-navigation/native";

interface FilterBaseProps {
    onClose?: () => void;
}

interface PageProcessProps {
    headerType: number;
    bodyType: number;
    onNewClick: any;
    helpType: string;
    title: string;
    iconTitle: ReactNode;
    isSelectionMode?: boolean | undefined;
    onDeselectClick?: () => void | undefined;
    titleActions?: ReactNode | undefined;
    headerContent?: ReactNode | undefined;
    topBodyArea?: ReactNode | undefined;
    children: ReactNode;
    renderFilters?: (closeModal: () => void) => ReactNode;
    filterActivated?: boolean;
    message?: string;
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
                         topBodyArea,
                         children,
                         renderFilters,
                         renderFilterFooter,
                         filterActivated,
                         message
                     }: PageProcessProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);

    const [showModalHelp, setShowModalHelp] = useState(false);
    const [showModalFilter, setShowModalFilter] = useState<boolean>(false);
    const [localMessage, setLocalMessage] = useState("");

    const loadData = async () => {
        let isConnected = await isInternetConnected();
        if (!isConnected)
            setLocalMessage("Sem conexão com a internet, seus dados podem estar desatualizados.");
        else
            setLocalMessage("");
    };

    useFocusEffect(
        useCallback(() => {
            loadData();
        }, [])
    );
        
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

    const closeModal = () => setShowModalFilter(false);

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
                                <DeselectIcon width="23" height="23" fill={theme.colors.primaryIcon}/>
                            </TouchableOpacity>
                        }
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => setShowModalHelp(true)}>
                            <HelpIcon width="24" height="24" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => setShowModalFilter(true)}>
                            {filterActivated ?
                                <FilterActivatedIcon width="25" height="25" fill={theme.colors.primaryIcon}/> :
                                <FilterIcon width="25" height="25" fill={theme.colors.primaryIcon}/>
                            }
                        </TouchableOpacity>
                        {titleActions}
                    </View>
                </View>
                {headerContent}
            </View>
            <View style={getStyleBody()}>
                {topBodyArea}
                {localMessage &&
                    <View style={style.viewAreaMessage}><Text style={style.textMessage}>{localMessage}</Text></View>
                }
                {children}
                {!isSelectionMode &&
                    <TouchableOpacity
                        style={style.buttonPlus}
                        onPress={onNewClick}>
                        <PlusIcon width="35" height="35" fill={theme.colors.quaternaryIcon}/>
                    </TouchableOpacity>
                }
                <CustomModal show={showModalHelp} setShow={setShowModalHelp}>
                    <Help helpType={helpType}/>
                </CustomModal>
                <CustomModal show={showModalFilter} setShow={setShowModalFilter}>
                    {renderFilters?.(closeModal)}
                </CustomModal>
            </View>
        </SafeAreaView>
    )
}

export default PageProcess;