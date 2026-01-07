import { useState, useEffect } from 'react';
import RNFS from 'react-native-fs';
import Markdown from 'react-native-markdown-display';

interface HelpProps {
    helpType: string;
}

const Help = (props: HelpProps) => {
    
    const [content, setContent] = useState<string>('Em contrução...');

    const loadHelpByType = async (type: string) => {
        const content = await RNFS.readFileAssets(`help/${type}.md`, 'utf8');   
        setContent(content);  
    };
    
    useEffect(() => {
        loadHelpByType(props.helpType);
    }, []);
    
    
    return (
        <Markdown>
            {content as any}
        </Markdown>
    )
};

export default Help;
