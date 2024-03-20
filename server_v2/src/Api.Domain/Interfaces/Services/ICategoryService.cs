using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Helpers;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de categoria.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Método responsável por retornar a categoria com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da categoria encontrada <see cref="CategoryModel"/>.</returns>
        Task<CategoryModel> GetById(int id);

        /// <summary>
        /// Método responsável por retornar a(s) categoria(s) com base nos parâmetros informados.
        /// </summary>
        /// <param name="pageParams">Parâmetros para filtro <see cref="PageParams"/>.</param>
        /// <returns>Objeto de Lista com as informações das categorias encontradas.</returns>
        Task<PageList<CategoryModel>> Get(PageParams pageParams);

        /// <summary>
        /// Método responsável por realizar a inclusão do registro.
        /// </summary>
        /// <param name="category">Modelo da categoria a ser inserida <see cref="CategoryModel"/>.</param>
        /// <returns>Modelo da categoria inserida <see cref="CategoryModel"/>.</returns>
        Task<CategoryModel> Post(CategoryModel category);

        /// <summary>
        /// Método responsável por realizar a atualização do registro.
        /// </summary>
        /// <param name="category">Modelo da categoria a ser atualizada <see cref="CategoryModel"/>.</param>
        /// <returns>Modelo da categoria atualizada <see cref="CategoryModel"/>.</returns>
        Task<CategoryModel> Put(CategoryModel category);

        /// <summary>
        /// Método responsável por realizar a exclusão do registro.
        /// </summary>
        /// <param name="id">Identificador do registro a ser excluído.</param>
        /// <returns>Resultado da execução.</returns>
        Task<bool> Delete(int id);
    }
}