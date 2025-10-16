using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class NotificationMap
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.DeviceToken)
                .IsRequired();

            builder.Property(u => u.Title)
                .IsRequired();
            
            builder.Property(u => u.Message)
                .IsRequired();
            
            builder.Property(u => u.Sent)
                .IsRequired();
        }
    }
}