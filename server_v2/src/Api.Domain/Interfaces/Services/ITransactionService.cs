using System;
using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Helpers;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de transação.
    /// </summary>
    public interface ITransactionService : IService<TransactionModel>
    {
        /// <summary>
        /// Método responsável por retornar a transação com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da transação encontrada <see cref="TransactionModel"/>.</returns>
        Task<TransactionModel> GetById(int id);

        /// <summary>
        /// Método responsável por retornar os valores totais das transação.
        /// </summary>
        /// <param name="pageParams">Parâmetros para filtro <see cref="PageParams"/>.</param>
        /// <returns>Modelo dos totais das transações <see cref="TransactionTotalModel"/>.</returns>
        Task<TransactionTotalModel> GetTotais(PageParams pageParams);

        /// <summary>
        /// Método responsável por executar a geração das transações de operações recorrentes.
        /// </summary>
        /// <param name="baseDate">Data base para geração das transações.</param>
        Task GenerateRecurringAndInstallmentPayments(DateTime? baseDate);
    }
}