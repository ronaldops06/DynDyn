using System;
using Domain.Entities;

namespace Api.Domain.Entities
{
    /// <summary>
    /// Entidade de saldos.
    /// </summary>
    public class BalanceEntity : BaseEntity
    {
        /// <summary>
        /// Valor total do saldo no mês.
        /// </summary>
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
        public DateTime BalanceDate { get; set; }
        
        /// <summary>
        /// Identificador da conta correspondente ao saldo.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Conta correspondenta ao saldo.
        /// </summary>
        public AccountEntity Account { get; set; }
    }
}