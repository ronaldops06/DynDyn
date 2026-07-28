using Api.Domain.Enums;
using Domain.Entities;
using System;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de Portfolio Attribute - armazena valores de atributos para um portfólio.
    /// </summary>
    public class PortfolioAttributeEntity : BaseEntity
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
        public AttributeOptionEntity AttributeOption { get; set; }

        /// <summary>
        /// Tipo de ação do atributo <see cref="AttributeActionType"/>.
        /// </summary>
        public AttributeActionType ActionType { get; set; }

        /// <summary>
        /// Identificador do atributo associado.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Atributo associado <see cref="AttributeEntity"/>.
        /// </summary>
        public AttributeEntity Attribute { get; set; }

        /// <summary>
        /// Identificador do portfólio associado.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Portfólio associado <see cref="PortfolioEntity"/>.
        /// </summary>
        public PortfolioEntity Portfolio { get; set; }

        /// <summary>
        /// Status do atributo do portfólio <see cref="StatusType"/>.
        /// </summary>
        public StatusType Status { get; set; } = StatusType.Ativo;

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