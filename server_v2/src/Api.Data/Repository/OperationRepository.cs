using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Repository;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório das operações.
    /// </summary>
    public class OperationRepository : BaseRepository<OperationEntity>, IOperationRepository
    {
        public OperationRepository(SomniaContext context) : base(context) { }

        public override async Task<IEnumerable<OperationEntity>> SelectAsync(int userId)
        {
            var result = new List<OperationEntity>();

            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
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
            query = query.Include(usr => usr.User);
            
            query = query.Where(x => x.UserId == userId);
            
            if (pageParams.Tipo != null)
                query = query.Where(a => ((int)a.Type) == pageParams.Tipo);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<OperationEntity> SelectByUkAsync(int userId, string name, OperationType operationType)
        {
            var result = new OperationEntity();

            try
            {
                IQueryable<OperationEntity> query = _context.Operation;

                query = query.Include(cat => cat.Category);
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

        public void UnchangedParentOperation(OperationEntity operationEntity)
        {
            if (operationEntity.Category != null)
                _context.Entry(operationEntity.Category).State = EntityState.Unchanged;
            
            if (operationEntity.User != null)
                _context.Entry(operationEntity.User).State = EntityState.Unchanged;
        }
    }
}