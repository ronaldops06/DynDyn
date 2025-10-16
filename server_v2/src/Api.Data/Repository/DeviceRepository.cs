using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class DeviceRepository: BaseRepository<DeviceEntity>, IDeviceRepository, ICleanupRepository
    {
        public DeviceRepository(SomniaContext context) : base(context) { }
        
        public int CleanupOrder => 6;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Device.Where(x => x.UserId == userId).ToListAsync();
                _context.Device.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        public override async Task<IEnumerable<DeviceEntity>> SelectAsync(int userId)
        {
            var result = new List<DeviceEntity>();

            try
            {
                IQueryable<DeviceEntity> query = _context.Device;
                
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o dispositivo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<DeviceEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new DeviceEntity();

            try
            {
                IQueryable<DeviceEntity> query = _context.Device;
                
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o dispositivo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<Data<DeviceEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<DeviceEntity> query = _context.Device;
            
            query = query.Include(usr => usr.User);

            query = query.Where(x => x.UserId == userId);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<DeviceEntity> SelectByUkAsync(int userId, string physicalDeviceId)
        {
            var result = new DeviceEntity();

            try
            {
                IQueryable<DeviceEntity> query = _context.Device;
                
                query = query.Include(act => act.User);
                query = query.AsNoTracking()
                    .Where(x => x.PhisicalDeviceId == physicalDeviceId && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o dispositivo: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentDevice(DeviceEntity deviceEntity)
        {
            if (deviceEntity.User != null)
            {
                _context.Entry(deviceEntity.User).State = EntityState.Unchanged;
            }
        }
    }
}