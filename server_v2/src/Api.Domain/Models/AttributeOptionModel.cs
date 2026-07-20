using Api.Domain.Enums;
using Domain.Models;
using System;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da opção de atributo - armazena as opções possíveis para atributos do tipo ListOption.
    /// </summary>
    public class AttributeOptionModel : BaseModel
    {
        /// <summary>
        /// Rótulo/nome da opção.
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// Indica se a opção é default.
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Status da opção <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;
        
        /// <summary>
        /// Identificador do atributo associado.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Atributo associado <see cref="AttributeModel"/>.
        /// </summary>
        public AttributeModel Attribute { get; set; }
        
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
