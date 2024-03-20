using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de categorias.
    /// </summary>
    public interface ICategoryRepository : IRepository<CategoryEntity>
    {
        /// <summary>
        /// Método responsável por retornar a categoria com base na UK.
        /// </summary>
        /// <param name="nome">Nome da categoria.</param>
        /// <returns>Entidade de categoria <see cref="CategoryEntity"/></returns>
        Task<CategoryEntity> SelectByUkAsync(string nome);
    }
}