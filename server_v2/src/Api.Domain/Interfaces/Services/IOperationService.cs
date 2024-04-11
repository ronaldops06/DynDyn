using System.Threading.Tasks;
using Api.Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de operação.
    /// </summary>
    public interface IOperationService : IService<OperationModel>
    {
        /// <summary>
        /// Método responsável por retornar a operação com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da operação encontrada <see cref="OperationModel"/>.</returns>
        Task<OperationModel> GetById(int id);
    }
}