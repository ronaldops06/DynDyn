namespace Api.Domain.Dtos.Category
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de categoria nas requisições.
    /// </summary>
    public class CategoryResponseDto : BaseDto
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tipo da categoria <see cref="CategoryType"/>.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Status da categoria <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }
    }
}