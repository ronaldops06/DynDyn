using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da conta.
    /// </summary>
    public class AccountModel : BaseModel
    {
        /// <summary>
        /// Nome da conta.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// Identificador da categoria da conta.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Categoria da conta <see cref="CategoryModel"/>.
        /// </summary>
        public CategoryModel Category { get; set; }

        /// <summary>
        /// Identificador da conta pai da conta em questão.
        /// </summary>
        public int? ParentAccountId { get; set; }

        /// <summary>
        /// Conta pai da conta em questão <see cref="AccountModel"/>.
        /// </summary>
        public AccountModel ParentAccount { get; set; }

    }
}