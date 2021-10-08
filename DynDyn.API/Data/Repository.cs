using DynDyn.API.Helpers;
using DynDyn.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DynDyn.API.Data
{
    public class Repository : IRepository
    {
        private readonly DyndynContext _context;

        public Repository(DyndynContext context)
        {
            _context = context;
        }

        public DyndynContext GetContext()
        {
            return _context;
        }

        #region default
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
        #endregion

        #region usuario
        public User GetUsuarioByUsernamaAndPassword(string username, string password)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(a => a.Id)
                         .Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password);

            return query.FirstOrDefault();
        }
        #endregion

        #region categoria
        public Categoria GetCategoriaById(int categoriaID)
        {
            IQueryable<Categoria> query = _context.Categorias;

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(categoria => categoria.ID == categoriaID);

            return query.FirstOrDefault();
        }

        public Categoria[] GetAllCategorias()
        {
            IQueryable<Categoria> query = _context.Categorias;

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return query.ToArray();
        }

        public async Task<PageList<Categoria>> GetAllCategoriasAsync(PageParams pageParams)
        {
            IQueryable<Categoria> query = _context.Categorias;

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<Categoria>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        #endregion

        #region conta
        public Conta GetContaById(int contaID)
        {
            IQueryable<Conta> query = _context.Contas;

            query = query.Include(cat => cat.Categoria);
            query = query.Include(cta => cta.ContaPai);

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(conta => conta.ID == contaID);

            return query.FirstOrDefault();
        }

        public Conta[] GetAllContas()
        {
            IQueryable<Conta> query = _context.Contas;

            query = query.Include(cat => cat.Categoria);
            query = query.Include(cta => cta.ContaPai);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return query.ToArray();
        }

        public async Task<PageList<Conta>> GetAllContasAsync(PageParams pageParams)
        {
            IQueryable<Conta> query = _context.Contas;

            query = query.Include(cat => cat.Categoria);
            query = query.Include(cta => cta.ContaPai);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<Conta>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public void UnchangedParentConta(Conta conta)
        {
            if (conta.Categoria != null)
            {
                _context.Entry(conta.Categoria).State = EntityState.Unchanged;
            }
            if (conta.ContaPai != null)
            {
                _context.Entry(conta.ContaPai).State = EntityState.Unchanged;
            }
        }
        #endregion

        #region operacao
        public Operacao GetOperacaoById(int operacaoID)
        {
            IQueryable<Operacao> query = _context.Operacoes;

            query.Include(cat => cat.Categoria);

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(operacao => operacao.ID == operacaoID);

            return query.FirstOrDefault();
        }

        public Operacao[] GetAllOperacoes()
        {
            IQueryable<Operacao> query = _context.Operacoes;

            query = query.Include(cat => cat.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return query.ToArray();
        }

        public async Task<PageList<Operacao>> GetAllOperacoesAsync(PageParams pageParams)
        {
            IQueryable<Operacao> query = _context.Operacoes;

            query = query.Include(cat => cat.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<Operacao>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public void UnchangedParentOperacao(Operacao operacao)
        {
            if (operacao.Categoria != null)
            {
                _context.Entry(operacao.Categoria).State = EntityState.Unchanged;
            }
        }
        #endregion

        #region movimento
        public Movimento GetMovimentoById(int movimentoID)
        {
            IQueryable<Movimento> query = _context.Movimentos;

            query = query.Include(ope => ope.Operacao);
            query = query.Include(cat => cat.Operacao.Categoria);
            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(movimento => movimento.ID == movimentoID);

            return query.FirstOrDefault();
        }

        public Movimento[] GetAllMovimentos()
        {
            IQueryable<Movimento> query = _context.Movimentos;

            query = query.Include(ope => ope.Operacao);
            query = query.Include(cat => cat.Operacao.Categoria);
            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return query.ToArray();
        }

        public async Task<PageList<Movimento>> GetAllMovimentosAsync(PageParams pageParams)
        {
            IQueryable<Movimento> query = _context.Movimentos;

            query = query.Include(ope => ope.Operacao);
            query = query.Include(cat => cat.Operacao.Categoria);
            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<Movimento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public void UnchangedParentMovimento(Movimento movimento)
        {
            if (movimento.Operacao != null)
            {
                _context.Entry(movimento.Operacao).State = EntityState.Unchanged;
            }
            if (movimento.Conta != null)
            {
                _context.Entry(movimento.Conta).State = EntityState.Unchanged;
            }
        }
        #endregion

        #region saldo
        public Saldo GetSaldoById(int saldoID)
        {
            IQueryable<Saldo> query = _context.Saldos;

            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(saldo => saldo.ID == saldoID);

            return query.FirstOrDefault();
        }

        public Saldo[] GetAllSaldos()
        {
            IQueryable<Saldo> query = _context.Saldos;

            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return query.ToArray();
        }

        public async Task<PageList<Saldo>> GetAllSaldosAsync(PageParams pageParams)
        {
            IQueryable<Saldo> query = _context.Saldos;

            query = query.Include(cta => cta.Conta);
            query = query.Include(cat => cat.Conta.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<Saldo>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public void UnchangedParentSaldo(Saldo saldo)
        {
            if (saldo.Conta != null)
            {
                _context.Entry(saldo.Conta).State = EntityState.Unchanged;
            }
        }
        #endregion
    }
}
