using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de operações no banco de dados.
    /// </summary>
    public class OperationMap
    {
        public void Configure(EntityTypeBuilder<OperationEntity> builder)
        {
            builder.ToTable("Operation");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Name)
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Type)
                .IsRequired();

            builder.Property(u => u.Recurrent)
                .IsRequired();
            
            builder.Property(u => u.Salary)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasOne(u => u.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .IsRequired();
        }
    }
}