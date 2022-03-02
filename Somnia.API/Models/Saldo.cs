using System;

namespace Somnia.API.Models
{
    public class Saldo : ModelBase
    {
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
        public int ContaID { get; set; }
        public Conta Conta { get; set; }
    }
}
