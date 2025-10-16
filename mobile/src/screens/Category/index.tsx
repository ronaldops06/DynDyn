import React, {useEffect, useRef, useState} from 'react';
import {Alert, SafeAreaView, Text, TouchableOpacity, View} from 'react-native';
import {useFocusEffect} from '@react-navigation/native';
import _ from 'lodash';

import PlusIcon from "../../assets/plus.svg";
import * as I from "../../interfaces/interfaces.tsx";
import CategoryItem from "./CategoryItem";
import {
    alterCategory,
    excludeCategory,
    loadAllCategory,
    loadAllCategoryInternal
} from "../../controller/category.controller.tsx";

import {constants} from "../../constants";
import CustomScroll from "../../components/CustomScroll";
import CarouselSelection from "../../components/CarouselSelection";
import {validateLogin} from "../../utils.ts";
import CategoryIcon from '../../assets/category.svg';

import { useTheme } from '../../contexts/ThemeContext';
import {getStyle} from '../../styles/styles';
import {getCategoryStyle} from './styles';

const Category = ({navigation, route}) => {
    const { theme } = useTheme();
    const style = getStyle(theme);
    const categoryStyle = getCategoryStyle(theme);
    
    const [loading, setLoading] = useState(false);
    const isFirstRender = useRef(true);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [categoryType, setCategoryType] = useState<number>(constants.categoryType.operation.Id);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    
    useFocusEffect(
        React.useCallback(() => {
            if (route.params?.actionNavigation === constants.actionNavigation.reload) {
                isFirstRender.current = false;
                setIsLoadInternal(true);
                setCategories([]);
            }
        }, [route.params?.actionNavigation])
    );

    useEffect(() => {
        //Faz com que não execute na abertura da tela (renderização)
        if (isFirstRender.current) {
            isFirstRender.current = false;
        } else {
            if (categories.length === 0) {
                loadCategories();
            }
        }
    }, [categories]);

    useEffect(() => {
        if (categories.length !== 0) {
            setIsLoadInternal(true);
            loadCategories();
        }
    }, [pageNumber]);

    useEffect(() => {
        setPageNumber(1);
        updateCategories();
        
        return () => updateCategories.cancel();
    }, [categoryType]);

    /*Se clicar várias vezes na troca de tipos essa lógica faz com que não seja efetuado a busca em todas as trocas, 
    o "debounce" faz com que aguarde para executar a função e se for chamada novamente enquanto o tempo não acabou
    cancela a chamada anterior e começa a aguardar novamente.*/
    const updateCategories = _.debounce(() => {
        setCategories([]);
    }, 500);
    
    const appendCategories = (data: I.Category[]) => {
        let categoriesNew = categories;
        if (data.length > 0) {
            data.map((item, key) => {
                categoriesNew.push(item);
            });
            setCategories(categoriesNew);
        }
    };

    const loadCategories = async () => {
        setLoading(true);

        let responseCategories = null;

        if (isLoadInternal) {
            responseCategories = await loadAllCategoryInternal(categoryType, pageNumber);
        } else {
            responseCategories = await loadAllCategory(categoryType, pageNumber);
            validateLogin(responseCategories, navigation);
        }

        setTotalPages(responseCategories?.totalPages ?? 1);
        
        appendCategories(responseCategories?.data ?? []);
        
        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Category) => {
        if (!isScrolling)
            navigation.navigate("CategoryCreate", {
                isEditing: true, data: data
            });
    }

    const onSwipeLeft = (data: I.Category) => {
        Alert.alert("Atenção!",
            "Esta categoria terá o status alterado. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        data.Status = (data.Status === constants.status.active.Id) ? constants.status.inactive.Id : constants.status.active.Id;
                        let response = await alterCategory(data);
                        validateLogin(response, navigation);

                        setCategories((prevCategories) =>
                            prevCategories.map((item) =>
                                item.Id === data.Id ? data : item
                            )
                        );
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const onSwipeRight = (data: I.Category) => {
        Alert.alert("Atenção!",
            "Esta categoria será excluída. Deseja continuar?",
            [
                {
                    text: "Não",
                    style: "cancel"
                },
                {
                    text: "Sim",
                    onPress: async () => {
                        let response = await excludeCategory(data.Id, data.InternalId);
                        validateLogin(response, navigation);

                        if (response.success) {
                            setIsLoadInternal(true);
                            setCategories([]);
                        }
                    }
                }
            ],
            {cancelable: false}
        );
    }

    const handleNewClick = () => {
        navigation.navigate("CategoryCreate", {
            isEditing: false, data: null, onGoBack: (actionNavigation: string) => {
                if (actionNavigation === constants.actionNavigation.reload) {
                    setIsLoadInternal(true);
                    setCategories([]);
                }
            }
        });
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <View style={style.titleScreen}>
                    <View style={style.titleScreenTitle}>
                        <CategoryIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>
                        <Text style={style.titleScreemText}>Categorias</Text>
                    </View>
                </View>
                <CarouselSelection disabled={loading} data={constants.categoryType} handleItemSelectedId={setCategoryType}/>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    data={categories}
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                    renderItem={({ item }) => (
                        <CategoryItem
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                        )
                    }
                />
                <TouchableOpacity
                    style={categoryStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill={theme.colors.primaryBaseColor}/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Category;