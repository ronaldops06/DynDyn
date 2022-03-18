import { postLogin } from '../../services/api';

import * as I from '../../interfaces/interfaces';

export const login = async (data: I.Login) => {
    return await postLogin('User/Auth', data);
};
