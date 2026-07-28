import {Text, View} from "react-native";
import {getTagStyles} from "./styles";
import {useTheme} from "../../contexts/ThemeContext.tsx";
import Icon from "../Icon";

interface TagProps {
    text: string;
    textColor: string;
    color: string;
    icon?: string | undefined;
    style?: {} | undefined;
}

const Tag = (props: TagProps) => {
    const { theme } = useTheme();
    const tagStyle = getTagStyles(theme);
    
    return(
        <View style={[tagStyle.container, { backgroundColor: props.color}, props.style]}>
            {props.icon && <Icon name={props.icon} size="20" color={theme.colors.octaveIcon}/>}
            <Text style={[tagStyle.text, { color: props.textColor}]}>{props.text}</Text>
        </View>
    );
}

export default Tag;