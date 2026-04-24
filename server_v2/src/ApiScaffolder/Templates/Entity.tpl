using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de {{name}}.
    /// </summary>
    public class {{model}}Entity : BaseEntity
    {
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