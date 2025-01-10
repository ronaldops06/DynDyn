using System.Threading.Tasks;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos do saldo.
    /// </summary>
    public interface IBalanceService : IService<BalanceModel>
    {
        /// <summary>
        /// Método responsável por retornar o base com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo do saldo encontrado <see cref="BalanceModel"/>.</returns>
        Task<BalanceModel> GetById(int id);
    }
}