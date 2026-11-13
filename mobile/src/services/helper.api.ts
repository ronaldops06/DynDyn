import * as I from "../interfaces/interfaces";
import {StatusHttp} from "../enums/enums";

export const validateLogin = (response: I.Response): I.Response => {
    response.isLogged = true;
    if (response.status == StatusHttp.Unauthorized)
        response.isLogged = false;
    
    return response;
};