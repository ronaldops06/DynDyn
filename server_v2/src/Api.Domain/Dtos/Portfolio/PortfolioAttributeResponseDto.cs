using System;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Enums;

namespace Api.Domain.Dtos.Portfolio
{
    public class PortfolioAttributeResponseDto : BaseDto
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
        public SituationType? ValueBoolean { get; set; }

        /// <summary>
        /// Valor de data do atributo (usado para atributos do tipo Date).
        /// </summary>
        public DateTime? ValueDate { get; set; }
        
        /// <summary>
        /// Opção de atributo selecionada (usado para atributos do tipo ListOption).
        /// </summary>
        public AttributeOptionResponseDto AttributeOption { get; set; }

        /// <summary>
        /// Tipo de ação do atributo <see cref="AttributeActionType"/>.
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// Atributo associado <see cref="AttributeRequestDto"/>.
        /// </summary>
        public AttributeResponseDto Attribute { get; set; }
        
        /// <summary>
        /// Status do atributo do portfólio.
        /// </summary>
        public int Status { get; set; }
    }
}