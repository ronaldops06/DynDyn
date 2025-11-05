namespace Domain.Models
{
    public class TrashModel: BaseModel
    {
        /// <summary>
        /// Tipo (entidade) referente ao lixo.
        /// </summary>
        public string Reference { get; set; }
        
        /// <summary>
        /// Identificador do registro na entidade referente ao lixo.
        /// </summary>
        public int ReferenceId { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base
        /// </summary>
        public UserModel User { get; set; }
    }
}