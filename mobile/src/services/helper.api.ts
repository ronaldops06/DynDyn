import * as I from "../interfaces/interfaces.tsx";
import {StatusHttp} from "../enums/enums.tsx";

export const validateLogin = (response: I.Response): I.Response => {
    response.isLogged = true;
    if (response.status == StatusHttp.Unauthorized)
        response.isLogged = false;
    
    return response;
};