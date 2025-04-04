using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de operação.
    /// </summary>
    public interface IOperationRepository : IRepository<OperationEntity>
    {
        /// <summary>
        /// Método responsável por retornar a operação com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="name">Nome da operação.</param>
        /// <param name="operationType">Tipo da operação <see cref="OperationType"/>.</param>
        /// <returns>Entidade de operação <see cref="OperationEntity"/>.</returns>
        Task<OperationEntity> SelectByUkAsync(int userId, string name, OperationType operationType);
        
        /// <summary>
        /// Método responsável por retornar as operações ativas e recorrentes.
        /// </summary>
        /// <param name="userId">Indetificador do usuário</param>
        /// <returns>Lista com as entidades de operação <see cref="OperationEntity"/>.</returns>
        Task<List<OperationEntity>> SelectByActiveAndRecurrent(int userId);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="operationEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentOperation(OperationEntity operationEntity);
    }
}