using System;

namespace Domain.Models
{
    public class UserModel : BaseModel
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }
    }
}