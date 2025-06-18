using System;

namespace Domain.Entities
{
    /// <summary>
    /// Entidade para cadastro de usuário em validação.
    /// </summary>
    public class TransientUserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int VerificationCode { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}