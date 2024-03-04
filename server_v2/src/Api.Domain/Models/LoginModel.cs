namespace Domain.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }
    }
}
