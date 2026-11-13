using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Portfolio;

namespace Api.Domain.Dtos.Transaction
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de impacto da transação nas requisições.
    /// </summary>
    public class TransactionImpactRequestDto : BaseDto
    {
        /// <summary>
        /// Portfólio impactado.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public PortfolioRequestDto Portfolio { get; set; }

        /// <summary>
        /// Atributo do portfólio impactado.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public PortfolioAttributeRequestDto PortfolioAttribute { get; set; }

        /// <summary>
        /// Tipo de impacto.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int ImpactType { get; set; }

        /// <summary>
        /// Tipo de operação do impacto.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int OperationType { get; set; }

        /// <summary>
        /// Valor do impacto.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public double Value { get; set; }
    }
}
