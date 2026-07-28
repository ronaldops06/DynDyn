using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Models;
using Domain.Models;

namespace Api.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface de serviço dos métodos específicos de Portfolio Attribute.
    /// </summary>
    public interface IPortfolioAttributeService : IService<PortfolioAttributeModel>
    {
        /// <summary>
        /// Método responsável por retornar a Portfolio Attribute com base no identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <returns>Modelo da Portfolio Attribute encontrada <see cref="PortfolioAttributeModel"/>.</returns>
        Task<PortfolioAttributeModel> GetById(int id);

        /// <summary>
        /// Método responsável por retornar a Portfolio Attribute com base no Portfolio e Attribute.
        /// </summary>
        /// <param name="portfolioId">Identificador do portfólio.</param>
        /// <param name="attributeId">Identificador do atributo.</param>
        /// <returns>Modelo da Portfolio Attribute encontrada <see cref="PortfolioAttributeModel"/>.</returns>
        Task<PortfolioAttributeModel> GetByPortfolioAndAttributeAsync(int portfolioId, int attributeId);

        /// <summary>
        /// Método responsável por retornar todas as Portfolio Attributes de um portfólio.
        /// </summary>
        /// <param name="portfolioId">Identificador do portfólio.</param>
        /// <returns>Lista de modelos de Portfolio Attribute <see cref="PortfolioAttributeModel"/>.</returns>
        Task<IEnumerable<PortfolioAttributeModel>> GetByPortfolioAsync(int portfolioId);
    }
}