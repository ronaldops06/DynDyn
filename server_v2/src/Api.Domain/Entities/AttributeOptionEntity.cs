using Api.Domain.Enums;
using Domain.Entities;
using System;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de opção de atributo - armazena as opções possíveis para atributos do tipo ListOption.
    /// </summary>
    public class AttributeOptionEntity : BaseEntity
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
        /// Atributo associado <see cref="AttributeEntity"/>.
        /// </summary>
        public AttributeEntity Attribute { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Usuário base.
        /// </summary>
        public UserEntity User { get; set; }
    }
}
