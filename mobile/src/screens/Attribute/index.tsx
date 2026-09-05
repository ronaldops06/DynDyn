import React, {useEffect, useRef, useState} from 'react';
import { View} from 'react-native';
import {useFocusEffect} from '@react-navigation/native';
import _ from 'lodash';
import CustomScroll from "../../components/CustomScroll";
import {PageSpecial} from "../../components/Page";
import AuxiliaryButton from "../../components/AuxiliaryButton";

import {constants} from "../../constants";
import * as I from "../../interfaces/interfaces";
import {Situation} from "../../enums/enums";
import {validateLogin} from "../../utils";

import {loadAllAttribute, loadAllAttributeInternal} from "../../controller/attribute.controller";
import AttributeItem from "./AttributeItem";

import {useTheme} from '../../contexts/ThemeContext';
import {getAttributeStyle} from "./styles";

const Attribute = ({navigation, route}: {navigation: any, route: any}) => {
    const {theme} = useTheme();
    const attributeStyle = getAttributeStyle(theme);

    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [filter, setFilter] = useState<I.AttributeFilter>({} as I.AttributeFilter);
    const [attributes, setAttributes] = useState<I.Attribute[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(true);

    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setAttributes([]);
            }
        }, [route.params?.actionNavigation])
    );

    useEffect(() => {
        if (isFirstRender.current) {
            isFirstRender.current = false;
        } else {
            if (attributes.length === 0) {
                loadAttributes(1);
            }
        }
    }, [attributes]);

    useEffect(() => {
        if (attributes.length !== 0) {
            setIsLoadInternal(true);
            loadAttributes(pageNumber);
        }
    }, [pageNumber]);

    useEffect(() => {
        setPageNumber(1);
        updateAttributes();

        return () => updateAttributes.cancel();
    }, []);

    const updateAttributes = _.debounce(() => {
        setAttributes([]);
    }, 500);

    const appendAttributes = (data: I.Attribute[]) => {
        let attributesNew = attributes;
        if (data.length > 0) {
            data.map((item, key) => {
                attributesNew.push(item);
            });
            setAttributes(attributesNew);
        }
    };

    const loadAttributes = async (page: number) => {
        setLoading(true);

        let responseAttributes = null;

        if (isLoadInternal) {
            responseAttributes = await loadAllAttributeInternal(page, null);
        } else {
            responseAttributes = await loadAllAttribute(page, null);
            validateLogin(responseAttributes, navigation);
        }

        setTotalPages(responseAttributes?.totalPages ?? 1);
        appendAttributes(responseAttributes?.data ?? []);

        setLoading(false);
        setIsLoadInternal(true);
    };

    const filterData = (attributes: I.Attribute[]): I.Attribute[] => {
        let result = attributes;

        if (filter.Situation !== undefined && filter.Situation !== Situation.All) {
            result = result.filter(item => {
                return filter.Situation !== Situation.All ? item.Status === filter.Situation : item;
            });
        }

        if (filter.Search && filter.Search !== "") {
            result = result.filter(item => {
                return item.Name.toLowerCase().includes(filter.Search);
            })
        }

        return result;
    };

    const handleItemClick = (data: I.Attribute) => {
        if (!isScrolling)
            navigation.navigate('AttributeCreate',
                {isEditing: true, data: data});
    }
    
    const handleBackClick = () => {
        navigation.goBack();
    };

    const handleNewClick = () => {
        navigation.navigate('AttributeCreate',
            {isEditing: false, data: null}
        );
    }

    return (
        <PageSpecial
            title={"Atributos Personalizados"}
            helpType={"attribute"}
            onBackClick={handleBackClick}
        >
            <View style={attributeStyle.areaButtonAdd}>
            <AuxiliaryButton
                text="Adicionar Patrimônio"
                onPress={handleNewClick}
                icon="plus"
                iconColor={theme.colors.quaternaryIcon}
                type="secondary"
            />
            </View>
            <View>
                <CustomScroll
                    data={filterData(attributes)}
                    styles={attributeStyle.scroll}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    renderItem={({item}) => (
                        <AttributeItem
                            data={item}
                            onPress={handleItemClick}/>
                    )
                    }
                />
            </View>
        </PageSpecial>
    );
}

export default Attribute;