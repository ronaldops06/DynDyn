using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Domain.Interfaces;

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
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="type">Tipo da categoria</param>
        /// <param name="nome">Nome da categoria.</param>
        /// <returns>Entidade de categoria <see cref="CategoryEntity"/></returns>
        Task<CategoryEntity> SelectByUkAsync(int userId, CategoryType type, string nome);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="categoryEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentCategory(CategoryEntity categoryEntity);
    }
}