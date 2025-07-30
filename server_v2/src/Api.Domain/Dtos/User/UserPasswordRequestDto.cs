using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.User
{
    public class UserPasswordRequestDto
    {
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(500, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(500, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string NewPassword { get; set; }
    }
}