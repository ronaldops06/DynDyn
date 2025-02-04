using Api.Domain.Enums;
using Domain.Models;

namespace Api.Domain.Models
{
    /// <summary>
    /// Objeto de modelo da categoria.
    /// </summary>
    public class CategoryModel : BaseModel
    {
        /// <summary>
        /// Nome da categoria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tipo da categoria <see cref="CategoryType"/>.
        /// </summary>
        public CategoryType Type { get; set; }

        /// <summary>
        /// Status da categoria <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base
        /// </summary>
        public UserModel User { get; set; }
    }
}