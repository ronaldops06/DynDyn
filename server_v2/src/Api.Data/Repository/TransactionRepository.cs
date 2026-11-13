using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Api.Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório das transações/movimentos.
    /// </summary>
    public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository, ICleanupRepository
    {
        public TransactionRepository(SomniaContext context) : base(context) { }

        public int CleanupOrder => 4;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Transaction.Where(x => x.UserId == userId).ToListAsync();
                _context.Transaction.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        /*public override async Task<TransactionEntity> UpdateAsync(TransactionEntity item)
        {
            try
            {
                var transaction = await _context.Transaction
                    .Include(t => t.Impacts)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);
    
                if (transaction == null)
                    throw new Exception("No data found");
                
                item.DataCriacao = item.DataCriacao ?? transaction.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                // atualiza campos do portfolio
                _context.Entry(transaction).CurrentValues.SetValues(item);

                var newIds = item.Impacts?.Select(i => i.Id).ToHashSet();

                transaction.Impacts?.RemoveAll((i => !newIds.Contains(i.Id)));
                
                // Atualiza os que já existem
                if (transaction.Impacts != null && item.Impacts != null)
                {
                    foreach (var source in item.Impacts)
                    {
                        var target = transaction.Impacts.FirstOrDefault(i => i.Id == source.Id);

                        if (target != null)
                        {
                            target.TransactionId = source.TransactionId;
                            target.Transaction = source.Transaction;
                            target.OperationType = source.OperationType;
                            target.ImpactType = source.ImpactType;
                            target.PortfolioAttribute = source.PortfolioAttribute;
                            target.PortfolioAttributeId = source.PortfolioAttributeId;
                            target.Portfolio = source.Portfolio;
                            target.PortfolioId = source.PortfolioId;
                            target.Value = source.Value;
                            target.DataCriacao = source.DataCriacao;
                            target.DataAlteracao = DateTime.Now;
                            target.User = source.User;
                            target.UserId = source.UserId;
                        }
                    }
                }
                
                var existIds = transaction.Impacts?.Select(i => i.Id).ToHashSet();

                var add = item.Impacts?.FindAll(i => !existIds.Contains(i.Id));
                foreach (var impact in add)
                {
                    impact.DataCriacao = item.DataCriacao ?? DateTime.Now;
                    impact.DataAlteracao = item.DataAlteracao ?? DateTime.Now;
                }

                transaction.Impacts?.AddRange(add);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }*/
        
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

        public async Task<IEnumerable<TransactionEntity>> SelectByOperationAndPeriodAsync(int userId, int operationId, Period period)
        {
            var result = new List<TransactionEntity>();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.OperationId == operationId &&
                                x.DataCriacao >= period.DateStartMonth && x.DataCriacao <= period.DateEndMonth);

                result = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public async Task<TransactionEntity> SelectByOperationAndDateAndValueAsync(int userId, int operationId, DateTime creationDate, double value)
        {
            var result = new TransactionEntity();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.OperationId == operationId &&
                                x.DataCriacao == creationDate && x.Value == value);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<TransactionEntity> UpdateAsync(TransactionEntity item)
        {
            try
            {
                var transaction = await _context.Transaction
                    .Include(t => t.Impacts)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);

                if (transaction == null)
                    throw new Exception("No data found");

                item.DataCriacao = item.DataCriacao ?? transaction.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                _context.Entry(transaction).CurrentValues.SetValues(item);

                var impacts = item.Impacts ?? new List<TransactionImpactEntity>();
                var newIds = impacts.Select(i => i.Id).ToHashSet();

                transaction.Impacts?.RemoveAll(i => !newIds.Contains(i.Id));

                var existIds = transaction.Impacts?.Select(i => i.Id).ToHashSet() ?? new HashSet<int>();
                var newImpacts = impacts.FindAll(i => !existIds.Contains(i.Id));

                foreach (var impact in impacts.Where(i => existIds.Contains(i.Id)))
                {
                    var currentImpact = transaction.Impacts.First(i => i.Id == impact.Id);
                    impact.TransactionId = item.Id;
                    _context.Entry(currentImpact).CurrentValues.SetValues(impact);

                    if (impact.Portfolio != null)
                        _context.Entry(impact.Portfolio).State = EntityState.Unchanged;

                    if (impact.PortfolioAttribute != null)
                        _context.Entry(impact.PortfolioAttribute).State = EntityState.Unchanged;
                }

                foreach (var impact in newImpacts)
                {
                    impact.TransactionId = item.Id;
                    impact.Transaction = null;

                    if (impact.Portfolio != null)
                        _context.Entry(impact.Portfolio).State = EntityState.Unchanged;

                    if (impact.PortfolioAttribute != null)
                        _context.Entry(impact.PortfolioAttribute).State = EntityState.Unchanged;
                }

                transaction.Impacts?.AddRange(newImpacts);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public async Task<List<TransactionEntity>> SelectByPendingInstallments(int userId, Period period)
        {
            var result = new List<TransactionEntity>();

            try
            {
                IQueryable<TransactionEntity> query = _context.Transaction;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId &&
                                x.DataCriacao >= period.DateStartMonth && x.DataCriacao <= period.DateEndMonth && x.Installment < x.TotalInstallments);

                result = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a transação: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentTransaction(TransactionEntity transactionEntity)
        {
            try
            {
                if (transactionEntity.ParentTransaction != null)
                    _context.Entry(transactionEntity.ParentTransaction).State = EntityState.Unchanged;

                if (transactionEntity.Portfolio != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<PortfolioEntity>()
                        .FirstOrDefault(e => e.Entity.Id == transactionEntity.Portfolio.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(transactionEntity.Portfolio).State = EntityState.Unchanged;
                }

                if (transactionEntity.DestinationPortfolio != null)
                    _context.Entry(transactionEntity.DestinationPortfolio).State = EntityState.Unchanged;

                if (transactionEntity.Operation != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<OperationEntity>()
                           .FirstOrDefault(e => e.Entity.Id == transactionEntity.Operation.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(transactionEntity.Operation).State = EntityState.Unchanged;
                }
                
                if (transactionEntity.User != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                        .FirstOrDefault(e => e.Entity.Id == transactionEntity.User.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(transactionEntity.User).State = EntityState.Unchanged;
                }

                /*if (transactionEntity.Impacts != null)
                {
                    foreach (var impact in transactionEntity.Impacts)
                    {
                        if (impact.Portfolio != null)
                        {
                            var existingEntry = _context.ChangeTracker.Entries<PortfolioEntity>()
                                .FirstOrDefault(e => e.Entity.Id == impact.Portfolio.Id);

                            if (existingEntry != null)
                                _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                            _context.Entry(impact.Portfolio).State = EntityState.Unchanged;
                        }

                        if (impact.PortfolioAttribute != null)
                            _context.Entry(impact.PortfolioAttribute).State = EntityState.Unchanged;
                        
                        if (impact.User != null)
                            _context.Entry(impact.User).State = EntityState.Unchanged;
                    }
                }*/
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
            query = query.Include(opeRol => opeRol.Operation.OperationRoles)
                .ThenInclude(r => r.OperationRole);
            query = query.Include(cta => cta.Portfolio);
            query = query.Include(p => p.Portfolio.Category);
            query = query.Include(cta => cta.DestinationPortfolio);
            query = query.Include(cat => cat.DestinationPortfolio.Category);
            query = query.Include(imp => imp.Impacts)
                .ThenInclude(i => i.Portfolio);
            query = query.Include(imp => imp.Impacts)
                .ThenInclude(i => i.PortfolioAttribute);
            query = query.Include(usr => usr.User);

            return query;
        }
    }
}
