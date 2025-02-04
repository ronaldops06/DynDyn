using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de saldo no banco de dados.
    /// </summary>
    public class BalanceMap
    {
        public void Configure(EntityTypeBuilder<BalanceEntity> builder)
        {
            builder.ToTable("Balance");

            builder.HasKey(u => u.Id);
            
            builder.HasIndex(u => new { u.Year, u.Month, u.AccountId, u.UserId })
                .IsUnique();
            
            builder.Property(u => u.Value)
                .IsRequired();

            builder.Property(u => u.Valuation);
            
            builder.Property(u => u.Dividends);
            
            builder.Property(u => u.Income);
            
            builder.Property(u => u.PercentageValuation);
            
            builder.Property(u => u.PercentageIncome);
            
            builder.Property(u => u.Credit);
            
            builder.Property(u => u.Debit);
            
            builder.Property(u => u.SalaryCredit);
            
            builder.Property(u => u.SalaryDebit);
            
            builder.Property(u => u.SalaryDebit);
            
            builder.Property(u => u.Inflow);
            
            builder.Property(u => u.Outflow);

            builder.HasOne(u => u.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .IsRequired();
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}