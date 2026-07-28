using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Attribute
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de atributo nas requisições.
    /// </summary>
    public class AttributeRequestDto : BaseDto
    {
        /// <summary>
        /// Nome do atributo.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Descrição do atributo.
        /// </summary>
        [StringLength(500, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// Tipo de dado do atributo <see cref="AttributeDataType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int DataType { get; set; }
        
        /// <summary>
        /// Status do atributo <see cref="StatusType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }
        
        /// <summary>
        /// Lista de opções, caso o tipo for AttributeDataType.ListOptions <see cref="AttributeOptionRequestDto"/>
        /// </summary>
        public IEnumerable<AttributeOptionRequestDto> Options { get; set; }
    }
}
