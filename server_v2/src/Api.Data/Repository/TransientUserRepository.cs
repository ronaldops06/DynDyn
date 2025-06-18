using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class TransientUserRepository : BaseRepository<TransientUserEntity>, ITransientUserRepository
    {
        public TransientUserRepository(SomniaContext context) : base(context) { }

        public async Task<TransientUserEntity> SelectUsuarioByLogin(string login)
        {
            var result = new TransientUserEntity();
            try
            {
                IQueryable<TransientUserEntity> query = _context.TransientUsers;

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

        public async Task<TransientUserEntity> SelectByIdAsync(int id)
        {
            var result = new TransientUserEntity();

            try
            {
                IQueryable<TransientUserEntity> query = _context.TransientUsers;

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
    }
}