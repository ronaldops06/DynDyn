using System;

namespace Domain.Dtos.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
