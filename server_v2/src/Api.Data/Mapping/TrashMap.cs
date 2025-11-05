using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class TrashMap
    {
        public void Configure(EntityTypeBuilder<TrashEntity> builder)
        {
            builder.ToTable("Trash");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => new { u.Reference, u.ReferenceId, u.UserId })
                .IsUnique();
            
            builder.Property(u => u.Reference)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.ReferenceId)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}