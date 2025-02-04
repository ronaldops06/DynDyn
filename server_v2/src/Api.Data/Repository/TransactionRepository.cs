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
    /// Gerenciador de repositório das transações/movimentos.
    /// </summary>
    public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(SomniaContext context) : base(context) { }

        public override async Task<IEnumerable<TransactionEntity>> SelectAsync(int userId)
        {
            var result = new List<TransactionEntity>();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);
                
                query = query.Where(x => x.UserId == userId);
                
                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<TransactionEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new TransactionEntity();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                             .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<TransactionEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<TransactionEntity> query = _context.Transaction;

            query = QueryableIncludeRelations(query);

            query = query.AsNoTracking().OrderBy(a => a.DataCriacao);
            
            query = query.Where(x => x.UserId == userId);

            if (pageParams.DataCriacaoInicio != null)
                query = query.Where(a => a.DataCriacao >= pageParams.DataCriacaoInicio);

            if (pageParams.DataCriacaoFim != null)
                query = query.Where(a => a.DataCriacao <= pageParams.DataCriacaoFim);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            if (pageParams.MovimentoPaiID != null)
                query = query.Where(a => a.ParentTransactionId == pageParams.MovimentoPaiID || a.Id == pageParams.MovimentoPaiID);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<IEnumerable<TransactionEntity>> SelectTransactionByParentTransactionIdAsync(int userId, int parentTransactionId)
        {
            IQueryable<TransactionEntity> query = _context.Transaction;

            query = QueryableIncludeRelations(query);

            query = query.AsNoTracking().OrderBy(a => a.DataCriacao);
            
            query = query.Where(x => x.UserId == userId);
            query = query.Where(a => a.ParentTransactionId == parentTransactionId || a.Id == parentTransactionId);

            return await query.ToListAsync();
        }

        public async Task<Dictionary<OperationType, double>> SelectTransactionsTotalsAsync(int userId, PageParams pageParams)
        {
            var totals = new Dictionary<OperationType, double>();
            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = query.Include(ope => ope.Operation);

                query = query.Where(x => x.UserId == userId);

                if (pageParams.DataCriacaoInicio != null)
                    query = query.Where(a => a.DataCriacao >= pageParams.DataCriacaoInicio);

                if (pageParams.DataCriacaoFim != null)
                    query = query.Where(a => a.DataCriacao <= pageParams.DataCriacaoFim);

                totals = query.GroupBy(a => a.Operation.Type)
                .Select(g => new { tipo = g.Key, sum = g.Sum(s => s.Value) })
                .ToDictionary(k => k.tipo, i => Math.Round(i.sum, 2));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar os valores totais das transações: Erro.: {ex.Message}");
            }

            return totals;
        }

        public void UnchangedParentTransaction(TransactionEntity transactionEntity)
        {
            try
            {
                if (transactionEntity.ParentTransaction != null)
                    _context.Entry(transactionEntity.ParentTransaction).State = EntityState.Unchanged;

                if (transactionEntity.Account != null)
                    _context.Entry(transactionEntity.Account).State = EntityState.Unchanged;

                if (transactionEntity.DestinationAccount != null)
                    _context.Entry(transactionEntity.DestinationAccount).State = EntityState.Unchanged;

                if (transactionEntity.Operation != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<OperationEntity>()
                           .FirstOrDefault(e => e.Entity.Id == transactionEntity.Operation.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(transactionEntity.Operation).State = EntityState.Unchanged;
                }
                
                if (transactionEntity.User != null)
                    _context.Entry(transactionEntity.User).State = EntityState.Unchanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IQueryable<TransactionEntity> QueryableIncludeRelations(IQueryable<TransactionEntity> query)
        {
            query = query.Include(ope => ope.Operation);
            query = query.Include(cat => cat.Operation.Category);
            query = query.Include(cta => cta.Account);
            query = query.Include(cat => cat.Account.Category);
            query = query.Include(cta => cta.DestinationAccount);
            query = query.Include(cat => cat.DestinationAccount.Category);
            query = query.Include(usr => usr.User);

            return query;
        }
    }
}