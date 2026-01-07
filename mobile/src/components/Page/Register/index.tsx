import React, {ReactNode, useState} from "react";
import {useTheme} from "../../../contexts/ThemeContext.tsx";
import {SafeAreaView, ScrollView, TouchableOpacity, View} from "react-native";
import PrevIcon from "../../../assets/nav_prev.svg";
import TrashIcon from "../../../assets/trash.svg";
import {getStyleCadastro} from "../../../styles/styles.cadastro.ts";
import {getStyle} from "../../../styles/styles.ts";
import Button from "../../Button";
import HelpIcon from "../../../assets/help_outline.svg";
import Help from "../../Help";
import CustomModal from "../../CustomModal";

interface PageRegisterProps {
    onTrashClick: any;
    onBackClick: any;
    onSaveClick: any;
    helpType: string;
    isEditing: boolean;
    isLoading: boolean;
    children: ReactNode;
}

const PageRegister = ({
                          onTrashClick,
                          onBackClick,
                          isEditing,
                          onSaveClick,
                          helpType,
                          isLoading,
                          children
                      }: PageRegisterProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    const styleCadastro = getStyleCadastro(theme);

    const [showModalHelp, setShowModalHelp] = useState(false);

    return (
        <SafeAreaView style={[style.container, style.containerCadastro]}>
            <ScrollView style={style.scrollCadastro}>
                <View style={styleCadastro.viewHeaderCadastro}>
                    <TouchableOpacity
                        style={styleCadastro.buttonBack}
                        onPress={onBackClick}>
                        <PrevIcon width="40" height="40" fill={theme.colors.primaryIcon}/>
                    </TouchableOpacity>
                    <View style={style.headerScreenActions}>
                        <TouchableOpacity style={style.titleScreenMoreInfo}
                                          onPress={() => setShowModalHelp(true)}>
                            <HelpIcon width="35" height="35" fill={theme.colors.primaryIcon}/>
                        </TouchableOpacity>
                        {isEditing &&
                            <TouchableOpacity
                                style={styleCadastro.buttonTrash}
                                onPress={onTrashClick}>
                                <TrashIcon width="35" height="35" fill={theme.colors.primaryIcon}/>
                            </TouchableOpacity>}
                    </View>
                </View>
                <View style={styleCadastro.viewBodyCadastro}>
                    {children}
                    <View style={styleCadastro.areaButtonSave}>
                        <Button
                            label={"Salvar"}
                            onPress={onSaveClick}
                            loading={isLoading}
                            disabled={isLoading}
                        />
                    </View>
                </View>
                <CustomModal show={showModalHelp} setShow={setShowModalHelp}>
                    <Help helpType={helpType}/>
                </CustomModal>
            </ScrollView>
        </SafeAreaView>
    );
}

export default PageRegister;