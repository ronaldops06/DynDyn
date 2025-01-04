import React, {useEffect} from "react";
import {Text, TouchableOpacity, View} from "react-native";
import NavPrevIcon from "../../assets/nav_prev.svg";
import NavNextIcon from "../../assets/nav_next.svg";
import {carouselSelectionStyles} from "./styles";

interface CarouselSelectionProps {
    data: any;
    handleItemSelectedId: any;
}

const CarouselSelection = (props: CarouselSelectionProps) => {
    const keys: string[] = Object.keys(props.data);
    
    const [itemIndex, setItemIndex] = React.useState<number>(1);
    
    useEffect(() => {
        props.handleItemSelectedId(props.data[keys[itemIndex]]?.Id);
    }, [itemIndex]);
    const handleLeftClick = () => {
        setItemIndex((itemIndex + 1) % keys.length);
    };

    const handleRightClick = () => {
        setItemIndex((itemIndex - 1 + keys.length) % keys.length);
    };
    
    return(
        <View style={carouselSelectionStyles.viewSelect}>
            <TouchableOpacity onPress={handleLeftClick} style={carouselSelectionStyles.buttonPrev}>
                <NavPrevIcon width="35" height="35" fill="#F5F5F5"/>
            </TouchableOpacity>
            <View style={carouselSelectionStyles.viewTitle}>
                <Text style={carouselSelectionStyles.textTitle}>{props.data[keys[itemIndex]]?.Name}</Text>
            </View>
            <TouchableOpacity onPress={handleRightClick} style={carouselSelectionStyles.buttonNext}>
                <NavNextIcon width="35" height="35" fill="#F5F5F5"/>
            </TouchableOpacity>
        </View>
    );
}

export default CarouselSelection;