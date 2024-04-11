using System.Threading.Tasks;
using Domain.Helpers;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço com os métodos genéricos.
    /// </summary>
    public interface IService<T>
    {
        /// <summary>
        /// Método responsável por retornar a(s) categoria(s) com base nos parâmetros informados.
        /// </summary>
        /// <param name="pageParams">Parâmetros para filtro <see cref="PageParams"/>.</param>
        /// <returns>Objeto de Lista com as informações das categorias encontradas.</returns>
        Task<PageList<T>> Get(PageParams pageParams);

        /// <summary>
        /// Método responsável por realizar a inclusão do registro.
        /// </summary>
        /// <param name="model">Modelo a ser inserido</param>
        /// <returns>Modelo inserido.</returns>
        Task<T> Post(T model);

        /// <summary>
        /// Método responsável por realizar a atualização do registro.
        /// </summary>
        /// <param name="model">Modelo a ser atualizado.</param>
        /// <returns>Modelo atualizado.</returns>
        Task<T> Put(T model);

        /// <summary>
        /// Método responsável por realizar a exclusão do registro.
        /// </summary>
        /// <param name="id">Identificador do registro a ser excluído.</param>
        /// <returns>Resultado da execução.</returns>
        Task<bool> Delete(int id);
    }
}