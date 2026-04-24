using Api.Domain.Entities;

namespace Domain.Entities
{
    /// <summary>
    /// Relacionamento entre operações e papeis de operação.
    /// </summary>
    public class TotalizerRoleLinkEntity
    {
        /// <summary>
        /// Identificador do papel de totalizador.
        /// </summary>
        public int TotalizerRoleId { get; set; }

        /// <summary>
        /// Papel de totalizador relacionado <see cref="TotalizerRoleEntity"/>
        /// </summary>
        public TotalizerRoleEntity TotalizerRole { get; set; }
        
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