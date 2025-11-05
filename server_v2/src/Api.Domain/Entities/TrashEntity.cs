namespace Domain.Entities
{
    public class TrashEntity : BaseEntity
    {
        public string Reference { get; set; }
        public int ReferenceId { get; set; }
        
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