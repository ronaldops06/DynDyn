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
    public class TrashRepository : BaseRepository<TrashEntity>, ITrashRepository, ICleanupRepository
    {
        public TrashRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 7;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Trash.Where(x => x.UserId == userId).ToListAsync();
                _context.Trash.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        public override async Task<IEnumerable<TrashEntity>> SelectAsync(int userId)
        {
            var result = new List<TrashEntity>();

            try
            {
                IQueryable<TrashEntity> query = _context.Trash;
                
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a lixeira: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<TrashEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new TrashEntity();

            try
            {
                IQueryable<TrashEntity> query = _context.Trash;
                
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a lixeira: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<TrashEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<TrashEntity> query = _context.Trash;
            
            query = query.Include(usr => usr.User);

            query = query.Where(x => x.UserId == userId);
            
            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<TrashEntity> SelectByUkAsync(int userId, string reference, int referenceId)
        {
            var result = new TrashEntity();

            try
            {
                IQueryable<TrashEntity> query = _context.Trash;
                
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Reference == reference && x.ReferenceId == referenceId && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a lixeira: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public void UnchangedParentTrash(TrashEntity trashEntity)
        {
            if (trashEntity.User != null)
                _context.Entry(trashEntity.User).State = EntityState.Unchanged;
        }
    }
}