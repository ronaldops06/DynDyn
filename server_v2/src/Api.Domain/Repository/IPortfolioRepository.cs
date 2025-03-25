using System.Threading.Tasks;
using Api.Domain.Enums;
using Domain.Interfaces;
using Domain.Entities;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de contas.
    /// </summary>
    public interface IPortfolioRepository : IRepository<PortfolioEntity>
    {
        /// <summary>
        /// Método responsável por retornar a conta com base na UK.
        /// </summary>
        /// <param name="userId">Identificador do usuário</param>
        /// <param name="nome">Nome da conta.</param>
        /// <param name="status">Status da conta</param>
        /// <returns>Entidade de conta <see cref="PortfolioEntity"/>.</returns>
        Task<PortfolioEntity> SelectByUkAsync(int userId, string nome, StatusType status);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="portfolioEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentAccount(PortfolioEntity portfolioEntity);
    }
}