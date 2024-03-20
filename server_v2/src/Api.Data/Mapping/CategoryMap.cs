using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de categorias no banco de dados.
    /// </summary>
    public class CategoryMap
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Nome)
                .IsUnique();

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Tipo)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();
        }
    }
}