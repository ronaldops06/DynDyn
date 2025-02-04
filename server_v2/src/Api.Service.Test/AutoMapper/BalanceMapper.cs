using System.Globalization;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper;

public class BalanceMapper : BaseTestService
{
    [Fact(DisplayName = "É possível mapear os modelos")]
    public void Eh_Possivel_Mapear_Os_Modelos()
    {
        var accountModel = GenerateAccount(2, "Cach");
        
        BalanceModel model = new BalanceModel()
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
            Inflow = 8700.70,
            Outflow = 3500,
            Month = 1,
            Year = 2025,
            Account = accountModel,
            AccountId = accountModel.Id,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        //Model -> Entity
        var entity = Mapper.Map<BalanceEntity>(model);
        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Value, model.Value);
        Assert.Equal(entity.Valuation, model.Valuation);
        Assert.Equal(entity.Dividends, model.Dividends);
        Assert.Equal(entity.Income, model.Income);
        Assert.Equal(entity.PercentageValuation, model.PercentageValuation);
        Assert.Equal(entity.PercentageIncome, model.PercentageIncome);
        Assert.Equal(entity.Credit, model.Credit);
        Assert.Equal(entity.Debit, model.Debit);
        Assert.Equal(entity.SalaryCredit, model.SalaryCredit);
        Assert.Equal(entity.SalaryDebit, model.SalaryDebit); 
        Assert.Equal(entity.Inflow, model.Inflow);
        Assert.Equal(entity.Outflow, model.Outflow); 
        Assert.Equal(entity.Month, model.Month);
        Assert.Equal(entity.Year, model.Year);
        Assert.Equal(entity.AccountId, model.AccountId);
        Assert.Equal(entity.Account.Id, model.Account.Id);
        Assert.Equal(entity.UserId, model.UserId);
        Assert.Equal(entity.User.Id, model.User.Id);

        //Entity -> Model
        var balanceModel = Mapper.Map<BalanceModel>(entity);
        Assert.Equal(balanceModel.Value, entity.Value);
        Assert.Equal(balanceModel.Valuation, entity.Valuation);
        Assert.Equal(balanceModel.Dividends, entity.Dividends);
        Assert.Equal(balanceModel.Income, entity.Income);
        Assert.Equal(balanceModel.PercentageValuation, entity.PercentageValuation);
        Assert.Equal(balanceModel.PercentageIncome, entity.PercentageIncome);
        Assert.Equal(balanceModel.Credit, entity.Credit);
        Assert.Equal(balanceModel.Debit, entity.Debit);
        Assert.Equal(balanceModel.SalaryCredit, entity.SalaryCredit);
        Assert.Equal(balanceModel.SalaryDebit, entity.SalaryDebit); 
        Assert.Equal(balanceModel.Inflow, entity.Inflow);
        Assert.Equal(balanceModel.Outflow, entity.Outflow); 
        Assert.Equal(balanceModel.Month, entity.Month);
        Assert.Equal(balanceModel.Year, entity.Year);
        Assert.Equal(balanceModel.AccountId, entity.AccountId);
        Assert.Equal(balanceModel.Account.Id, entity.Account.Id);
        Assert.Equal(balanceModel.DataCriacao, entity.DataCriacao);
        Assert.Equal(balanceModel.DataAlteracao, entity.DataAlteracao);
        Assert.Equal(balanceModel.UserId, entity.UserId);
        Assert.Equal(balanceModel.User.Id, entity.User.Id);
    }
    
    [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            
            var accountModel = GenerateAccount(2, "Cach");
            var accountEntity = Mapper.Map<AccountEntity>(accountModel);

            var listEntity = new List<BalanceEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new BalanceEntity()
                {
                    Id = 2,
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
                    Inflow = 8700.70,
                    Outflow = 3500,
                    Month = 1,
                    Year = 2025,
                    Account = accountEntity,
                    AccountId = accountEntity.Id,
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<BalanceModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Value, listEntity[i].Value);
                Assert.Equal(listModel[i].Valuation, listEntity[i].Valuation);
                Assert.Equal(listModel[i].Dividends, listEntity[i].Dividends);
                Assert.Equal(listModel[i].Income, listEntity[i].Income);
                Assert.Equal(listModel[i].PercentageValuation, listEntity[i].PercentageValuation);
                Assert.Equal(listModel[i].PercentageIncome, listEntity[i].PercentageIncome);
                Assert.Equal(listModel[i].Credit, listEntity[i].Credit);
                Assert.Equal(listModel[i].Debit, listEntity[i].Debit);
                Assert.Equal(listModel[i].SalaryCredit, listEntity[i].SalaryCredit);
                Assert.Equal(listModel[i].SalaryDebit, listEntity[i].SalaryDebit); 
                Assert.Equal(listModel[i].Inflow, listEntity[i].Inflow);
                Assert.Equal(listModel[i].Outflow, listEntity[i].Outflow); 
                Assert.Equal(listModel[i].Month, listEntity[i].Month);
                Assert.Equal(listModel[i].Year, listEntity[i].Year);
                Assert.Equal(listModel[i].AccountId, listEntity[i].AccountId);
                Assert.Equal(listModel[i].Account.Id, listEntity[i].Account.Id);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }
    
    private AccountModel GenerateAccount(int id, string name)
    {
        var category = GenerateCategory(CategoryType.Conta, "Corrente", 1);

        AccountModel _parentAccountModel = new AccountModel()
        {
            Id = 1,
            Name = "Geral",
            Status = StatusType.Ativo,
            CategoryId = category.Id,
            Category = category,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        return new AccountModel()
        {
            Id = id,
            Name = name,
            Status = StatusType.Ativo,
            CategoryId = category.Id,
            Category = category,
            ParentAccountId = _parentAccountModel.Id,
            ParentAccount = _parentAccountModel,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };
    }
    
    private CategoryModel GenerateCategory(CategoryType type, string name, int id)
    {
        return new CategoryModel()
        {
            Id = id,
            Name = name,
            Type = type,
            Status = StatusType.Ativo,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };
    }
    
}