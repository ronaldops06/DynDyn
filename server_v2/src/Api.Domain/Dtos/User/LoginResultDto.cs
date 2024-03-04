using System;

namespace Domain.Dtos.User
{
    public class LoginResultDto
    {
        public bool Authenticated { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
