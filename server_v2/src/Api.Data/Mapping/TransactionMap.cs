using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de transação no banco de dados.
    /// </summary>
    public class TransactionMap
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Value)
                .IsRequired();

            builder.Property(u => u.Observation)
                .HasMaxLength(200);

            builder.Property(u => u.Consolidated)
                .IsRequired();

            builder.Property(u => u.Installment);

            builder.Property(u => u.TotalInstallments);

            builder.Property(u => u.DataCriacao);

            builder.Property(u => u.DataAlteracao);

            builder.HasOne(u => u.ParentTransaction)
                  .WithMany()
                  .HasForeignKey(e => e.ParentTransactionId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Account)
                  .WithMany()
                  .HasForeignKey(e => e.AccountId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();

            builder.HasOne(u => u.DestinationAccount)
                  .WithMany()
                  .HasForeignKey(e => e.DestinationAccountId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Operation)
                  .WithMany()
                  .HasForeignKey(e => e.OperationId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired();
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}