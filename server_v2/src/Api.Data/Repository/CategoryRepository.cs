using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Repository;
using Data.Context;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    /// <summary>
    /// Gerenciador de repositório das categorias.
    /// </summary>
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(SomniaContext context) : base(context) { }

        public override async Task<IEnumerable<CategoryEntity>> SelectAsync(int userId)
        {
            var result = new List<CategoryEntity>();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;

                query = query.Include(act => act.User);

                query = query.Where(x => x.UserId == userId);
                
                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<CategoryEntity> SelectByIdAsync(int userId, int id)
        {
            var result = new CategoryEntity();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;
                
                query = query.Include(act => act.User);
                query = query.AsNoTracking()
                             .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<CategoryEntity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<CategoryEntity> query = _context.Category;
            
            query = query.Include(act => act.User);
            
            query = query.Where(x => x.UserId == userId);
            
            query = query.AsNoTracking().OrderBy(a => a.Id);

            if (pageParams.Tipo != null)
                query = query.Where(a => ((int)a.Type) == pageParams.Tipo);

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<CategoryEntity> SelectByUkAsync(int userId, CategoryType type, string nome)
        {
            var result = new CategoryEntity();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;
                
                query = query.Include(act => act.User);
                query = query.AsNoTracking()
                             .Where(x => x.Name == nome && x.Type == type && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }
        
        public void UnchangedParentCategory(CategoryEntity categoryEntity)
        {
            if (categoryEntity.User != null)
            {
                /*var existingEntry = _context.ChangeTracker.Entries<UserEntity>()
                    .FirstOrDefault(e => e.Entity.Id == categoryEntity.User.Id);

                if (existingEntry != null)
                    _context.Entry(existingEntry.Entity).State = EntityState.Detached;*/

                _context.Entry(categoryEntity.User).State = EntityState.Unchanged;
            }
        }
    }
}