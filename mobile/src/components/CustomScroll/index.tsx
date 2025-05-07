import {style} from "../../styles/styles.ts";
import {ActivityIndicator, FlatList, TextStyle} from "react-native";
import React from "react";

interface CustomScrollParams {
    data: any[];
    loading: boolean;
    totalPages: number;
    pageNumber: number;
    handlePageNumber: any;
    handleScrolling: any;
    renderItem: ({ item }: { item: any }) => JSX.Element;
    styles?: TextStyle | undefined;
}

const CustomScroll = (props: CustomScrollParams) => {
        
    const reloadPage = () => {
        if (!props.loading && props.pageNumber <= props.totalPages) {
            props.handlePageNumber(props.pageNumber + 1);
        }
    }
    
    return (
        <FlatList
            data={props.data}
            style={props.styles ? props.styles : style.scroll}
            keyExtractor={(item) => item.Id.toString()}
            renderItem={props.renderItem}
            onMomentumScrollBegin={() => props.handleScrolling(true)}
            onMomentumScrollEnd={() => props.handleScrolling(false)}
            onEndReached={reloadPage}
            onEndReachedThreshold={0.5}
            ListFooterComponent={props.loading ? <ActivityIndicator style={style.loadingIcon} size="large" color="#6E8BB8"/> : null}
        />
    );
};

export default CustomScroll;