using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Operation
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de operação nas requisições.
    /// </summary>
    public class OperationRequestDto : BaseDto
    {
        /// <summary>
        /// Nome da operação.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Indica se a operação acontece de forma recorrente.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public bool Recurrent { get; set; }
        
        /// <summary>
        /// Indica se a operação é uma operação relativa a salário.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public bool Salary { get; set; }

        /// <summary>
        /// Tipo da operação <see cref="OperationType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(1, 3, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Type { get; set; }

        /// <summary>
        /// Status da operação <see cref="StatusType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }

        /// <summary>
        /// Categoria da operação <see cref="CategoryRequestDto"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public CategoryRequestDto Category { get; set; }
    }
}