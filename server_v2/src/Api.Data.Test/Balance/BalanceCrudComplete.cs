using System.Globalization;
using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Balance;

public class BalanceCrudComplete : IClassFixture<DbTest>
{
    private UserEntity _user;
    private ServiceProvider _serviceProvider;

    public BalanceCrudComplete(DbTest dbTest)
    {
        _serviceProvider = dbTest.ServiceProvider;
    }
    
    private void AplicaTesteCampos(BalanceEntity balanceEntitySource, BalanceEntity balanceEntityDest)
    {
        Assert.NotNull(balanceEntityDest);
        Assert.Equal(balanceEntitySource.Value, balanceEntityDest.Value);
        Assert.Equal(balanceEntitySource.Valuation, balanceEntityDest.Valuation);
        Assert.Equal(balanceEntitySource.Dividends, balanceEntityDest.Dividends);
        Assert.Equal(balanceEntitySource.Income, balanceEntityDest.Income);
        Assert.Equal(balanceEntitySource.PercentageValuation, balanceEntityDest.PercentageValuation);
        Assert.Equal(balanceEntitySource.PercentageIncome, balanceEntityDest.PercentageIncome);
        Assert.Equal(balanceEntitySource.Credit, balanceEntityDest.Credit);
        Assert.Equal(balanceEntitySource.Debit, balanceEntityDest.Debit);
        Assert.Equal(balanceEntitySource.SalaryCredit, balanceEntityDest.SalaryCredit);
        Assert.Equal(balanceEntitySource.SalaryDebit, balanceEntityDest.SalaryDebit); 
        Assert.Equal(balanceEntitySource.Inflow, balanceEntityDest.Inflow); 
        Assert.Equal(balanceEntitySource.Outflow, balanceEntityDest.Outflow); 
        Assert.Equal(balanceEntitySource.Month, balanceEntityDest.Month);
        Assert.Equal(balanceEntitySource.Year, balanceEntityDest.Year);
        Assert.Equal(balanceEntitySource.AccountId, balanceEntityDest.AccountId);
        Assert.Equal(balanceEntitySource.Account.Id, balanceEntityDest.Account.Id);
        Assert.Equal(balanceEntitySource.UserId, balanceEntityDest.UserId);
        Assert.Equal(balanceEntitySource.User.Id, balanceEntityDest.User.Id);
    }

    [Fact(DisplayName = "CRUD de Saldo")]
    [Trait("CRUD", "BalanceEntity")]
    public async Task Eh_Possivel_Realizar_CRUD_Balance()
    {
        using (var context = _serviceProvider.GetService<SomniaContext>())
        {
            UserRepository userRepository = new UserRepository(context);
            _user = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
            Assert.NotNull(_user);
            Assert.True(_user.Id > 0);
            
            BalanceRepository _repositorio = new BalanceRepository(context);
            
            var accountEntity = await InsertAccont(context);

            BalanceEntity _balanceEntity = new BalanceEntity()
            {
                Value = 12000.00,
                Valuation = null,
                Dividends = 45.52,
                Income = -25.0,
                PercentageValuation = 0,
                PercentageIncome = -0.01,
                Credit = 9800.00,
                Debit = 1251.00,
                SalaryCredit = 5450.00,
                SalaryDebit = 1050.00,
                Inflow = 6800,
                Outflow = 7000,
                Month = 1,
                Year = 2025,
                Account = accountEntity,
                AccountId = accountEntity.Id,
                UserId = _user.Id,
                User = _user
            };
            
            var _registroCriado = await _repositorio.InsertAsync(_balanceEntity);
            AplicaTesteCampos(_balanceEntity, _registroCriado);
            Assert.True(_registroCriado.Id > 0);

            _balanceEntity.Value = 12350.51;
            
            var _registroAtualizado = await _repositorio.UpdateAsync(_balanceEntity);
            AplicaTesteCampos(_balanceEntity, _registroAtualizado);

            var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
            Assert.True(_registroExiste);

            var _registroSelecionado = await _repositorio.SelectByIdAsync(_user.Id, _registroAtualizado.Id);
            AplicaTesteCampos(_balanceEntity, _registroSelecionado);

            var _todosRegistros = await _repositorio.SelectAsync(_user.Id);
            Assert.NotNull(_todosRegistros);
            Assert.True(_todosRegistros.Count() > 0);

            var _removeu = await _repositorio.DeleteAsync(_registroCriado.Id);
            Assert.True(_removeu);
            
            _registroCriado.Id = 0;
            await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_registroCriado));
            await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_registroCriado.Id));
        }
    }
    
    private async Task<AccountEntity> InsertAccont(SomniaContext context)
    {
        var _categoryCreated = await InsertCategory(context, CategoryType.Conta, "Corrente");

        AccountRepository _repositorio = new AccountRepository(context);

        AccountEntity _parentAccountEntity = new AccountEntity()
        {
            Name = "Geral",
            Status = StatusType.Ativo,
            CategoryId = _categoryCreated.Id,
            Category = _categoryCreated,
            UserId = _user.Id,
            User = _user
        };

        var _parentAccountCreated = await _repositorio.InsertAsync(_parentAccountEntity);
        Assert.NotNull(_parentAccountCreated);
        Assert.True(_parentAccountCreated.Id > 0);
        Assert.Equal(_parentAccountCreated.Name, _parentAccountEntity.Name);

        AccountEntity _accountEntity = new AccountEntity()
        {
            Name = "Cash",
            Status = StatusType.Ativo,
            CategoryId = _categoryCreated.Id,
            Category = _categoryCreated,
            ParentAccountId = _parentAccountEntity.Id,
            ParentAccount = _parentAccountEntity,
            UserId = _user.Id,
            User = _user
        };

        var _registroCriado = await _repositorio.InsertAsync(_accountEntity);
        Assert.True(_registroCriado.Id > 0);
        Assert.NotNull(_registroCriado);
        Assert.Equal(_registroCriado.Name, _accountEntity.Name);

        return _registroCriado;
    }
    
    private async Task<CategoryEntity> InsertCategory(SomniaContext context, CategoryType type, string name)
    {
        CategoryRepository _repositorioCategory = new CategoryRepository(context);
        CategoryEntity _categoryEntity = new CategoryEntity()
        {
            Name = name,
            Type = type,
            Status = StatusType.Ativo,
            UserId = _user.Id,
            User = _user
        };

        var _registroCriado = await _repositorioCategory.InsertAsync(_categoryEntity);
        Assert.NotNull(_registroCriado);
        Assert.True(_registroCriado.Id > 0);
        Assert.Equal(_categoryEntity.Name, _registroCriado.Name);

        return _registroCriado;
    }
}