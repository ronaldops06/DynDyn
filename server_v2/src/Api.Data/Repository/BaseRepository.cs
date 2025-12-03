using Data.Context;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly SomniaContext _context;
        private DbSet<T> _dataset;

        public BaseRepository(SomniaContext context)
        {
            _context = context;
            _dataset = context.Set<T>();
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                item.DataCriacao = item.DataCriacao ?? DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                item.DataAlteracao = item.DataCriacao ?? DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                _dataset.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                
                if (ex.InnerException != null)
                    message = $"Inner Exception: {ex.InnerException.Message}";
                
                //Log.Info<BaseRepository<T>>(message);
                throw new Exception(message);
            }

            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(x => x.Id.Equals(item.Id));
                if (result == null)
                    throw new Exception("No data found");
                
                item.DataCriacao = item.DataCriacao ?? result.DataCriacao;
                item.DataAlteracao = DateTime.Now;

                _context.Entry(result).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(x => x.Id.Equals(id));
                if (result == null)
                    throw new Exception("No data found");

                _dataset.Remove(result);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dataset.AnyAsync(x => x.Id.Equals(id));
        }
        
        public virtual Task<T> SelectByIdAsync(int userId, int id) => null;

        public virtual Task<IEnumerable<T>> SelectAsync(int userId) => null;

        public virtual Task<Data<T>> SelectByParamAsync(int userId, PageParams pageParams) => null;

        protected async Task<Data<T>> ExecuteQueryAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var itens = await source.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new Data<T>(count, itens);
        }
    }
}
