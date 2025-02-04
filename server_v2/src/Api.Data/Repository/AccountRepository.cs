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
    /// Gerenciador de repositório das contas.
    /// </summary>
    public class AccountRepository : BaseRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(SomniaContext context) : base(context) { }

        public override async Task<IEnumerable<AccountEntity>> SelectAsync(int userId)
        {
            var result = new List<AccountEntity>();

            try
            {
                IQueryable<AccountEntity> query = _context.Account;

                query = query.Include(cat => cat.Category);
                query = query.Include(cta => cta.ParentAccount);
                query = query.Include(cta => cta.ParentAccount.Category);
                query = query.Include(usr => usr.User);

                query = query.Where(x => x.UserId == userId);
                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a conta: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<AccountEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new AccountEntity();

            try
            {
                IQueryable<AccountEntity> query = _context.Account;

                query = query.Include(cat => cat.Category);
                query = query.Include(cta => cta.ParentAccount);
                query = query.Include(cta => cta.ParentAccount.Category);
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                             .Where(x => x.UserId == userId && x.Id == id);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a conta: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<AccountEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<AccountEntity> query = _context.Account;

            query = query.Include(cat => cat.Category);
            query = query.Include(cta => cta.ParentAccount);
            query = query.Include(cta => cta.ParentAccount.Category);
            query = query.Include(usr => usr.User);

            query = query.Where(x => x.UserId == userId);
            
            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<AccountEntity> SelectByUkAsync(int userId, string name, StatusType status)
        {
            var result = new AccountEntity();

            try
            {
                IQueryable<AccountEntity> query = _context.Account;

                query = query.Include(cat => cat.Category);
                query = query.Include(cta => cta.ParentAccount);
                query = query.Include(cta => cta.ParentAccount.Category);
                query = query.Include(usr => usr.User);

                query = query.AsNoTracking()
                             .Where(x => x.UserId == userId && x.Name == name && x.Status == status);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a conta: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentAccount(AccountEntity accountEntity)
        {
            if (accountEntity.Category != null)
                _context.Entry(accountEntity.Category).State = EntityState.Unchanged;

            if (accountEntity.ParentAccount != null)
                _context.Entry(accountEntity.ParentAccount).State = EntityState.Unchanged;
            
            if (accountEntity.User != null)
                _context.Entry(accountEntity.User).State = EntityState.Unchanged;
        }
    }
}