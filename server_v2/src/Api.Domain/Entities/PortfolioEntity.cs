using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// Entidade principal para o portfólio.
    /// </summary>
    public class PortfolioEntity : BaseEntity
    {
        /// <summary>
        /// Tipo de portfólio <see cref="PortfolioType"/>. 
        /// </summary>
        public PortfolioType Type { get; set; }
        
        /// <summary>
        /// Grupo de portfólio <see cref="PortfolioGroupType"/>.
        /// </summary>
        public PortfolioGroupType Group { get; set; }
        
        /// <summary>
        /// Nome da conta.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status da conta <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;
        
        // <summary>
        /// Identificador da categoria para conta.
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// Categoria para conta <see cref="CategoryEntity"/>.
        /// </summary>
        public CategoryEntity Category { get; set; }
        
        /// <summary>
        /// Identificador do registro pai do grupo de portfólio em questão.
        /// </summary>
        public int? ParentPortfolioId { get; set; }

        /// <summary>
        /// Registro pai do grupo de portfólio em questão.
        /// </summary>
        public PortfolioEntity ParentPortfolio { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base.
        /// </summary>
        public UserEntity User { get; set; } 
    }
}