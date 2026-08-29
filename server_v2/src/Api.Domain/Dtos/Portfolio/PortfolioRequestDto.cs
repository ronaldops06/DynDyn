using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Category;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Portfolio
{
    public class PortfolioRequestDto : BaseDto
    {
        /// <summary>
        /// Tipo de portfólio <see cref="PortfolioType"/>. 
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public PortfolioType Type { get; set; }
        
        /// <summary>
        /// Grupo de portfólio <see cref="PortfolioGroupType"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public PortfolioGroupType Group { get; set; }
        
        /// <summary>
        /// Nome da conta.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [StringLength(100, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Name { get; set; }
        
        /// <summary>
        /// Descrição do portfólio.
        /// </summary>
        [StringLength(250, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
        public string Description { get; set; }
        
        /// <summary>
        /// Código da moeda do portfólio.
        /// </summary>
        [StringLength(3, ErrorMessage = "{0} deve ter no máximo {1} caracteres")]
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
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [Range(0, 1, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public int Status { get; set; }
        
        /// <summary>
        /// Categoria da conta <see cref="CategoryRequestDto"/>.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public CategoryRequestDto Category { get; set; }
        
        /// <summary>
        /// Conta pai da conta em questão <see cref="PortfolioRequestDto"/>.
        /// </summary>
        public PortfolioRequestDto ParentPortfolio { get; set; }
        
        /// <summary>
        /// Lista de atributos <see cref="PortfolioAttributeEntity"/>
        /// </summary>
        public IEnumerable<PortfolioAttributeRequestDto> Attributes { get; set; }
    }
}