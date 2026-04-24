using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de papel de totalizador.
    /// </summary>
    public interface ITotalizerRoleService : IService<TotalizerRoleModel>
    {
        /// <summary>
        /// Método responsável por retornar a papel de totalizador com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da papel de totalizador encontrada <see cref="TotalizerRoleModel"/>.</returns>
        Task<TotalizerRoleModel> GetById(int id);
        
        /// <summary>
        /// Método responsável por gerar as papel de totalizador iniciais para o usuário.
        /// </summary>
        /// <param name="user">Modelo do usuário.</param>
        /// <returns>Modelo da papel de totalizador gerado <see cref="TotalizerRoleModel"/>.</returns>
        Task<TotalizerRoleModel> GenerateInitialByUser(UserModel user);
    }
}