using System;

namespace Somnia.API.V1.Dtos
{
    public class SaldoDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public double Valor { get; set; }
        public double Acumulado { get; set; }
        public double Valorizacao { get; set; }
        public double Dividendos { get; set; }
        public double Rendimento { get; set; }
        public double Credito { get; set; }
        public double Debito { get; set; }
        public double Entrada { get; set; }
        public double Saida { get; set; }
        public DateTime DataSaldo { get; set; }
        public ContaDTO Conta { get; set; }
    }
}
