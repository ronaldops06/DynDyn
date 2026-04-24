using Api.Domain.Entities;

namespace Domain.Entities
{
    /// <summary>
    /// Relacionamento entre operações e papeis de operação.
    /// </summary>
    public class OperationRoleLinkEntity
    {
        /// <summary>
        /// Identificador da operação.
        /// </summary>
        public int OperationId { get; set; }

        /// <summary>
        /// operação relacionada <see cref="OperationEntity"/>
        /// </summary>
        public OperationEntity Operation { get; set; }
        
        /// <summary>
        /// Identificador do papel de operação.
        /// </summary>
        public int OperationRoleId { get; set; }

        /// <summary>
        /// Papel de operação relacionada <see cref="OperationRoleEntity"/>
        /// </summary>
        public OperationRoleEntity OperationRole { get; set; }
    }
}