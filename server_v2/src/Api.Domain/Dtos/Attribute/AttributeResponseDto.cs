using System.Collections.Generic;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Attribute
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de atributo nas requisições.
    /// </summary>
    public class AttributeResponseDto : BaseDto
    {
        /// <summary>
        /// Nome do atributo.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descrição do atributo.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tipo de dado do atributo <see cref="AttributeDataType"/>.
        /// </summary>
        public int DataType { get; set; }
        
        /// <summary>
        /// Status do atributo <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// Lista de opções, caso o tipo for AttributeDataType.ListOptions <see cref="AttributeOptionResponseDto"/>
        /// </summary>
        public IEnumerable<AttributeOptionResponseDto> Options { get; set; }
    }
}
