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
        public string Nome { get; set; }

        /// <summary>
        /// Tipo da categoria <see cref="CategoryType"/>.
        /// </summary>
        public CategoryType Tipo { get; set; }

        /// <summary>
        /// Status da categoria <see cref="StatusType"/>
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;
    }
}