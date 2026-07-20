using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de Portfolio Attribute no banco de dados.
    /// </summary>
    public class PortfolioAttributeMap
    {
        public void Configure(EntityTypeBuilder<PortfolioAttributeEntity> builder)
        {
            builder.ToTable("PortfolioAttribute");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.PortfolioId, u.AttributeId })
                .IsUnique();

            builder.Property(u => u.ValueNumber)
                .HasColumnType("numeric(18,2)");

            builder.Property(u => u.ValueText)
                .HasMaxLength(500);

            builder.Property(u => u.ValueBoolean);

            builder.Property(u => u.ValueDate);

            builder.Property(u => u.ActionType)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasOne(u => u.Attribute)
                .WithMany()
                .HasForeignKey(e => e.AttributeId)
                .IsRequired();
            
            builder.HasOne(u => u.AttributeOption)
                .WithMany()
                .HasForeignKey(e => e.AttributeOptionId)
                .IsRequired(false);

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}