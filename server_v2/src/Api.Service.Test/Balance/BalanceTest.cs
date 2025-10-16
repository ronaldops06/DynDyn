using System.Globalization;
using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Api.Service.Test.Balance;

public class BalanceTest : BaseTestService
{
    private static readonly int RECORD_NUMBER = 10;

    protected Mock<IBalanceRepository> RepositoryMock = new Mock<IBalanceRepository>();
    protected Mock<IDeviceService> DeviceServiceMock = new Mock<IDeviceService>();
    protected List<BalanceModel> listBalanceModel = new List<BalanceModel>();
    protected List<BalanceModel> listBalanceModelResult = new List<BalanceModel>();
    protected BalanceModel balanceModel;
    protected BalanceModel balanceModelResult;
    protected BalanceModel balanceModelUpdate;
    protected BalanceModel balanceModelUpdateResult;
    protected PageParams pageParams;
    protected NotificationModel notificationModel;

    protected BalanceTest()
    {
        var categoryModel = new CategoryModel
        {
            Id = 1,
            Name = "Corrente",
            Type = CategoryType.Conta,
            Status = StatusType.Ativo,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        var portfolioModelBase = new PortfolioModel
        {
            Id = 1,
            Name = "Cash",
            Status = StatusType.Ativo,
            Category = categoryModel,
            CategoryId = categoryModel.Id,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        pageParams = new PageParams()
        {
            PageNumber = 1,
            PageSize = 5,
        };

        Random random = new Random();

        for (int i = 1; i <= RECORD_NUMBER; i++)
        {
            var portfolioModel = new PortfolioModel
            {
                Id = i,
                Name = $"Cash {i}",
                Status = StatusType.Ativo,
                Category = categoryModel,
                CategoryId = categoryModel.Id,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            var model = new BalanceModel()
            {
                Value = random.Next(50000),
                Valuation = random.Next(5000),
                Dividends = random.Next(300),
                Income = random.Next(500),
                PercentageValuation = random.Next(100),
                PercentageIncome = random.Next(100),
                Credit = random.Next(10000),
                Debit = random.Next(10000),
                SalaryCredit = random.Next(8000),
                SalaryDebit = random.Next(1500),
                Inflow = random.Next(10000),
                Outflow = random.Next(8000),
                Month = 1,
                Year = 2025,
                Portfolio = portfolioModel,
                PortfolioId = portfolioModel.Id,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            listBalanceModel.Add(model);
        }

        listBalanceModelResult = listBalanceModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
            .Take(pageParams.PageSize)
            .ToList();

        balanceModel = new BalanceModel
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
            Inflow = 6800,
            Outflow = 7000,
            Month = 1,
            Year = 2025,
            Portfolio = portfolioModelBase,
            PortfolioId = portfolioModelBase.Id,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        balanceModelResult = new BalanceModel
        {
            Id = balanceModel.Id,
            Value = balanceModel.Value,
            Valuation = balanceModel.Valuation,
            Dividends = balanceModel.Dividends,
            Income = balanceModel.Income,
            PercentageValuation = balanceModel.PercentageValuation,
            PercentageIncome = balanceModel.PercentageIncome,
            Credit = balanceModel.Credit,
            Debit = balanceModel.Debit,
            SalaryCredit = balanceModel.SalaryCredit,
            SalaryDebit = balanceModel.SalaryDebit,
            Inflow = balanceModel.Inflow,
            Outflow = balanceModel.Outflow,
            Month = balanceModel.Month,
            Year = balanceModel.Year,
            Portfolio = balanceModel.Portfolio,
            PortfolioId = balanceModel.PortfolioId,
            DataCriacao = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        balanceModelUpdate = new BalanceModel
        {
            Id = balanceModel.Id,
            Value = 11250.00,
            Valuation = 520,
            Dividends = 88.00,
            Income = 150,
            PercentageValuation = 23,
            PercentageIncome = 12,
            Credit = 7500,
            Debit = 980,
            SalaryCredit = 3800,
            SalaryDebit = 680,
            Inflow = 6800,
            Outflow = 7000,
            Month = 1,
            Year = 2025,
            Portfolio = balanceModel.Portfolio,
            PortfolioId = balanceModel.PortfolioId,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        balanceModelUpdateResult = new BalanceModel
        {
            Id = balanceModelUpdate.Id,
            Value = balanceModelUpdate.Value,
            Valuation = balanceModelUpdate.Valuation,
            Dividends = balanceModelUpdate.Dividends,
            Income = balanceModelUpdate.Income,
            PercentageValuation = balanceModelUpdate.PercentageValuation,
            PercentageIncome = balanceModelUpdate.PercentageIncome,
            Credit = balanceModelUpdate.Credit,
            Debit = balanceModelUpdate.Debit,
            SalaryCredit = balanceModelUpdate.SalaryCredit,
            SalaryDebit = balanceModelUpdate.SalaryDebit,
            Month = balanceModelUpdate.Month,
            Year = balanceModelUpdate.Year,
            Inflow = balanceModelUpdate.Inflow,
            Outflow = balanceModelUpdate.Outflow,
            Portfolio = balanceModelUpdate.Portfolio,
            PortfolioId = balanceModelUpdate.PortfolioId,
            DataCriacao = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
            User = UserModelFake,
            UserId = UserModelFake.Id
        };
        
        notificationModel = new NotificationModel
        {
            Title = "Exclude Entity",
            Body = new
            {
                Operation = "DELETE",
                Reference = "balance",
                Id = balanceModel.Id
            }
        };
    }

    protected void ApplyTest(BalanceModel balanceModelSource, BalanceModel balanceModelDest)
    {
        Assert.NotNull(balanceModelDest);
        Assert.Equal(balanceModelSource.Id, balanceModelDest.Id);
        Assert.Equal(balanceModelSource.Value, balanceModelDest.Value);
        Assert.Equal(balanceModelSource.Valuation, balanceModelDest.Valuation);
        Assert.Equal(balanceModelSource.Dividends, balanceModelDest.Dividends);
        Assert.Equal(balanceModelSource.Income, balanceModelDest.Income);
        Assert.Equal(balanceModelSource.PercentageValuation, balanceModelDest.PercentageValuation);
        Assert.Equal(balanceModelSource.PercentageIncome, balanceModelDest.PercentageIncome);
        Assert.Equal(balanceModelSource.Credit, balanceModelDest.Credit);
        Assert.Equal(balanceModelSource.Debit, balanceModelDest.Debit);
        Assert.Equal(balanceModelSource.SalaryCredit, balanceModelDest.SalaryCredit);
        Assert.Equal(balanceModelSource.SalaryDebit, balanceModelDest.SalaryDebit);
        Assert.Equal(balanceModelSource.Inflow, balanceModelDest.Inflow);
        Assert.Equal(balanceModelSource.Outflow, balanceModelDest.Outflow);
        Assert.Equal(balanceModelSource.Month, balanceModelDest.Month);
        Assert.Equal(balanceModelSource.Year, balanceModelDest.Year);
        Assert.Equal(balanceModelSource.PortfolioId, balanceModelDest.PortfolioId);
        Assert.Equal(balanceModelSource.Portfolio.Id, balanceModelDest.Portfolio.Id);
        Assert.Equal(balanceModelSource.UserId, balanceModelDest.UserId);
        Assert.Equal(balanceModelSource.User.Id, balanceModelDest.User.Id);
    }
}