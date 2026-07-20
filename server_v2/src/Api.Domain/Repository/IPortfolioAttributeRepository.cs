using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de Portfolio Attribute.
    /// </summary>
    public interface IPortfolioAttributeRepository : IRepository<PortfolioAttributeEntity>
    {
        /// <summary>
        /// Método responsável por retornar o Portfolio Attribute com base no Portfolio e Attribute.
        /// </summary>
        /// <param name="portfolioId">Identificador do portfólio</param>
        /// <param name="attributeId">Identificador do atributo</param>
        /// <returns>Entidade de Portfolio Attribute <see cref="PortfolioAttributeEntity"/>.</returns>
        Task<PortfolioAttributeEntity> SelectByPortfolioAndAttributeAsync(int portfolioId, int attributeId);

        /// <summary>
        /// Método responsável por retornar todos os Portfolio Attributes de um portfólio.
        /// </summary>
        /// <param name="portfolioId">Identificador do portfólio</param>
        /// <returns>Lista de entidades de Portfolio Attribute <see cref="PortfolioAttributeEntity"/>.</returns>
        Task<IEnumerable<PortfolioAttributeEntity>> SelectByPortfolioAsync(int portfolioId);
        
        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="portfolioAttributeEntity">Entidade a ter os dependentes com status alterado.</param>
        void UnchangedParentPortfolioAttribute(PortfolioAttributeEntity portfolioAttributeEntity);
    }
}