using System.Collections.Generic;
using Api.Domain.Dtos.Operation;

namespace Api.Domain.Dtos.TotalizerRole
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de papel de totalizador nas requisições.
    /// </summary>
    public class TotalizerRoleResponseDto : BaseDto
    {
        /// <summary>
        /// Nome da regra de totalizador.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Tipo da regra de totalizador.
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// Lista de papeis de operação.
        /// </summary>
        public IEnumerable<OperationRoleResponseDto> OperationRoles { get; set; }
    }
}