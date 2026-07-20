using Api.Domain.Enums;
using Api.Domain.Dtos.Attribute;
using System;

namespace Api.Domain.Dtos.PortfolioAttribute
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de Portfolio Attribute nas requisições.
    /// </summary>
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
        public bool? ValueBoolean { get; set; }

        /// <summary>
        /// Valor de data do atributo (usado para atributos do tipo Date).
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// Identificador da opção de atributo (usado para atributos do tipo ListOption).
        /// </summary>
        public int? AttributeOptionId { get; set; }

        /// <summary>
        /// Opção de atributo selecionada (usado para atributos do tipo ListOption).
        /// </summary>
        public object AttributeOption { get; set; }

        /// <summary>
        /// Tipo de ação do atributo <see cref="AttributeActionType"/>.
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// Identificador do atributo associado.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Atributo associado (resumido).
        /// </summary>
        public object Attribute { get; set; }

        /// <summary>
        /// Identificador do portfólio associado.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Status do atributo do portfólio <see cref="StatusType"/>.
        /// </summary>
        public int Status { get; set; }
    }
}