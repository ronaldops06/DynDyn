using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "{0} é obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Login { get; set; }

        [Required(ErrorMessage = "{0} é obrigatória")]
        [StringLength(50, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Password { get; set; }
    }
}
