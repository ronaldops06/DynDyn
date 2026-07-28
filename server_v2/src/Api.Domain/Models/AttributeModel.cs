using System.Collections.Generic;
using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo do atributo.
    /// </summary>
    public class AttributeModel : BaseModel
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
        public AttributeDataType DataType { get; set; }
        
        /// <summary>
        /// Status do atributo <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;
        
        /// <summary>
        /// Lista de opções, caso o tipo for AttributeDataType.ListOptions <see cref="AttributeOptionModel"/>
        /// </summary>
        public List<AttributeOptionModel> Options { get; set; }

        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base.
        /// </summary>
        public UserModel User { get; set; }
    }
}
