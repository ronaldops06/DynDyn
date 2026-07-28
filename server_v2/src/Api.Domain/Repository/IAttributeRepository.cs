using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de atributo.
    /// </summary>
    public interface IAttributeRepository : IRepository<AttributeEntity>
    {
        /// <summary>
        /// Método responsável por retornar o atributo com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="name">Nome do atributo.</param>
        /// <param name="status">Status do atributo.</param>
        /// <returns>Entidade de atributo <see cref="AttributeEntity"/>.</returns>
        Task<AttributeEntity> SelectByUkAsync(int userId, string name);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="attributeEntity">Entidade a ter os dependentes com status alterado.</param>
        void UnchangedParentAttribute(AttributeEntity attributeEntity);
    }
}
