using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Repository;
using Data.Context;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório de atributos.
    /// </summary>
    public class AttributeRepository : BaseRepository<AttributeEntity>, IAttributeRepository, ICleanupRepository
    {
        public AttributeRepository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => 1;
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.Attribute.Where(x => x.UserId == userId).ToListAsync();
                _context.Attribute.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        public override async Task<AttributeEntity> UpdateAsync(AttributeEntity item)
        {
            try
            {
                var attribute = await _context.Attribute
                    .Include(t => t.Options)
                    .SingleOrDefaultAsync(t => t.Id == item.Id);
    
                if (attribute == null)
                    throw new Exception("No data found");
                
                item.DataCriacao = item.DataCriacao ?? attribute.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                // atualiza campos da operação
                _context.Entry(attribute).CurrentValues.SetValues(item);

                var newIds = item.Options?.Select(i => i.Id).ToHashSet();

                attribute.Options?.RemoveAll((i => !newIds.Contains(i.Id)));

                // Atualiza os que já existem
                if (attribute.Options != null && item.Options != null)
                {
                    foreach (var source in item.Options)
                    {
                        var target = attribute.Options.FirstOrDefault(i => i.Id == source.Id);

                        if (target != null)
                        {
                            target.Label = source.Label;
                            target.Status = source.Status;
                            target.IsDefault = source.IsDefault;
                            target.DataCriacao = source.DataCriacao;
                            target.DataAlteracao = DateTime.Now;
                        }
                    }
                }
                
                var existIds = attribute.Options?.Select(i => i.Id).ToHashSet();

                var add = item.Options?.FindAll(i => !existIds.Contains(i.Id));
                foreach (var option in add)
                {
                    option.DataCriacao = item.DataCriacao ?? DateTime.Now;
                    option.DataAlteracao = item.DataAlteracao ?? DateTime.Now;
                }
                
                attribute.Options?.AddRange(add);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }
                
        public override async Task<IEnumerable<AttributeEntity>> SelectAsync(int userId)
        {
            var result = new List<AttributeEntity>();

            try
            {
                IQueryable<AttributeEntity> query = _context.Attribute;

                query = QueryableIncludeRelations(query);
                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar atributo: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<AttributeEntity> SelectByIdAsync(int userId, int id)
        {
            try
            {
                IQueryable<AttributeEntity> query = _context.Attribute;
                
                query = QueryableIncludeRelations(query);
                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar atributo: Erro.: {ex.Message}");
            }
        }

        public override async Task<Data<AttributeEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<AttributeEntity> query = _context.Attribute;
            
            query = QueryableIncludeRelations(query);
            query = query.Where(x => x.UserId == userId);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<AttributeEntity> SelectByUkAsync(int userId, string name)
        {
            try
            {
                IQueryable<AttributeEntity> query = _context.Attribute;
                
                query = QueryableIncludeRelations(query);
                query = query.AsNoTracking()
                    .Where(x => x.UserId == userId && x.Name == name);

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar atributo: Erro.: {ex.Message}");
            }
        }

        public void UnchangedParentAttribute(AttributeEntity entity)
        {
            if (entity.User != null)
            {
                var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                    .FirstOrDefault(e => e.Entity.Id == entity.User.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                _context.Entry(entity.User).State = EntityState.Unchanged;
            }
            
            foreach (var option in entity.Options)
            {
                if (option.User != null)
                {
                    var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                        .FirstOrDefault(e => e.Entity.Id == option.User.Id);

                    if (existingEntry != null)
                        _context.Entry(existingEntry.Entity).State = EntityState.Detached;

                    _context.Entry(option.User).State = EntityState.Unchanged;
                }
            }
        }

        protected IQueryable<AttributeEntity> QueryableIncludeRelations(IQueryable<AttributeEntity> query)
        {
            query = query.Include(t => t.Options);
            query = query.Include(usr => usr.User);

            return query;
        }
    }
}
