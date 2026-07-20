using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de atributo.
    /// </summary>
    public interface IAttributeService : IService<AttributeModel>
    {
        /// <summary>
        /// Método responsável por retornar o atributo com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo do atributo encontrado <see cref="AttributeModel"/>.</returns>
        Task<AttributeModel> GetById(int id);
    }
}
