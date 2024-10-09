import { getLogin } from '../../services/api';

export const login = async (navigation: any) => {
    return await getLogin('Login/AuthValidate', navigation);
};
