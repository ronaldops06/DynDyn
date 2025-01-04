import React, {useEffect, useState} from 'react';
import {View, Text, SafeAreaView, TouchableOpacity} from 'react-native';
import {StackNavigationProp} from '@react-navigation/stack';
import {useNavigation} from '@react-navigation/core';
import {RootStackParamList} from '../RootStackParams';
import _ from 'lodash';

import {style} from '../../styles/styles';
import {categoryStyle} from './styles';
import PlusIcon from "../../assets/plus.svg";
import * as I from "../../interfaces/interfaces.tsx";
import CategoryItem from "./CategoryItem";
import {CustomAlert} from "../../components/CustomAlert";
import {loadAllCategory, loadAllCategoryInternal} from "../../controller/category.controller.tsx";
import NavPrevIcon from "../../assets/nav_prev.svg";
import NavNextIcon from "../../assets/nav_next.svg";

import {constants} from "../../constants";
import CustomScroll from "../../components/CustomScroll";
import CarouselSelection from "../../components/CarouselSelection";

type homeScreenProp = StackNavigationProp<RootStackParamList, 'Category'>;

const Category = () => {
    const keys: string[] = Object.keys(constants.categoryType);

    const navigation = useNavigation<homeScreenProp>();

    const [loading, setLoading] = useState(false);
    const [isScrolling, setIsScrolling] = useState(false);
    const [isLoadInternal, setIsLoadInternal] = useState(false);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [categories, setCategories] = useState<I.Category[]>([]);
    const [categoryType, setCategoryType] = useState<number>(1);

    useEffect(() => {
        setCategoryType(1);
    }, []);

    useEffect(() => {
        // Executado no goBack da tela seguinte
        navigation.addListener('focus', () => {
            setIsLoadInternal(true);
            setCategories([]);
        });
    }, [navigation]);

    useEffect(() => {
        if (categories.length === 0) {
            setPageNumber(1);
            loadCategories();
        }
    }, [categories]);

    useEffect(() => {
        setIsLoadInternal(true);
        loadCategories();
    }, [pageNumber]);

    useEffect(() => {
        updateCategories();

        return () => updateCategories.cancel();
    }, [categoryType]);

    /*Se clicar várias vezes na troca de datas essa lógica faz com que não seja efetuado a busca em todas as trocas de 
    datas, o "debounce" faz com que aguarde para executar a função e se for chamada novamente enquanto o tempo não acabou
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

        if (isLoadInternal)
            responseCategories = await loadAllCategoryInternal(categoryType, pageNumber);
        else
            responseCategories = await loadAllCategory(categoryType, pageNumber, navigation);

        setTotalPages(responseCategories?.totalPages ?? 1);
        appendCategories(responseCategories?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Category) => {
        if (!isScrolling)
            navigation.navigate("CategoryCreate", {isEditing: true, data: data});
    }

    const onSwipeLeft = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta categoria terá o status alterado. Deseja continuar?", console.log("implementar"));
    }

    const onSwipeRight = (data: I.Transaction) => {
        CustomAlert("Atenção", "Esta categoria será excluída. Deseja continuar?", console.log("implementar"));
    }

    const handleNewClick = () => {
        navigation.navigate("CategoryCreate");
    }

    return (
        <SafeAreaView style={[style.container, style.containerConsulta]}>
            <View style={style.viewHeaderConsultaReduced}>
                <CarouselSelection data={constants.categoryType} handleItemSelectedId={setCategoryType}/>
            </View>
            <View style={style.viewBodyConsultaLarger}>
                <CustomScroll
                    loading={loading}
                    totalPages={totalPages}
                    pageNumber={pageNumber}
                    handlePageNumber={setPageNumber}
                    handleScrolling={setIsScrolling}
                >
                    {categories != null && categories.map((item, key) => (
                        <CategoryItem
                            key={key}
                            data={item}
                            onPress={handleItemClick}
                            onSwipeLeft={onSwipeLeft}
                            onSwipeRight={onSwipeRight}/>
                    ))
                    }
                </CustomScroll>
                <TouchableOpacity
                    style={categoryStyle.buttonPlus}
                    onPress={handleNewClick}>
                    <PlusIcon width="35" height="35" fill="#6E8BB8"/>
                </TouchableOpacity>
            </View>
        </SafeAreaView>
    );
}

export default Category;