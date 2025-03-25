using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de conta.
    /// </summary>
    public interface IPortfolioService : IService<PortfolioModel>
    {
        /// <summary>
        /// Método responsável por retornar a conta com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da conta encontrada <see cref="PortfolioModel"/>.</returns>
        Task<PortfolioModel> GetById(int id);
    }
}