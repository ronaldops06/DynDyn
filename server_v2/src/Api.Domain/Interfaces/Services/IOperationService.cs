using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Models;

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

        /// <summary>
        /// Método responsável por retornar a operação com base no nome e tipo.
        /// </summary>
        /// <param name="name">Nome da operação.</param>
        /// <param name="type">Tipo da operação <see cref="OperationType"/>.</param>
        /// <returns>odelo da operação encontrada <see cref="OperationModel"/>.</returns>
        Task<OperationModel> GetByNameAndType(string name, OperationType type);
        
        /// <summary>
        /// Método responsável por retornar as operações ativas e recorrentes.
        /// </summary>
        /// <returns>Lista com os modelos de operação <see cref="OperationModel"/>.</returns>
        Task<List<OperationModel>> GetByActiveAndRecurrent();

        /// <summary>
        /// Método responsável por gerar as operações iniciais para o usuário.
        /// </summary>
        /// <param name="user">Modelo do usuário.</param>
        /// <param name="category">Modelo da categoria.</param>
        /// <returns>Modelo da operação gerado <see cref="OperationModel"/>.</returns>
        Task<OperationModel> GenerateInitialByUser(UserModel user, CategoryModel category);
    }
}