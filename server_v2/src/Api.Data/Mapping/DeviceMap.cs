using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de dispositivo no banco de dados.
    /// </summary>
    public class DeviceMap
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
            builder.ToTable("Device");

            builder.HasKey(u => u.Id);
            
            builder.HasIndex(u => new { u.PhisicalDeviceId, u.UserId })
                .IsUnique();
            
            builder.Property(u => u.PhisicalDeviceId)
                .IsRequired();
            
            builder.Property(u => u.NotificationToken);
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}