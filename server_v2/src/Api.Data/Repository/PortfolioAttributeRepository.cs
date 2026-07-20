using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Repository;
using Data.Context;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório de Portfolio Attribute.
    /// </summary>
    public class PortfolioAttributeRepository : BaseRepository<PortfolioAttributeEntity>, IPortfolioAttributeRepository, ICleanupRepository
    {
        public PortfolioAttributeRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 4;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.PortfolioAttribute.Where(x => x.Portfolio.UserId == userId).ToListAsync();
                _context.PortfolioAttribute.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
                
        public override async Task<IEnumerable<PortfolioAttributeEntity>> SelectAsync(int userId)
        {
            var result = new List<PortfolioAttributeEntity>();

            try
            {
                IQueryable<PortfolioAttributeEntity> query = _context.PortfolioAttribute;

                query = query.Include(pa => pa.Attribute)
                    .Include(pa => pa.Portfolio)
                    .Include(pa => pa.AttributeOption);
                
                query = query.Where(x => x.Portfolio.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar Portfolio Attribute: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<PortfolioAttributeEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new PortfolioAttributeEntity();

            try
            {
                IQueryable<PortfolioAttributeEntity> query = _context.PortfolioAttribute;
                
                query = query.Include(pa => pa.Attribute)
                    .Include(pa => pa.Portfolio)
                    .Include(pa => pa.AttributeOption);

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.Portfolio.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar Portfolio Attribute: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<PortfolioAttributeEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<PortfolioAttributeEntity> query = _context.PortfolioAttribute;
            
            query = query.Include(pa => pa.Attribute)
                .Include(pa => pa.Portfolio)
                .Include(pa => pa.AttributeOption);

            query = query.Where(x => x.Portfolio.UserId == userId);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<PortfolioAttributeEntity> SelectByPortfolioAndAttributeAsync(int portfolioId, int attributeId)
        {
            var result = new PortfolioAttributeEntity();

            try
            {
                IQueryable<PortfolioAttributeEntity> query = _context.PortfolioAttribute;
                
                query = query.Include(pa => pa.Attribute)
                    .Include(pa => pa.Portfolio)
                    .Include(pa => pa.AttributeOption);

                query = query.AsNoTracking()
                    .Where(x => x.PortfolioId == portfolioId && x.AttributeId == attributeId);

                result = await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar Portfolio Attribute: Erro.: {ex.Message}");
            }

            return result;
        }

        public async Task<IEnumerable<PortfolioAttributeEntity>> SelectByPortfolioAsync(int portfolioId)
        {
            var result = new List<PortfolioAttributeEntity>();

            try
            {
                IQueryable<PortfolioAttributeEntity> query = _context.PortfolioAttribute;

                query = query.Include(pa => pa.Attribute)
                    .Include(pa => pa.Portfolio)
                    .Include(pa => pa.AttributeOption);
                
                query = query.AsNoTracking()
                    .Where(x => x.PortfolioId == portfolioId)
                    .OrderBy(a => a.Id);

                result = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar Portfolio Attributes: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParentPortfolioAttribute(PortfolioAttributeEntity entity)
        {
            if (entity.Attribute != null)
                _context.Entry(entity.Attribute).State = EntityState.Unchanged;
            
            if (entity.Portfolio != null)
                _context.Entry(entity.Portfolio).State = EntityState.Unchanged;
            
            if (entity.AttributeOption != null)
                _context.Entry(entity.AttributeOption).State = EntityState.Unchanged;
        }
    }
}