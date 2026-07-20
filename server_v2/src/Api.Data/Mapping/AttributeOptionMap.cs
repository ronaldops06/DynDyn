using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de Attribute Option no banco de dados.
    /// </summary>
    public class AttributeOptionMap
    {
        public void Configure(EntityTypeBuilder<AttributeOptionEntity> builder)
        {
            builder.ToTable("AttributeOption");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.AttributeId, u.Label })
                .IsUnique();

            builder.Property(u => u.Label)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.IsDefault)
                .IsRequired();
            
            builder.Property(u => u.Status)
                .IsRequired();
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
