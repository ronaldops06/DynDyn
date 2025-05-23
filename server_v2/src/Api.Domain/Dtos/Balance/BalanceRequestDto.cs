using System;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Portfolio;

namespace Api.Domain.Dtos.Balance
{
    /// <summary>
    /// Objeto de transferência de dados para o recebimento de saldos nas requisições.
    /// </summary>
    public class BalanceRequestDto : BaseDto
    {
        /// <summary>
        /// Valor total do saldo no mês.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public Double Value { get; set; }
        
        /// <summary>
        /// Total de valorização no mẽs.
        /// </summary>
        public Double? Valuation { get; set; }
        
        /// <summary>
        /// Total de dividendos no mês.
        /// </summary>
        public Double? Dividends { get; set; }
        
        /// <summary>
        /// Total de rendimentos no mês.
        /// </summary>
        public Double? Income { get; set; }
        
        /// <summary>
        /// Percentual de valorização.
        /// </summary>
        public Double? PercentageValuation { get; set; }
        
        /// <summary>
        /// Percentual de rendimentos.
        /// </summary>
        public Double? PercentageIncome { get; set; }
        
        /// <summary>
        /// Total de crédito.
        /// </summary>
        public Double Credit { get; set; }
        
        /// <summary>
        /// Total de débito.
        /// </summary>
        public Double Debit { get; set; }
        
        /// <summary>
        /// Total de crédido específico de salário. 
        /// </summary>
        public Double? SalaryCredit { get; set; }

        /// <summary>
        /// Total de débido específico de salário. 
        /// </summary>
        public Double? SalaryDebit { get; set; }
        
        /// <summary>
        /// Total de entradas, considerando salário, transferência e todo o resto
        /// </summary>
        public Double Inflow { get; set; }
        
        /// <summary>
        /// Total de saídas, considerando salário, transferência e todo o resto
        /// </summary>
        public Double Outflow { get; set; }
        
        /// <summary>
        /// Mẽs correspondente ao saldo.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int Month { get; set; }
        
        /// <summary>
        /// Ano correspondente ao saldo.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public int Year { get; set; }
        
        /// <summary>
        /// Conta correspondenta ao saldo.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public PortfolioRequestDto Portfolio { get; set; }
    }
}