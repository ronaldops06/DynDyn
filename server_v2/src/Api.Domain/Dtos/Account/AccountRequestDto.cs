using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Account
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de conta nas requisições.
    /// </summary>
    public class AccountRequestDto : BaseDto
    {
        /// <summary>
        /// Nome da conta.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }

        /// <summary>
        /// Categoria da conta <see cref="CategoryRequestDto"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public CategoryRequestDto Category { get; set; }

        /// <summary>
        /// Conta pai da conta em questão <see cref="AccountRequestDto"/>.
        /// </summary>
        public AccountRequestDto ParentAccount { get; set; }
    }
}