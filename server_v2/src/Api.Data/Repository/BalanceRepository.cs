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
        
        public override async Task<IEnumerable<BalanceEntity>> SelectAsync(int userId)
        {
            var result = new List<BalanceEntity>();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId);
                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o saldo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<BalanceEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new BalanceEntity();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o saldo: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public override async Task<Data<BalanceEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<BalanceEntity> query = _context.Balance;

            query = query.Include(act => act.Account);
            query = query.Include(usr => usr.User);
            
            query = query.Where(x => x.UserId == userId);
            
            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<BalanceEntity> SelectByUkAsync(int userId, int accountId, int month, int year)
        {
            var result = new BalanceEntity();

            try
            {
                IQueryable<BalanceEntity> query = _context.Balance;

                query = query.Include(act => act.Account);
                query = query.Include(usr => usr.User);
    
                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.AccountId == accountId && x.Month == month && x.Year == year);

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
            
            if (balanceEntity.User != null)
                _context.Entry(balanceEntity.User).State = EntityState.Unchanged;
        }
    }
}