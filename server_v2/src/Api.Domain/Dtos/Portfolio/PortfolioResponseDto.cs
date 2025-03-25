using Api.Domain.Dtos.Category;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Portfolio
{
    public class PortfolioResponseDto : BaseDto
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
        public int Status { get; set; }
        
        /// <summary>
        /// Categoria da conta <see cref="CategoryResponseDto"/>.
        /// </summary>
        public CategoryResponseDto Category { get; set; }
        
        /// <summary>
        /// Conta pai da conta em questão <see cref="PortfolioResponseDto"/>.
        /// </summary>
        public PortfolioResponseDto ParentPortfolio { get; set; }
    }
}