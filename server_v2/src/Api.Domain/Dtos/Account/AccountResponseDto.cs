using Api.Domain.Dtos.Category;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Account
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de conta nas requisições.
    /// </summary>
    public class AccountResponseDto : BaseDto
    {
        /// <summary>
        /// Nome da conta.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Categoria da conta <see cref="CategoryResponseDto"/>.
        /// </summary>
        public CategoryResponseDto Category { get; set; }

        /// <summary>
        /// Conta pai da conta em questão <see cref="AccountResponseDto"/>.
        /// </summary>
        public AccountResponseDto ParentAccount { get; set; }
    }
}