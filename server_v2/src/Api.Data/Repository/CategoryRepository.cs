using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Repository;
using Data.Context;
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

        public override async Task<IEnumerable<CategoryEntity>> SelectAsync()
        {
            var result = new List<CategoryEntity>();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<CategoryEntity> SelectByIdAsync(int id)
        {
            var result = new CategoryEntity();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;

                query = query.AsNoTracking()
                             .Where(x => x.Id == id);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<CategoryEntity>> SelectByParamAsync(PageParams pageParams)
        {
            IQueryable<CategoryEntity> query = _context.Category;

            query = query.AsNoTracking().OrderBy(a => a.Id);

            if (pageParams.Tipo != null)
                query = query.Where(a => ((int)a.Tipo) == pageParams.Tipo);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<CategoryEntity> SelectByUkAsync(string nome)
        {
            var result = new CategoryEntity();

            try
            {
                IQueryable<CategoryEntity> query = _context.Category;

                query = query.AsNoTracking()
                             .Where(x => x.Nome == nome);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a categoria: Erro.: {ex.Message}");
            }

            return result;
        }
    }
}