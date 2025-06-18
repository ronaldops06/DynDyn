using System;

namespace Domain.Dtos.User
{
    public class LoginResponseDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string AccessToken { get; set; }
    }
}
