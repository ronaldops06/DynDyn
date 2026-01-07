import React, {useEffect, useRef, useState} from 'react';
import {Alert} from 'react-native';
import {useFocusEffect} from '@react-navigation/native';
import _ from 'lodash';
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

import {useTheme} from '../../contexts/ThemeContext';
import {getStyle} from '../../styles/styles';
import {getCategoryStyle} from './styles';
import {constants as pageConstants} from "../../components/Page/constants";
import {PageProcess} from "../../components/Page";

const Category = ({navigation, route}) => {
    const {theme} = useTheme();
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
                loadCategories(1);
            }
        }
    }, [categories]);

    useEffect(() => {
        if (categories.length !== 0) {
            setIsLoadInternal(true);
            loadCategories(pageNumber);
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

    const loadCategories = async (page: number) => {
        setLoading(true);

        let responseCategories = null;

        if (isLoadInternal) {
            responseCategories = await loadAllCategoryInternal(categoryType, page, null);
        } else {
            responseCategories = await loadAllCategory(categoryType, page, null);
            validateLogin(responseCategories, navigation);
        }

        setTotalPages(responseCategories?.totalPages ?? 1);

        appendCategories(responseCategories?.data ?? []);

        setLoading(false);
        setIsLoadInternal(false);
    };

    const handleItemClick = (data: I.Category) => {
        if (!isScrolling)
            navigation.navigate("Category", {
                screen: 'CategoryCreate',
                params: {isEditing: true, data: data}
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
        navigation.navigate("Category", {
            screen: 'CategoryCreate',
            params: {isEditing: false, data: null}
        });
    }

    return (
        <PageProcess
            headerType={pageConstants.headerType.processReduced}
            bodyType={pageConstants.bodyType.processLarger}
            title={"Categorias"}
            helpType={"category"}
            iconTitle={<CategoryIcon style={{opacity: 1}} width="24" height="24" fill={theme.colors.primaryIcon}/>}
            onNewClick={handleNewClick}
            headerContent={
                <CarouselSelection
                    disabled={loading}
                    data={constants.categoryType}
                    handleItemSelectedId={setCategoryType}/>
            }>
            <CustomScroll
                data={categories}
                loading={loading}
                totalPages={totalPages}
                pageNumber={pageNumber}
                handlePageNumber={setPageNumber}
                handleScrolling={setIsScrolling}
                renderItem={({item}) => (
                    <CategoryItem
                        data={item}
                        onPress={handleItemClick}
                        onSwipeLeft={onSwipeLeft}
                        onSwipeRight={onSwipeRight}/>
                )
                }
            />
        </PageProcess>
    );
}

export default Category;