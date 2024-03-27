using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Helpers;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de categoria.
    /// </summary>
    public interface ICategoryService : IService<CategoryModel>
    {
        /// <summary>
        /// Método responsável por retornar a categoria com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da categoria encontrada <see cref="CategoryModel"/>.</returns>
        Task<CategoryModel> GetById(int id);
    }
}