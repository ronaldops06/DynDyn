namespace Api.Domain.Dtos.Operation
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de papel de operação nas requisições.
    /// </summary>
    public class OperationRoleResponseDto : BaseDto
    {
        /// <summary>
        /// Nome do papel de transação.
        /// </summary>
        public string Name { get; set; }
    }
}