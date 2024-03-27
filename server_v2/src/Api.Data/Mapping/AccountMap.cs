using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de contas no banco de dados.
    /// </summary>
    public class AccountMap
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Name)
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasOne(u => u.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .IsRequired();

            builder.HasOne(u => u.ParentAccount)
                  .WithMany()
                  .HasForeignKey(e => e.ParentAccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}