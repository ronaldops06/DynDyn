using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da {{name}}.
    /// </summary>
    public class {{model}}Model : BaseModel
    {
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base.
        /// </summary>
        public UserModel User { get; set; }
    }
}