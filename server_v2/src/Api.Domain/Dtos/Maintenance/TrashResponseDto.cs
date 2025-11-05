namespace Api.Domain.Dtos.Maintenance
{
    public class TrashResponseDto
    {
        /// <summary>
        /// Tipo (entidade) referente ao lixo.
        /// </summary>
        public string Reference { get; set; }
        
        /// <summary>
        /// Identificador do registro na entidade referente ao lixo.
        /// </summary>
        public int ReferenceId { get; set; }
    }
}