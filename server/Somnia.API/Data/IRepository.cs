using Somnia.API.Helpers;
using Somnia.API.Models;
using Somnia.API.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Somnia.API.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool SaveChanges();

        //Usuário
        User GetUsuarioByUsernamaAndPassword(string login, string password);
        User GetUsuarioByLogin(string login);
        User GetUsuarioById(int userID);
        Task<PageList<User>> GetAllUsuariosAsync(PageParams pageParams);

        // Categorias
        Categoria GetCategoriaById(int categoriaID);
        Categoria[] GetAllCategorias();
        Task<PageList<Categoria>> GetAllCategoriasAsync(PageParams pageParams);

        //Contas
        Conta GetContaById(int contaID);
        Conta[] GetAllContas();
        Task<PageList<Conta>> GetAllContasAsync(PageParams pageParams);
        void UnchangedParentConta(Conta conta);

        //Operacoes
        Operacao GetOperacaoById(int operacaoID);
        Operacao GetOperacaoByNameAndCategoryAndType(string name, int categoriaID, OperacaoTipo type);
        Operacao[] GetAllOperacoes();
        Task<PageList<Operacao>> GetAllOperacoesAsync(PageParams pageParams);
        void UnchangedParentOperacao(Operacao operacao);

        //Movimentos
        Movimento GetMovimentoById(int movimentoID);
        Movimento[] GetAllMovimentos();
        Task<PageList<Movimento>> GetAllMovimentosAsync(PageParams pageParams);
        Task<Dictionary<OperacaoTipo, double>> GetTotaisMovimentosAsync(PageParams pageParams);
        void UnchangedParentMovimento(Movimento movimento);

        //Saldos
        Saldo GetSaldoById(int saldoID);
        Saldo[] GetAllSaldos();
        Task<PageList<Saldo>> GetAllSaldosAsync(PageParams pageParams);
        void UnchangedParentSaldo(Saldo saldo);        
    }
}
