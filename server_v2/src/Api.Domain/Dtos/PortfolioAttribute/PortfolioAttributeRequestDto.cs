using System.ComponentModel.DataAnnotations;
using System;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.PortfolioAttribute
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de Portfolio Attribute nas requisições.
    /// </summary>
    public class PortfolioAttributeRequestDto : BaseDto
    {
        /// <summary>
        /// Valor numérico do atributo (usado para atributos do tipo Number).
        /// </summary>
        public decimal? ValueNumber { get; set; }

        /// <summary>
        /// Valor textual do atributo (usado para atributos do tipo Text).
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Valor booleano do atributo (usado para atributos do tipo Boolean).
        /// </summary>
        public bool? ValueBoolean { get; set; }

        /// <summary>
        /// Valor de data do atributo (usado para atributos do tipo Date).
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// Identificador da opção de atributo (usado para atributos do tipo ListOption).
        /// </summary>
        public AttributeOptionRequestDto AttributeOption { get; set; }

        /// <summary>
        /// Tipo de ação do atributo <see cref="AttributeActionType"/>.
        /// </summary>
        [Required(ErrorMessage = "ActionType é obrigatório")]
        public int ActionType { get; set; }
        
        /// <summary>
        /// Identificador do portfólio associado.
        /// </summary>
        [Required(ErrorMessage = "PortfolioId é obrigatório")]
        public PortfolioRequestDto Portfolio { get; set; }

        /// <summary>
        /// Status do atributo do portfólio <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; } = (int)StatusType.Ativo;
    }
}