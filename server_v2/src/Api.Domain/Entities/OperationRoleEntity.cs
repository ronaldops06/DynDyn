using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de regras transação.
    /// </summary>
    public class OperationRoleEntity : BaseEntity
    {
        /// <summary>
        /// Nome da regra de transação.
        /// </summary>
        public string Name { get; set; }
        
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