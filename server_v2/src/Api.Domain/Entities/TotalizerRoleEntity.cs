using System.Collections.Generic;
using Api.Domain.Enums;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de papel de totalizador.
    /// </summary>
    public class TotalizerRoleEntity : BaseEntity
    {
        /// <summary>
        /// Código da regra de totalizador.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Tipo da regra de totalizador.
        /// </summary>
        public TotalizerType Type { get; set; }
        
        /// <summary>
        /// Lista de papeis de operação.
        /// </summary>
        public List<TotalizerRoleLinkEntity> OperationRoles { get; set; }
        
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