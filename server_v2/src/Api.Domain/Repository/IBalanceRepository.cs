using System;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de saldo.
    /// </summary>
    public interface IBalanceRepository : IRepository<BalanceEntity>
    {
        /// <summary>
        /// Método responsável por retornar o saldo com base na UK.
        /// </summary>
        /// <param name="accountId">Identificador da conta.</param>
        /// <param name="balanceDate">Data do saldo.</param>
        /// <returns>Entidade de categoria <see cref="CategoryEntity"/></returns>
        Task<BalanceEntity> SelectByUkAsync(int accountId, DateTime balanceDate);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="balanceEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentBalance(BalanceEntity balanceEntity);
    }
}