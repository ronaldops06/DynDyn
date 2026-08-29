using System;
using System.Collections.Generic;
using Api.Domain.Dtos.Category;
using Api.Domain.Entities;
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
        /// Descrição do portfólio.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Código da moeda do portfólio.
        /// </summary>
        public string CurrencyCode { get; set; }
        
        /// <summary>
        /// Valor de aquisição do portfólio.
        /// </summary>
        public double? AcquisitionCost { get; set; }
        
        /// <summary>
        /// Data de encerramento do portfólio.
        /// </summary>
        public DateTime? EndDate { get; set; }

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
        
        /// <summary>
        /// Lista de atributos <see cref="PortfolioAttributeEntity"/>
        /// </summary>
        public IEnumerable<PortfolioAttributeResponseDto> Attributes { get; set; }
    }
}