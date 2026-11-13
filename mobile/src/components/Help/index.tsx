import {useState, useEffect} from 'react';
import {ScrollView} from "react-native";
import Markdown from 'react-native-markdown-display';

import {readFileByAsset} from "../../fileAccess";

import {useTheme} from "../../contexts/ThemeContext";
import {getCustomMarkdownStyle} from "./styles";

interface HelpProps {
    helpType: string;
}

const Help = (props: HelpProps) => {
    const {theme} = useTheme();
    const helpStyle = getCustomMarkdownStyle(theme);

    const [content, setContent] = useState<string>('Em contrução...');

    const loadHelpByType = async (type: string) => {
        const content = await readFileByAsset(`${type}.md`);
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
