using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Repository;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório das operações.
    /// </summary>
    public class OperationRepository : BaseRepository<OperationEntity>, IOperationRepository, ICleanupRepository
    {
        public OperationRepository(SomniaContext context) : base(context)
        {
        }

        public int CleanupOrder => 4;

        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Operation.Where(x => x.UserId == userId).ToListAsync();
                _context.Operation.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public override async Task<OperationEntity> UpdateAsync(OperationEntity item)
        {
            try
            {
                var operation = await _context.Operation
                    .Include(t => t.OperationRoles)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);
    
                if (operation == null)
                    throw new Exception("No data found");
                
                item.DataCriacao = item.DataCriacao ?? operation.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                // atualiza campos da operação
                _context.Entry(operation).CurrentValues.SetValues(item);

                var newIds = item.OperationRoles?.Select(i => i.OperationRoleId).ToHashSet();

                operation.OperationRoles?.RemoveAll((i => !newIds.Contains(i.OperationRoleId)));

                var existIds = operation.OperationRoles?.Select(i => i.OperationRoleId).ToHashSet();

                operation.OperationRoles?.AddRange(
                    item.OperationRoles?.FindAll(i => !existIds.Contains(i.OperationRoleId)));

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public override async Task<IEnumerable<OperationEntity>> SelectAsync(int userId)
        {
            var result = new List<OperationEntity>();

            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a operação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<OperationEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new OperationEntity();

            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a operação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<OperationEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<OperationEntity> query = _context.Operation;

            query = query.Include(cat => cat.Category);
            query = query.Include(t => t.OperationRoles)
                .ThenInclude(r => r.OperationRole);
            query = query.Include(usr => usr.User);

            query = query.Where(x => x.UserId == userId);

            if (pageParams.Tipo != null)
                query = query.Where(a => ((int)a.Type) == pageParams.Tipo);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<List<OperationEntity>> SelectByActiveAndRecurrent(int userId)
        {
            var result = new List<OperationEntity>();
            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId && x.Status == StatusType.Ativo && x.Recurrent);

                query = query.AsNoTracking().OrderBy(a => a.Id);

                result = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a operação: Erro.: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationEntity> SelectByUkAsync(int userId, string name, OperationType operationType)
        {
            var result = new OperationEntity();

            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
                query = query.Include(t => t.OperationRoles)
                    .ThenInclude(r => r.OperationRole);
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Name == name && x.Type == operationType && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a operação: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentOperation(OperationEntity entity)
        {
            if (entity.Category != null)
                _context.Entry(entity.Category).State = EntityState.Unchanged;

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