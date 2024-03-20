using Data.Context;
using Domain.Entities;
using Domain.Repository;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Helpers;
using System;
using Domain.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Data.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(SomniaContext context) : base(context) { }

        public async Task<UserEntity> FindUsuarioByLogin(string login)
        {
            var result = new UserEntity();
            try
            {
                IQueryable<UserEntity> query = _context.Users;

                query = query.AsNoTracking()
                             .OrderBy(a => a.Id)
                             .Where(x => x.Login.ToLower() == login.ToLower());

                result = await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o usuário: Erro.: {ex.Message}");
            }

            return result;
        }

        public async Task<UserEntity> FindUsuarioByUsernamaAndPassword(string login, string password)
        {
            IQueryable<UserEntity> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(x => x.Login.ToLower() == login.ToLower() && x.Password == password);

            return await query.FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<UserEntity>> SelectAsync()
        {
            var result = new List<UserEntity>();

            try
            {
                IQueryable<UserEntity> query = _context.Users;

                query = query.AsNoTracking().OrderBy(a => a.Id);
                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o usuário: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<UserEntity> SelectByIdAsync(int id)
        {
            var result = new UserEntity();

            try
            {
                IQueryable<UserEntity> query = _context.Users;

                query = query.AsNoTracking()
                             .Where(x => x.Id == id);

                result = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar o usuário: Erro.: {ex.Message}");
            }

            return result;
        }

        public override async Task<Data<UserEntity>> SelectByParamAsync(PageParams pageParams)
        {
            IQueryable<UserEntity> query = _context.Users;

            query = query.AsNoTracking().OrderBy(a => a.Id);

            return await ExecuteQueryAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
