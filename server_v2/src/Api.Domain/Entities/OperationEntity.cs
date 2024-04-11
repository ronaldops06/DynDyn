using Api.Domain.Enums;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de operação.
    /// </summary>
    public class OperationEntity : BaseEntity
    {
        /// <summary>
        /// Nome da operação.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indica se a operação acontece de forma recorrente.
        /// </summary>
        public bool Recurrent { get; set; }

        /// <summary>
        /// Tipo da operação <see cref="OperationType"/>.
        /// </summary>
        public OperationType Type { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;

        /// <summary>
        /// Identificador da categoria da operação.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Categoria da operação <see cref="CategoryEntity"/>.
        /// </summary>
        public CategoryEntity Category { get; set; }
    }
}