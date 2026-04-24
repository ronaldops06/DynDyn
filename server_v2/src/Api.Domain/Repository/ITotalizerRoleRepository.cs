using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de papel de totalizador.
    /// </summary>
    public interface ITotalizerRoleRepository : IRepository<TotalizerRoleEntity>
    {
        /// <summary>
        /// Método responsável por retornar a papel de totalizador com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="code">Código da regra de totalizador.</param>
        /// <param name="type">Tipo da regra de totalizador.</param>
        /// <returns>Entidade de papel de totalizador <see cref="TotalizerRoleEntity"/>.</returns>
        Task<TotalizerRoleEntity> SelectByUkAsync(int userId, string code, TotalizerType type);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="TotalizerRoleEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentTotalizerRole(TotalizerRoleEntity TotalizerRoleEntity);
    }
}