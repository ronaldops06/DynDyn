import HomeIcon from '../../assets/home.svg';
import AccountIcon from '../../assets/account.svg';
import EditIcon from '../../assets/edit.svg';
import PlusIcon from '../../assets/plus.svg';
import CopyIcon from '../../assets/copy.svg';
import RuleIcon from '../../assets/rule.svg';
import DoneIcon from '../../assets/done.svg';
import NavNext from '../../assets/nav_next.svg';
import NumberIcon from '../../assets/number.svg';
import TextIcon from '../../assets/text.svg';
import BooleanIcon from '../../assets/boolean.svg';
import DateIcon from '../../assets/today.svg';
import ListIcon from '../../assets/list.svg';

interface iconProps {
    name: string | 'edit' | 'home' | 'account' | 'plus' | 'copy' | 'rule' | 'done' | 'next' | 'number' | 'text' | 'boolean' | 'date' | 'listoptions';
    size: number;
    color: string;
}

const Icon = (props: iconProps) => {

    return (
        <>
            {(() => {
                switch (props.name) {
                    case 'account':
                        return <AccountIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'boolean':
                        return <BooleanIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'copy':
                        return <CopyIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'date':
                        return <DateIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'done':
                        return <DoneIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'edit':
                        return <EditIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'home':
                        return <HomeIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'listoptions':
                        return <ListIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'next':
                        return <NavNext width={props.size} height={props.size} fill={props.color}/>;
                    case 'number':
                        return <NumberIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'plus': 
                        return <PlusIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'rule':
                        return <RuleIcon width={props.size} height={props.size} fill={props.color}/>;
                    case 'text':
                        return <TextIcon width={props.size} height={props.size} fill={props.color}/>;
                        default:
                        return "";
                }
            })()}
        </>
    );
}

export default Icon;