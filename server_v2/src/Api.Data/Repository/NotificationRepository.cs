using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class NotificationRepository: BaseRepository<NotificationEntity>, INotificationRepository, ICleanupRepository
    {
        public NotificationRepository(SomniaContext context) : base(context)
        {
        }

        public int CleanupOrder => 1;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var devices = await _context.Device.Where(d => d.UserId == userId).ToListAsync();

                foreach (var device in devices)
                {
                    var registros = await _context.Notification.Where(x => x.DeviceToken == device.NotificationToken).ToListAsync();
                    _context.Notification.RemoveRange(registros);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }
}