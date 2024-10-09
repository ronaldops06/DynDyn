using System.ComponentModel.DataAnnotations;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Category
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de categoria nas requisições.
    /// </summary>
    public class CategoryRequestDto : BaseDto
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Tipo da categoria <see cref="CategoryType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(1, 2, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Type { get; set; }

        /// <summary>
        /// Status da categoria <see cref="StatusType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }
    }
}