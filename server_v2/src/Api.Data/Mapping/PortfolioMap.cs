using Api.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class PortfolioMap
    {
        public void Configure(EntityTypeBuilder<PortfolioEntity> builder)
        {
            builder.ToTable("Portfolio");
            
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.Type, u.Group, u.Name, u.Status, u.UserId })
                .IsUnique();
            
            builder.Property(u => u.Type)
                .IsRequired();
            
            builder.Property(u => u.Group)
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Status)
                .IsRequired();
            
            builder.HasOne(u => u.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.ParentPortfolio)
                .WithMany()
                .HasForeignKey(e => e.ParentPortfolioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
        
    }
}