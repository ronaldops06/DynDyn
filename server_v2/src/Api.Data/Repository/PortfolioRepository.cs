using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
    /// Gerenciador de repositório das contas.
    /// </summary>
    public class PortfolioRepository : BaseRepository<PortfolioEntity>, IPortfolioRepository, ICleanupRepository
    {
        public PortfolioRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 2;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Portfolio.Where(x => x.UserId == userId).ToListAsync();
                _context.Portfolio.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        public override async Task<PortfolioEntity> UpdateAsync(PortfolioEntity item)
        {
            try
            {
                var portfolio = await _context.Portfolio
                    .Include(t => t.Attributes)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);
    
                if (portfolio == null)
                    throw new Exception("No data found");
                
                item.DataCriacao = item.DataCriacao ?? portfolio.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                // atualiza campos do portfolio
                _context.Entry(portfolio).CurrentValues.SetValues(item);

                var newIds = item.Attributes?.Select(i => i.AttributeId).ToHashSet();

                portfolio.Attributes?.RemoveAll((i => !newIds.Contains(i.AttributeId)));
                
                // Atualiza os que já existem
                if (portfolio.Attributes != null && item.Attributes != null)
                {
                    foreach (var source in item.Attributes)
                    {
                        var target = portfolio.Attributes.FirstOrDefault(i => i.Id == source.Id);

                        if (target != null)
                        {
                            target.ActionType = source.ActionType;
                            target.ValueBoolean = source.ValueBoolean;
                            target.ValueNumber = source.ValueNumber;
                            target.ValueText = source.ValueText;
                            target.ValueDate = source.ValueDate;
                            target.Attribute = source.Attribute;
                            target.AttributeOption = source.AttributeOption;
                            target.AttributeOptionId = source.AttributeOptionId;
                            target.Portfolio = source.Portfolio;
                            target.PortfolioId = source.PortfolioId;
                            target.AttributeId = source.AttributeId;
                            target.Status = source.Status;
                            target.DataCriacao = source.DataCriacao;
                            target.DataAlteracao = DateTime.Now;
                            target.User = source.User;
                            target.UserId = source.UserId;
                        }
                    }
                }
                
                var existIds = portfolio.Attributes?.Select(i => i.AttributeId).ToHashSet();

                var add = item.Attributes?.FindAll(i => !existIds.Contains(i.AttributeId));
                foreach (var attribute in add)
                {
                    attribute.DataCriacao = item.DataCriacao ?? DateTime.Now;
                    attribute.DataAlteracao = item.DataAlteracao ?? DateTime.Now;
                }

                portfolio.Attributes?.AddRange(add);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }
        
        public override async Task<IEnumerable<PortfolioEntity>> SelectAsync(int userId)
        {
            var result = new List<PortfolioEntity>();

            try
            {
                IQueryable<PortfolioEntity> query = _context.Portfolio;

                query = QueryableIncludeRelations(query);

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

        public override async Task<PortfolioEntity> SelectByIdAsync(int userId, int id)
        {
            try
            {
                IQueryable<PortfolioEntity> query = _context.Portfolio;

                query = QueryableIncludeRelations(query);

                query = query
                    .Where(x => x.UserId == userId && x.Id == id);

                var result = await query.FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a conta: Erro.: {ex.Message}");
            }
        }

        public override async Task<Data<PortfolioEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<PortfolioEntity> query = _context.Portfolio;

            query = QueryableIncludeRelations(query);

            query = query.Where(x => x.UserId == userId);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<PortfolioEntity> SelectByUkAsync(int userId, string name, StatusType status)
        {
            try
            {
                IQueryable<PortfolioEntity> query = _context.Portfolio;

                query = QueryableIncludeRelations(query);

                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Name == name && x.Status == status);

                var result = query.FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a conta: Erro.: {ex.Message}");
            }
        }

        public void UnchangedParentAccount(PortfolioEntity portfolioEntity)
        {
            if (portfolioEntity.Category != null)
            {
                var existingEntry = _context.ChangeTracker.Entries<CategoryEntity>()
                    .FirstOrDefault(e => e.Entity.Id == portfolioEntity.Category.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                _context.Entry(portfolioEntity.Category).State = EntityState.Unchanged;
            }
            
            if (portfolioEntity.ParentPortfolio != null)
            {
                var existingEntry = _context.ChangeTracker.Entries<PortfolioEntity>()
                    .FirstOrDefault(e => e.Entity.Id == portfolioEntity.ParentPortfolio.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                _context.Entry(portfolioEntity.ParentPortfolio).State = EntityState.Unchanged;
            }
            
            if (portfolioEntity.User != null)
            {
                var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                    .FirstOrDefault(e => e.Entity.Id == portfolioEntity.User.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                _context.Entry(portfolioEntity.User).State = EntityState.Unchanged;
            }
            
            foreach (var portfolioAttribute in portfolioEntity.Attributes)
            {
                if (portfolioAttribute.User != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                        .FirstOrDefault(e => e.Entity.Id == portfolioAttribute.User.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(portfolioAttribute.User).State = EntityState.Unchanged;
                }
                
                if (portfolioAttribute?.Attribute != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<AttributeEntity>()
                        .FirstOrDefault(e => e.Entity.Id == portfolioAttribute.Attribute.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(portfolioAttribute.Attribute).State = EntityState.Unchanged;
                    
                    //Resposta do atributo
                    if (portfolioAttribute.AttributeOption != null)
                    {
                        var existingEntryOption = _context.ChangeTracker.Entries<AttributeOptionEntity>()
                            .FirstOrDefault(e => e.Entity.Id == portfolioAttribute.AttributeOption.Id);

                        if (existingEntryOption != null)
                            _context.Entry(existingEntryOption.Entity).State = EntityState.Detached;

                        _context.Entry(portfolioAttribute.AttributeOption).State = EntityState.Unchanged;
                    }
                    
                    //Lista de opções (respostas possíveis) do atributo
                    foreach (var attributeOption in portfolioAttribute.Attribute.Options)
                    {
                        var existingEntryOption = _context.ChangeTracker.Entries<AttributeOptionEntity>()
                            .FirstOrDefault(e => e.Entity.Id == attributeOption.Id);

                        if (existingEntryOption != null)
                            _context.Entry(existingEntryOption.Entity).State = EntityState.Detached;

                        _context.Entry(attributeOption).State = EntityState.Unchanged;
                    }
                }
            }
        }
        
        protected IQueryable<PortfolioEntity> QueryableIncludeRelations(IQueryable<PortfolioEntity> query)
        {
            query = query.Include(p => p.Category);
            query = query.Include(cta => cta.ParentPortfolio);
            query = query.Include(cta => cta.ParentPortfolio.Category);
            query = query.Include(usr => usr.User).AsTracking();

            return query;
        }
    }
}