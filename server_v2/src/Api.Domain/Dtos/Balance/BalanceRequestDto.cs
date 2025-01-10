using System;
using System.ComponentModel.DataAnnotations;
using Api.Domain.Dtos.Account;

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
        [Range(0.000001, double.MaxValue)]
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
        /// Data base do saldo (corresponde ao mês).
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public DateTime BalanceDate { get; set; }
        
        /// <summary>
        /// Conta correspondenta ao saldo.
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public AccountRequestDto Account { get; set; }
    }
}