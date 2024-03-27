using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Interfaces;

namespace Api.Domain.Repository
{
    /// <summary>
    /// Interface para o repositório de contas.
    /// </summary>
    public interface IAccountRepository : IRepository<AccountEntity>
    {
        /// <summary>
        /// Método responsável por retornar a conta com base na UK.
        /// </summary>
        /// <param name="nome">Nome da conta.</param>
        /// <returns>Entidade de conta <see cref="AccountEntity"/>.</returns>
        Task<AccountEntity> SelectByUkAsync(string nome);

        /// <summary>
        /// Método responsável por alterar o estado das entidades dependentes para que não ocorra erro ao salvar a entidade principal.
        /// </summary>
        /// <param name="accountEntity">Entidade a ter os dependetes com status alterado.</param>
        void UnchangedParentAccount(AccountEntity accountEntity);
    }
}