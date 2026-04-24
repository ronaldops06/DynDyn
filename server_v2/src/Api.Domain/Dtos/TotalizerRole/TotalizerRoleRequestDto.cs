using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Operation;

namespace Api.Domain.Dtos.TotalizerRole
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de papel de totalizador nas requisições.
    /// </summary>
    public class TotalizerRoleRequestDto : BaseDto
    {
        /// <summary>
        /// Nome da regra de totalizador.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(8, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Code { get; set; }
        
        /// <summary>
        /// Tipo da regra de totalizador.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int Type { get; set; }
        
        /// <summary>
        /// Lista de papeis de operação.
        /// </summary>
        public IEnumerable<OperationRoleRequestDto> OperationRoles { get; set; }
    }
}