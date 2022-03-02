namespace Somnia.API.Models
{
    public class User : ModelBase
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
