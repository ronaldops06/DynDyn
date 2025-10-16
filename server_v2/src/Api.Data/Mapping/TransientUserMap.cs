using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class TransientUserMap
    {
        public void Configure(EntityTypeBuilder<TransientUserEntity> builder)
        {
            builder.ToTable("TransientUser");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Login)
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.VerificationCode)
                .HasMaxLength(6);
            
            builder.Property(u => u.VerificationToken)
                .HasMaxLength(500);
            
            builder.Property(u => u.Attempts)
                .IsRequired();

            builder.Property(u => u.ExpirationDate);
        }
    }
}