using Api.Domain.Dtos.Balance;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Enums;
using Api.Domain.Models;
using Application.V1.Controllers;
using Domain.Helpers;
using Domain.Models;
using Xunit;

namespace Api.Application.Test.Balance
{
    public class BaseTestBalance : BaseTestApplication
    {
        protected BalanceController Controller;
        protected BalanceModel BalanceModel;
        protected BalanceRequestDto BalanceRequestDto;
        protected PageParams PageParams;
        protected List<BalanceModel> ListBalanceModel = new List<BalanceModel>();

        protected BaseTestBalance()
        {
            Random random = new Random();

            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Diversos",
                Type = CategoryType.Operação,
                Status = StatusType.Ativo
            };

            var accountModel = new PortfolioModel
            {
                Id = 1,
                Name = "Cash",
                Status = StatusType.Ativo,
                Category = categoryModel,
                CategoryId = categoryModel.Id
            };

            BalanceModel = new BalanceModel()
            {
                Id = 1,
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
                Outflow = random.Next(2500),
                Month = 1,
                Year = 2025,
                Portfolio = accountModel,
                PortfolioId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            accountModel = new PortfolioModel
            {
                Id = 2,
                Name = "Cash 2",
                Status = StatusType.Ativo,
                Category = categoryModel,
                CategoryId = categoryModel.Id
            };

            BalanceModel = new BalanceModel
            {
                Id = 2,
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
                Outflow = random.Next(2500),
                Month = 1,
                Year = 2025,
                Portfolio = accountModel,
                PortfolioId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            accountModel = new PortfolioModel
            {
                Id = 3,
                Name = "Cash 3",
                Status = StatusType.Ativo,
                Category = categoryModel,
                CategoryId = categoryModel.Id
            };

            BalanceModel = new BalanceModel
            {
                Id = 3,
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
                Outflow = random.Next(2500),
                Month = 1,
                Year = 2025,
                Portfolio = accountModel,
                PortfolioId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            var accountRequestDto = new PortfolioRequestDto
            {
                Id = 3
            };

            BalanceRequestDto = new BalanceRequestDto
            {
                Id = 3,
                Value = BalanceModel.Value,
                Valuation = BalanceModel.Valuation,
                Dividends = BalanceModel.Dividends,
                Income = BalanceModel.Income,
                PercentageValuation = BalanceModel.PercentageValuation,
                PercentageIncome = BalanceModel.PercentageIncome,
                Credit = BalanceModel.Credit,
                Debit = BalanceModel.Debit,
                SalaryCredit = BalanceModel.SalaryCredit,
                SalaryDebit = BalanceModel.SalaryDebit,
                Inflow = BalanceModel.Inflow,
                Outflow = BalanceModel.Outflow,
                Month = 1,
                Year = 2025,
                Portfolio = accountRequestDto,
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }
        
        protected void ApplyTest(BalanceRequestDto balanceRequestDto, BalanceResponseDto balanceResponseDto)
        {
            Assert.NotNull(balanceResponseDto);
            Assert.Equal(balanceRequestDto.Id, balanceResponseDto.Id);
            Assert.Equal(balanceRequestDto.Value, balanceResponseDto.Value);
            Assert.Equal(balanceRequestDto.Valuation, balanceResponseDto.Valuation);
            Assert.Equal(balanceRequestDto.Dividends, balanceResponseDto.Dividends);
            Assert.Equal(balanceRequestDto.Income, balanceResponseDto.Income);
            Assert.Equal(balanceRequestDto.PercentageValuation, balanceResponseDto.PercentageValuation);
            Assert.Equal(balanceRequestDto.PercentageIncome, balanceResponseDto.PercentageIncome);
            Assert.Equal(balanceRequestDto.Credit, balanceResponseDto.Credit);
            Assert.Equal(balanceRequestDto.Debit, balanceResponseDto.Debit);
            Assert.Equal(balanceRequestDto.SalaryCredit, balanceResponseDto.SalaryCredit);
            Assert.Equal(balanceRequestDto.SalaryDebit, balanceResponseDto.SalaryDebit);
            Assert.Equal(balanceRequestDto.Inflow, balanceResponseDto.Inflow);
            Assert.Equal(balanceRequestDto.Outflow, balanceResponseDto.Outflow);
            Assert.Equal(balanceRequestDto.Month, balanceResponseDto.Month);
            Assert.Equal(balanceRequestDto.Year, balanceResponseDto.Year);
            Assert.Equal(balanceRequestDto.Portfolio.Id, balanceResponseDto.Portfolio.Id);
        }
    }
}