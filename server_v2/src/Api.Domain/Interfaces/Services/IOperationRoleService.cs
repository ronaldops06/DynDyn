using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de papeis de operação.
    /// </summary>
    public interface IOperationRoleService : IService<OperationRoleModel>
    {
        /// <summary>
        /// Método responsável por retornar os papeis de operação com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da papeis de operação encontrada <see cref="OperationRoleModel"/>.</returns>
        Task<OperationRoleModel> GetById(int id);
        
        /// <summary>
        /// Método responsável por gerar os papeis de operação iniciais para o usuário.
        /// </summary>
        /// <param name="user">Modelo do usuário.</param>
        /// <returns>Modelo da papeis de operação gerado <see cref="OperationRoleModel"/>.</returns>
        Task<OperationRoleModel> GenerateInitialByUser(UserModel user);
    }
}