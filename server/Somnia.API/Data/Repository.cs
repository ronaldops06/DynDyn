using Somnia.API.Helpers;
using Somnia.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Somnia.API.Models.Enums;
using System.Collections.Generic;

namespace Somnia.API.Data
{
    public class Repository : IRepository
    {
        private readonly SomniaContext _context;

        public Repository(SomniaContext context)
        {
            _context = context;
        }

        public SomniaContext GetContext()
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
        public User GetUsuarioByUsernamaAndPassword(string login, string password)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(x => x.Login.ToLower() == login.ToLower() && x.Password == password);

            return query.FirstOrDefault();
        }

        public User GetUsuarioByLogin(string login)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(x => x.Login.ToLower() == login.ToLower());

            return query.FirstOrDefault();
        }

        public User GetUsuarioById(int userID)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(x => x.ID == userID);

            return query.FirstOrDefault();
        }

        public async Task<PageList<User>> GetAllUsuariosAsync(PageParams pageParams)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().OrderBy(a => a.ID);

            return await PageList<User>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
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

            if (pageParams.Tipo != null)
            {
                query = query.Where(a => ((int)a.Tipo) == pageParams.Tipo);
            }

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

        public Operacao GetOperacaoByNameAndCategoryAndType(string name, int categoriaID, OperacaoTipo type)
        {
            IQueryable<Operacao> query = _context.Operacoes;

            query.Include(cat => cat.Categoria);

            query = query.AsNoTracking()
                         .OrderBy(a => a.ID)
                         .Where(operacao => operacao.Nome == name
                             && operacao.CategoriaID == categoriaID
                             && operacao.Tipo == type);

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

            if (pageParams.Tipo != null)
            {
                query = query.Where(ope => ((int)ope.Tipo) == pageParams.Tipo);
            }

            query = query.AsNoTracking().OrderBy(a => a.Nome);

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
            query = query.Include(cta => cta.ContaDestino);
            query = query.Include(cat => cat.ContaDestino.Categoria);

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
            query = query.Include(cta => cta.ContaDestino);
            query = query.Include(cat => cat.ContaDestino.Categoria);

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
            query = query.Include(cta => cta.ContaDestino);
            query = query.Include(cat => cat.ContaDestino.Categoria);

            query = query.AsNoTracking().OrderBy(a => a.DataCriacao);

            if (pageParams.DataCriacaoInicio != null)
            {
                query = query.Where(a => a.DataCriacao >= pageParams.DataCriacaoInicio);
            }

            if (pageParams.DataCriacaoFim != null)
            {
                query = query.Where(a => a.DataCriacao <= pageParams.DataCriacaoFim);
            }

            if (pageParams.MovimentoPaiID != null)
            {
                query = query.Where(a => a.MovimentoPaiID == pageParams.MovimentoPaiID || a.ID == pageParams.MovimentoPaiID);
            }

            return await PageList<Movimento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Dictionary<OperacaoTipo, double>> GetTotaisMovimentosAsync(PageParams pageParams)
        {
            IQueryable<Movimento> query = _context.Movimentos;

            query = query.Include(ope => ope.Operacao);

            if (pageParams.DataCriacaoInicio != null)
            {
                query = query.Where(a => a.DataCriacao >= pageParams.DataCriacaoInicio);
            }

            if (pageParams.DataCriacaoFim != null)
            {
                query = query.Where(a => a.DataCriacao <= pageParams.DataCriacaoFim);
            }

            var group = query.GroupBy(a => a.Operacao.Tipo)
            .Select(g => new { tipo = g.Key, sum = g.Sum(s => s.Valor) })
            .ToDictionary(k => k.tipo, i => i.sum);

            return group;
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

            if (movimento.ContaDestino != null)
            {
                _context.Entry(movimento.ContaDestino).State = EntityState.Unchanged;
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
