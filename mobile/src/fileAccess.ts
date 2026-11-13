import { Platform } from 'react-native';
import { Dirs, FileSystem } from 'react-native-file-access';

export const readFileByAsset = async (fileName: string) => {
    try {
        if (Platform.OS === 'android') {
            const destinoCache = `${Dirs.CacheDir}/${fileName}`;

            // Copia o arquivo dos assets para o cache interno do app
            await FileSystem.cpAsset(`help/${fileName}`, destinoCache);

            // Lê o conteúdo do arquivo copiado
            return await FileSystem.readFile(destinoCache, 'utf8');
        } else {
            // No iOS, você pode ler diretamente do MainBundleDir se o arquivo estiver lá
            const caminhoIOS = `${Dirs.MainBundleDir}/${fileName}`;
            return await FileSystem.readFile(caminhoIOS, 'utf8');
        }
    } catch (erro) {
        console.error('Erro ao ler o arquivo dos assets:', erro);
        return null;
    }
}
