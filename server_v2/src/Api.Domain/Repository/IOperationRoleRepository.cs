using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de papeis de operação.
    /// </summary>
    public interface IOperationRoleRepository : IRepository<OperationRoleEntity>
    {
        /// <summary>
        /// Método responsável por retornar a papeis de operação com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="name">Nome da regra de transação.</param>
        /// <returns>Entidade de papeis de operação <see cref="OperationRoleEntity"/>.</returns>
        Task<OperationRoleEntity> SelectByUkAsync(int userId, string name);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="entity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentOperationRole(OperationRoleEntity entity);
    }
}