import {useState, useEffect} from 'react';
import {ScrollView} from "react-native";
import RNFS from 'react-native-fs';
import Markdown from 'react-native-markdown-display';
import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getCustomMarkdownStyle} from "./styles";

interface HelpProps {
    helpType: string;
}

const Help = (props: HelpProps) => {
    const {theme} = useTheme();
    const helpStyle = getCustomMarkdownStyle(theme);

    const [content, setContent] = useState<string>('Em contrução...');

    const loadHelpByType = async (type: string) => {
        const content = await RNFS.readFileAssets(`help/${type}.md`, 'utf8');
        setContent(content);
    };

    useEffect(() => {
        loadHelpByType(props.helpType);
    }, []);


    return (
        <ScrollView>
            <Markdown
                style={helpStyle}>
                {content as any}
            </Markdown>
        </ScrollView>
    )
};

export default Help;
