using System.Globalization;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Balance;
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

            var accountModel = new AccountModel
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
                BalanceDate = DateTime.ParseExact("2025-01-31 23:59:59", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Account = accountModel,
                AccountId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            accountModel = new AccountModel
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
                BalanceDate = DateTime.ParseExact("2025-01-31 23:59:59", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Account = accountModel,
                AccountId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            accountModel = new AccountModel
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
                BalanceDate = DateTime.ParseExact("2025-01-31 23:59:59", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Account = accountModel,
                AccountId = accountModel.Id,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            ListBalanceModel.Add(BalanceModel);

            var accountRequestDto = new AccountRequestDto
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
                BalanceDate = BalanceModel.BalanceDate,
                Account = accountRequestDto,
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
            Assert.Equal(balanceRequestDto.BalanceDate, balanceResponseDto.BalanceDate);
            Assert.Equal(balanceRequestDto.Account.Id, balanceResponseDto.Account.Id);
        }
    }
}