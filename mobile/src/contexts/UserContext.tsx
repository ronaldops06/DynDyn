import React, { createContext, useReducer } from 'react';
import { initialState, UserReducer } from '../reducers/UserReducer';

interface UserContextParams {
    children: any;
}

export const UserContext = createContext({});

export default (props: UserContextParams) => {
    function init(initial) {
        return initial;
    }
    
    const[state, dispatch] = useReducer(UserReducer, initialState, init);

    return (
        <UserContext.Provider value={{state, dispatch}}>
            {props.children}
        </UserContext.Provider>
    )
}