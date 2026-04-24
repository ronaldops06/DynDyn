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
    public class {{model}}Repository : BaseRepository<{{model}}Entity>, I{{model}}Repository, ICleanupRepository
    {
        public {{model}}Repository(SomniaContext context) : base(context)
        {
        }
        
        public int CleanupOrder => {{cleanupOrder}};
        
        public async Task<bool> DeleteAllByUserAsync(int userId)
        {
            try
            {
                var registros = await _context.{{model}}.Where(x => x.UserId == userId).ToListAsync();
                _context.{{model}}.RemoveRange(registros);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
                
        public override async Task<IEnumerable<{{model}}Entity>> SelectAsync(int userId)
        {
            var result = new List<{{model}}Entity>();

            try
            {
                IQueryable<{{model}}Entity> query = _context.{{model}};

                query = query.Include(usr => usr.User);
                //Adicionar referências
                
                query = query.Where(x => x.UserId == userId);

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar {{name}}: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<{{model}}Entity> SelectByIdAsync(int userId, int id)
        {
            var result = new {{model}}Entity();

            try
            {
                IQueryable<{{model}}Entity> query = _context.{{model}};
                
                query = query.Include(usr => usr.User);
                //Adicionar referências

                query = query.AsNoTracking()
                    .Where(x => x.Id == id && x.UserId == userId);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar a {{name}}: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<{{model}}Entity>> SelectByParamAsync(int userId, PageParams pageParams)
        {
            IQueryable<{{model}}Entity> query = _context.{{model}};
            
            query = query.Include(usr => usr.User);
            //Adicionar referências

            query = query.Where(x => x.UserId == userId);

            //Adicionar filtros específicos

            if (pageParams.LastSyncDate != null)
                query = query.Where(a => a.DataAlteracao >= pageParams.LastSyncDate);

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<{{model}}Entity> SelectByUkAsync(int userId)
        {
            var result = new {{model}}Entity();

            try
            {
                IQueryable<{{model}}Entity> query = _context.{{model}};
                
                query = query.Include(usr => usr.User);
                //Adicionar referências

                //Adicionar filtros das UKs

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar {{name}}: Erro.: {ex.Message}");
            }

            return result;
        }

        public void UnchangedParent{{model}}({{model}}Entity entity)
        {
            if (entity.User != null)
                _context.Entry(entity.User).State = EntityState.Unchanged;
        }
    }
}