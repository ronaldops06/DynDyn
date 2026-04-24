using System.Collections.Generic;
using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da papel de totalizador.
    /// </summary>
    public class TotalizerRoleModel : BaseModel
    {
        /// <summary>
        /// Nome da regra de totalizador.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Tipo da regra de totalizador.
        /// </summary>
        public TotalizerType Type { get; set; }
        
        /// <summary>
        /// Lista de papeis de operação.
        /// </summary>
        public List<OperationRoleModel> OperationRoles { get; set; }
        
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