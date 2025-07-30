using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.User
{
    /// <summary>
    /// Objeto de validação do usuário.
    /// </summary>
    public class ValidationUserRequestDto
    {
        /// <summary>
        /// Identificação de login do usuário.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(500, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Login { get; set; }
        
        /// <summary>
        /// Código de verificação para ativação do usuário.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(100000, 999999, ErrorMessage = "{0} deve estar entre {1} até {2}")]
        public int VerificationCode { get; set; }
    }
}