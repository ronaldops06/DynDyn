using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de transações/movimentos.
    /// </summary>
    public interface ITransactionRepository : IRepository<TransactionEntity>
    {
        /// <summary>
        /// Método responsável por retornar os valores totais das transações por tipo de operação.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="pageParams">Parametros de filtros.</param>
        /// <returns>Dicionário com os valores totais por tipo de operação.</returns>
        Task<Dictionary<OperationType, double>> SelectTransactionsTotalsAsync(int userId, PageParams pageParams);

        /// <summary>
        /// Método responsável por retornar as transações de acordo com a transação pai.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="parentTransactionId">Identificador da transação pai.</param>
        /// <returns>Lista de transações encontradas.</returns>
        Task<IEnumerable<TransactionEntity>> SelectTransactionByParentTransactionIdAsync(int userId, int parentTransactionId);
        
        /// <summary>
        /// Método responsável por retornar as transações de acordo com a operação e o período.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="operationId">Identificador da operação.</param>
        /// <param name="period">Período considerado <see cref="Period"/>.</param>
        /// <returns>Lista de transações encontradas.</returns>
        Task<IEnumerable<TransactionEntity>> SelectByOperationAndPeriodAsync(int userId, int operationId, Period period);
        
        /// <summary>
        /// Método responsável por retornar as transações de acordo com a operação e o período.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="operationId">Identificador da operação.</param>
        /// <param name="creationDate">Data de criação.</param>
        /// <returns>Entidade de transação <see cref="TransactionEntity"/>.</returns>
        Task<TransactionEntity> SelectByOperationAndDateAndValueAsync(int userId, int operationId, DateTime creationDate, double value);
        
        /// <summary>
        /// Método responsável por retornar as transações com parcelas pendentes em um periodo.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <param name="period">Período considerado <see cref="Period"/>.</param>
        /// <returns>Lista de transações encontradas.</returns>
        Task<List<TransactionEntity>> SelectByPendingInstallments(int userId, Period period);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="transactionEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentTransaction(TransactionEntity transactionEntity);
    }
}