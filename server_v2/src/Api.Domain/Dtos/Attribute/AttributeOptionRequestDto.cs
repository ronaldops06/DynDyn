using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.Attribute
{
    public class AttributeOptionRequestDto : BaseDto
    {
        /// <summary>
        /// Rótulo/nome da opção.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Label { get; set; }
        
        /// <summary>
        /// Indica se a opção é default.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int IsDefault { get; set; }
        
        /// <summary>
        /// Status da opção.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }
    }
}