using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da regras operação.
    /// </summary>
    public class OperationRoleModel : BaseModel
    {
        /// <summary>
        /// Nome da regra de operação.
        /// </summary>
        public string Name { get; set; }
        
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