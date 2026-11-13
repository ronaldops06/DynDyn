using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de impactos da transação no banco de dados.
    /// </summary>
    public class TransactionImpactMap
    {
        public void Configure(EntityTypeBuilder<TransactionImpactEntity> builder)
        {
            builder.ToTable("TransactionImpact");

            builder.HasKey(u => u.Id);
            
            builder.HasIndex(u => new { u.UserId, u.TransactionId, u.PortfolioId, u.PortfolioAttributeId })
                .IsUnique();

            builder.Property(u => u.ImpactType)
                .IsRequired();

            builder.Property(u => u.OperationType)
                .IsRequired();

            builder.Property(u => u.Value)
                .IsRequired();

            builder.Property(u => u.DataCriacao);

            builder.Property(u => u.DataAlteracao);

            builder.HasOne(u => u.Portfolio)
                .WithMany()
                .HasForeignKey(e => e.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(u => u.PortfolioAttribute)
                .WithMany()
                .HasForeignKey(e => e.PortfolioAttributeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
