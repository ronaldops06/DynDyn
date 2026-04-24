using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.Operation
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de papeis de operação nas requisições.
    /// </summary>
    public class OperationRoleRequestDto : BaseDto
    {
        /// <summary>
        /// Nome da regra de transação.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }
    }
}