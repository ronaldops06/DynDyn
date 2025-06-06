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
        public int Month { get; set; }
        
        /// <summary>
        /// Ano correspondente ao saldo.
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// Identificador da conta correspondente ao saldo.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Conta correspondenta ao saldo.
        /// </summary>
        public PortfolioEntity Portfolio { get; set; }
        
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