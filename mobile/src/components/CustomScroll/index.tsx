import {style} from "../../styles/styles.ts";
import {isEndScroll} from "../../utils.ts";
import {ActivityIndicator, ScrollView, View, TextStyle} from "react-native";
import {categoryStyle} from "../../screens/Category/styles";
import React, {useState} from "react";

interface CustomScrollParams {
    loading: boolean;
    totalPages: number;
    pageNumber: number;
    handlePageNumber: any;
    handleScrolling: any;
    children: React.ReactNode;
    styles?: TextStyle | undefined;
}

const CustomScroll = (props: CustomScrollParams) => {

    const [pageNumber, setPageNumber] = useState(1);

    const reloadPage = () => {
        if (props.pageNumber <= props.totalPages) {
            props.handlePageNumber(props.pageNumber + 1);
        }
    }

    const handleTouchEnd = () => {
        setTimeout(props.handleScrolling, 2000, false);
    }
    
    return (
        <ScrollView style={props.styles ? props.styles : style.scroll}
            onScroll={(event) => {
                if (isEndScroll(event)) {
                    if (!props.loading) {
                        reloadPage();
                    }
                }
            }}
            onTouchStart={(event) => props.handleScrolling(false)}
            onTouchMove={(event) => props.handleScrolling(true)}
            onTouchEnd={(event) => handleTouchEnd}
        >
            <View style={categoryStyle.viewList}>
                {props.children}
                {props.loading &&
                    <ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8"/>
                }
            </View>
        </ScrollView>
    );
};

export default CustomScroll;