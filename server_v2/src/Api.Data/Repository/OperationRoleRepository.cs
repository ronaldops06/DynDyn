using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Repository;
using Data.Context;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class OperationRoleRepository : BaseRepository<OperationRoleEntity>, IOperationRoleRepository, ICleanupRepository
    {
        public OperationRoleRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 5;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.OperationRole.Where(x => x.UserId == userId).ToListAsync();
                _context.OperationRole.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
                
        public override async Task<IEnumerable<OperationRoleEntity>> SelectAsync(int userId)
        {
            var result = new List<OperationRoleEntity>();

            try
            {
                IQueryable<OperationRoleEntity> query = _context.OperationRole;

                query = query.Include(usr => usr.User);
                
                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar regras transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<OperationRoleEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new OperationRoleEntity();

            try
            {
                IQueryable<OperationRoleEntity> query = _context.OperationRole;

                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a regras transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<OperationRoleEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<OperationRoleEntity> query = _context.OperationRole;
            
            query = query.Include(usr => usr.User);
            
            query = query.Where(x => x.UserId == userId);
            
            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<OperationRoleEntity> SelectByUkAsync(int userId, string name)
        {
            var result = new OperationRoleEntity();

            try
            {
                IQueryable<OperationRoleEntity> query = _context.OperationRole;
                
                query = query.Include(usr => usr.User);
                
                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Name == name);

                result = await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar regras transação: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public void UnchangedParentOperationRole(OperationRoleEntity entity)
        {
            if (entity.User != null)
                _context.Entry(entity.User).State = EntityState.Unchanged;
        }
    }
}