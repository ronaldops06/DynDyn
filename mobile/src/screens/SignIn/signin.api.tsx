import { postLogin } from '../../services/api';

import * as I from '../../interfaces/interfaces';

export const login = async (data: I.Login): Promise<I.User | null> => {
    return await postLogin('Login/Auth', data);
};
