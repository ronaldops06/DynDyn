using System;

namespace Domain.Models
{
    public class TransientUserModel : BaseModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public int? VerificationCode { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}