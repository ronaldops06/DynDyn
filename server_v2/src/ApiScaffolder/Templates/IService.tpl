using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de {{name}}.
    /// </summary>
    public interface I{{model}}Service : IService<{{model}}Model>
    {
        /// <summary>
        /// Método responsável por retornar a {{name}} com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da {{name}} encontrada <see cref="{{model}}Model"/>.</returns>
        Task<{{model}}Model> GetById(int id);
        
        /// <summary>
        /// Método responsável por gerar as {{name}} iniciais para o usuário.
        /// </summary>
        /// <param name="user">Modelo do usuário.</param>
        /// <returns>Modelo da {{name}} gerado <see cref="{{model}}Model"/>.</returns>
        Task<{{model}}Model> GenerateInitialByUser(UserModel user);
    }
}