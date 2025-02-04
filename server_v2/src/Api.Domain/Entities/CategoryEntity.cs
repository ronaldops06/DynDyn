using Api.Domain.Enums;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de categoria.
    /// </summary>
    public class CategoryEntity : BaseEntity
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tipo da categoria <see cref="CategoryType"/>.
        /// </summary>
        public CategoryType Type { get; set; }

        /// <summary>
        /// Status da categoria <see cref="StatusType"/>
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base
        /// </summary>
        public UserEntity User { get; set; }
    }
}