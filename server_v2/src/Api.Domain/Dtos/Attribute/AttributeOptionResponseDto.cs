using Api.Domain.Enums;

namespace Api.Domain.Dtos.Attribute
{
    public class AttributeOptionResponseDto : BaseDto
    {
        /// <summary>
        /// Rótulo/nome da opção.
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// Indica se a opção é default.
        /// </summary>
        public int IsDefault { get; set; }
        
        /// <summary>
        /// Status da opção <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }
    }
}