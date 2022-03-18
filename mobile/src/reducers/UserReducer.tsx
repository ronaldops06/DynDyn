export const initialState = {
    avatar: ''
};

export const UserReducer = (props: UserReducerParams) => {
    
    switch (props.action.type){
        case 'setAvatar':
            return { ...props.state, avatar: props.action.payload.avatar };
            break;
        default:
            return props.state;
    }
}

interface UserReducerParams {
    state: any;
    action: any;
}