using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Repository;
using Data.Context;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class TotalizerRoleRepository : BaseRepository<TotalizerRoleEntity>, ITotalizerRoleRepository, ICleanupRepository
    {
        public TotalizerRoleRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 6;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.TotalizerRole.Where(x => x.UserId == userId).ToListAsync();
                _context.TotalizerRole.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
               
        public override async Task<TotalizerRoleEntity> UpdateAsync(TotalizerRoleEntity item)
        {
            try
            {
                var totalizerRole = await _context.TotalizerRole
                    .Include(t => t.OperationRoles)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);
    
                if (totalizerRole == null)
                    throw new Exception("Registro não encontrado");
                
                item.DataCriacao = item.DataCriacao ?? totalizerRole.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                // atualiza campos da operação
                _context.Entry(totalizerRole).CurrentValues.SetValues(item);

                var newIds = item.OperationRoles?.Select(i => i.OperationRoleId).ToHashSet();

                totalizerRole.OperationRoles?.RemoveAll((i => !newIds.Contains(i.OperationRoleId)));

                var existIds = totalizerRole.OperationRoles?.Select(i => i.OperationRoleId).ToHashSet();

                totalizerRole.OperationRoles?.AddRange(
                    item.OperationRoles?.FindAll(i => !existIds.Contains(i.OperationRoleId)));

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }
        
        public override async Task<IEnumerable<TotalizerRoleEntity>> SelectAsync(int userId)
        {
            var result = new List<TotalizerRoleEntity>();

            try
            {
                IQueryable<TotalizerRoleEntity> query = _context.TotalizerRole;

                query = query.Include(usr => usr.User);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);
                
                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar papel de totalizador: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<TotalizerRoleEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new TotalizerRoleEntity();

            try
            {
                IQueryable<TotalizerRoleEntity> query = _context.TotalizerRole;
                
                query = query.Include(usr => usr.User);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a papel de totalizador: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<TotalizerRoleEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<TotalizerRoleEntity> query = _context.TotalizerRole;
            
            query = query.Include(usr => usr.User);
            query = query.Include(t => t.OperationRoles)
                .ThenInclude(r => r.OperationRole);

            query = query.Where(x => x.UserId == userId);

            //Adicionar filtros específicos

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<TotalizerRoleEntity> SelectByUkAsync(int userId, string code, TotalizerType type)
        {
            var result = new TotalizerRoleEntity();

            try
            {
                IQueryable<TotalizerRoleEntity> query = _context.TotalizerRole;
                
                query = query.Include(usr => usr.User);
                query = query.Include(t => t.OperationRoles)
                                .ThenInclude(r => r.OperationRole);

                query = query.AsNoTracking()
                    .Where(x => x.Code == code && x.Type == type && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar papel de totalizador: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentTotalizerRole(TotalizerRoleEntity entity)
        {
            /*if (entity.User != null)
                _context.Entry(entity.User).State = EntityState.Unchanged;*/
            
            if (entity.User != null)
            {
                var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                    .FirstOrDefault(e => e.Entity.Id == entity.User.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                _context.Entry(entity.User).State = EntityState.Unchanged;
            }

            foreach (var operationRole in entity.OperationRoles)
            {
                if (operationRole?.OperationRole != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<OperationRoleEntity>()
                        .FirstOrDefault(e => e.Entity.Id == operationRole.OperationRole.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(operationRole.OperationRole).State = EntityState.Unchanged;
                }
            }
        }
    }
}