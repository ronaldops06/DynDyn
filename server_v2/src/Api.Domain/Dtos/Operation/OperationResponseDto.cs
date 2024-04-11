using Api.Domain.Dtos.Category;

namespace Api.Domain.Dtos.Operation
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de operação nas requisições.
    /// </summary>
    public class OperationResponseDto : BaseDto
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
        public int Type { get; set; }

        /// <summary>
        /// Status da operação <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Categoria da operação <see cref="CategoryResponseDto"/>.
        /// </summary>
        public CategoryResponseDto Category { get; set; }
    }
}