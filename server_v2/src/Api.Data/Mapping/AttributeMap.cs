using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de atributo no banco de dados.
    /// </summary>
    public class AttributeMap
    {
        public void Configure(EntityTypeBuilder<AttributeEntity> builder)
        {
            builder.ToTable("Attribute");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.Name, u.UserId })
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Description)
                .HasMaxLength(500);

            builder.Property(u => u.DataType)
                .IsRequired();
            
            builder.Property(u => u.Status)
                .IsRequired();
            
            builder.HasMany(a => a.Options)
                .WithOne(l => l.Attribute)
                .HasForeignKey(l => l.AttributeId);
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
