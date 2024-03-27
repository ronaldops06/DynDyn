using Api.Domain.Enums;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de conta.
    /// </summary>
    public class AccountEntity : BaseEntity
    {
        /// <summary>
        /// Nome da conta.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;

        /// <summary>
        /// Identificador da categoria da conta.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Categoria da conta <see cref="CategoryEntity"/>.
        /// </summary>
        public CategoryEntity Category { get; set; }

        /// <summary>
        /// Identificador da conta pai da conta em questão.
        /// </summary>
        public int? ParentAccountId { get; set; }

        /// <summary>
        /// Conta pai da conta em questão <see cref="AccountEntity"/>.
        /// </summary>
        public AccountEntity ParentAccount { get; set; }
    }
}