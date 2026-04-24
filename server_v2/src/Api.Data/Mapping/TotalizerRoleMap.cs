using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de papel de totalizador no banco de dados.
    /// </summary>
    public class TotalizerRoleMap
    {
        public void Configure(EntityTypeBuilder<TotalizerRoleEntity> builder)
        {
            builder.ToTable("TotalizerRole");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.Code, u.Type, u.UserId})
                  .IsUnique();

            builder.Property(u => u.Code)
                  .IsRequired()
                  .HasMaxLength(8);
            
            builder.Property(u => u.Type)
                .IsRequired();
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            
            builder.HasMany(a => a.OperationRoles)
                .WithOne(l => l.TotalizerRole)
                .HasForeignKey(l => l.TotalizerRoleId);
        }
    }
}