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

        public override async Task<IEnumerable<TransactionEntity>> SelectAsync()
        {
            var result = new List<TransactionEntity>();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<TransactionEntity> SelectByIdAsync(int id)
        {
            var result = new TransactionEntity();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                             .Where(x => x.Id == id);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<TransactionEntity>> SelectByParamAsync(PageParams pageParams)
        {
            IQueryable<TransactionEntity> query = _context.Transaction;

            query = QueryableIncludeRelations(query);

            query = query.AsNoTracking().OrderBy(a => a.DataCriacao);

            if (pageParams.DataCriacaoInicio != null)
                query = query.Where(a => a.DataCriacao >= pageParams.DataCriacaoInicio);

            if (pageParams.DataCriacaoFim != null)
                query = query.Where(a => a.DataCriacao <= pageParams.DataCriacaoFim);

            if (pageParams.MovimentoPaiID != null)
                query = query.Where(a => a.ParentTransactionId == pageParams.MovimentoPaiID || a.Id == pageParams.MovimentoPaiID);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<IEnumerable<TransactionEntity>> SelectTransactionByParentTransactionIdAsync(int parentTransactionId)
        {
            IQueryable<TransactionEntity> query = _context.Transaction;

            query = QueryableIncludeRelations(query);

            query = query.AsNoTracking().OrderBy(a => a.DataCriacao);

            query = query.Where(a => a.ParentTransactionId == parentTransactionId || a.Id == parentTransactionId);

            return await query.ToListAsync();
        }

        public async Task<Dictionary<OperationType, double>> SelectTransactionsTotalsAsync(PageParams pageParams)
        {
            var totals = new Dictionary<OperationType, double>();
            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = query.Include(ope => ope.Operation);

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
            if (transactionEntity.ParentTransaction != null)
                _context.Entry(transactionEntity.ParentTransaction).State = EntityState.Unchanged;

            if (transactionEntity.Account != null)
                _context.Entry(transactionEntity.Account).State = EntityState.Unchanged;

            if (transactionEntity.DestinationAccount != null)
                _context.Entry(transactionEntity.DestinationAccount).State = EntityState.Unchanged;

            if (transactionEntity.Operation != null)
                _context.Entry(transactionEntity.Operation).State = EntityState.Unchanged;
        }

        protected IQueryable<TransactionEntity> QueryableIncludeRelations(IQueryable<TransactionEntity> query)
        {
            query = query.Include(ope => ope.Operation);
            query = query.Include(cat => cat.Operation.Category);
            query = query.Include(cta => cta.Account);
            query = query.Include(cat => cat.Account.Category);
            query = query.Include(cta => cta.DestinationAccount);
            query = query.Include(cat => cat.DestinationAccount.Category);

            return query;
        }
    }
}