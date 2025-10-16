namespace Domain.Dtos.User
{
    /// <summary>
    /// Objeto de validação do usuário.
    /// </summary>
    public class ValidationUserResponseDto
    {
        /// <summary>
        /// Token de verificação para recuperação de senha.
        /// </summary>
        public string VerificationToken { get; set; }
    }
}