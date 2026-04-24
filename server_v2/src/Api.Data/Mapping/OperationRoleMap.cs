using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de papeis de transação no banco de dados.
    /// </summary>
    public class OperationRoleMap
    {
        public void Configure(EntityTypeBuilder<OperationRoleEntity> builder)
        {
            builder.ToTable("OperationRole");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.Name, u.UserId })
                 .IsUnique();

            builder.Property(u => u.Name)
                 .IsRequired()
                 .HasMaxLength(100);
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}