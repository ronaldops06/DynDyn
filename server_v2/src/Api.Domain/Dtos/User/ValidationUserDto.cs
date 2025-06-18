namespace Domain.Dtos.User
{
    /// <summary>
    /// Objeto de validação do usuário.
    /// </summary>
    public class ValidationUserDto
    {
        /// <summary>
        /// Identificação de login do usuário.
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Código de verificação para ativação do usuário.
        /// </summary>
        public int VerificationCode { get; set; }
    }
}