using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Data.Context;
using Domain.Models;
using Domain.Helpers;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório do saldo.
    /// </summary>
    public class BalanceRepository: BaseRepository<BalanceEntity>, IBalanceRepository
    {
        public BalanceRepository(SomniaContext context) : base(context) { }
        
        public override async Task<IEnumerable<BalanceEntity>> SelectAsync()
        {
            var result = new List<BalanceEntity>();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o saldo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<BalanceEntity> SelectByIdAsync(int id)
        {
            var result = new BalanceEntity();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o saldo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<Data<BalanceEntity>> SelectByParamAsync(PageParams pageParams)
        {
            IQueryable<BalanceEntity> query = _context.Balance;

            query = query.Include(act => act.Account);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<BalanceEntity> SelectByUkAsync(int accountId, DateTime balanceDate)
        {
            var result = new BalanceEntity();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);

                query = query.AsNoTracking()
                    .Where(x => x.AccountId == accountId && x.BalanceDate >= balanceDate);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o saldo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public void UnchangedParentBalance(BalanceEntity balanceEntity)
        {
            if (balanceEntity.Account != null)
                _context.Entry(balanceEntity.Account).State = EntityState.Unchanged;
        }
    }
}