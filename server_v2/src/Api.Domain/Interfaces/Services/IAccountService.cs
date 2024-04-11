using System.Threading.Tasks;
using Api.Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de conta.
    /// </summary>
    public interface IAccountService : IService<AccountModel>
    {
        /// <summary>
        /// Método responsável por retornar a conta com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da conta encontrada <see cref="AccountModel"/>.</returns>
        Task<AccountModel> GetById(int id);
    }
}