using System.Globalization;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Balance;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Balance;

public class BaseTestBalance : BaseIntegration
{
    protected class CategoryBase
    {
        public int CategoryId { get; set; }
        public string CategoryNome { get; set; }
        public int CategoryTipo { get; set; }
        public int CategoryStatus { get; set; }
    }

    protected class AccountBase
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountStatus { get; set; }
        public CategoryBase AccountCategory { get; set; }
    }

    protected class BalanceBase
    {
        public int BalanceId { get; set; }
        public Double BalanceValue { get; set; }
        public Double? BalanceValuation { get; set; }
        public Double? BalanceDividends { get; set; }
        public Double? BalanceIncome { get; set; }
        public Double? BalancePercentageValuation { get; set; }
        public Double? BalancePercentageIncome { get; set; }
        public Double BalanceCredit { get; set; }
        public Double BalanceDebit { get; set; }
        public Double? BalanceSalaryCredit { get; set; }
        public Double? BalanceSalaryDebit { get; set; }
        public DateTime BalanceBalanceDate { get; set; }
        public int BalanceAccountId { get; set; }
        public AccountBase BalanceAccount { get; set; }
    }
    
    protected BalanceRequestDto BalanceRequestDto;
    protected CategoryRequestDto CategoryRequestDto;
    protected AccountRequestDto AccountRequestDto;
    protected BalanceRequestDto ParentBalanceRequestDto;
    protected BalanceBase BalanceBaseDto;
    protected PageParams PageParams;
    
    protected BaseTestBalance()
    {
        PageParams = new PageParams()
        {
            PageNumber = 1,
            PageSize = 3
        };

        var categoryBase = new CategoryBase
        {
            CategoryNome = "Corrente",
            CategoryTipo = (int)CategoryType.Conta,
            CategoryStatus = (int)StatusType.Ativo
        };
        
        var accountBase = new AccountBase
        {
            AccountName = "Cash",
            AccountStatus = (int)StatusType.Ativo,
            AccountCategory = categoryBase
        };
        
        Random random = new Random();
        
        BalanceBaseDto = new BalanceBase
        {
            BalanceValue = random.Next(50000),
            BalanceValuation = random.Next(5000),
            BalanceDividends = random.Next(300),
            BalanceIncome = random.Next(500),
            BalancePercentageValuation = random.Next(100),
            BalancePercentageIncome = random.Next(100),
            BalanceCredit = random.Next(10000),
            BalanceDebit = random.Next(10000),
            BalanceSalaryCredit = random.Next(8000),
            BalanceSalaryDebit = random.Next(1500),
            BalanceBalanceDate = DateTime.ParseExact("2025-01-31 23:59:59", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            BalanceAccount = accountBase
        };
        
        BalanceRequestDto = new BalanceRequestDto
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
            BalanceDate = DateTime.ParseExact("2025-01-31 23:59:59", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            Account = null
        };
    }
    
    protected void GenerateRequestDto()
    {
        CategoryRequestDto = new CategoryRequestDto()
        {
            Id = BalanceBaseDto.BalanceAccount.AccountCategory.CategoryId,
            Name = BalanceBaseDto.BalanceAccount.AccountCategory.CategoryNome,
            Status = BalanceBaseDto.BalanceAccount.AccountCategory.CategoryStatus,
            Type = BalanceBaseDto.BalanceAccount.AccountCategory.CategoryTipo
        };
        
        AccountRequestDto = new AccountRequestDto
        {
            Id = BalanceBaseDto.BalanceAccount.AccountId,
            Name = BalanceBaseDto.BalanceAccount.AccountName,
            Status = BalanceBaseDto.BalanceAccount.AccountStatus,
            Category = CategoryRequestDto
        };

        BalanceRequestDto = new BalanceRequestDto
        {
            Value = BalanceBaseDto.BalanceValue,
            Valuation = BalanceBaseDto.BalanceValuation,
            Dividends = BalanceBaseDto.BalanceDividends,
            Income = BalanceBaseDto.BalanceIncome,
            PercentageValuation = BalanceBaseDto.BalancePercentageValuation,
            PercentageIncome = BalanceBaseDto.BalancePercentageIncome,
            Credit = BalanceBaseDto.BalanceCredit,
            Debit = BalanceBaseDto.BalanceDebit,
            SalaryCredit =  BalanceBaseDto.BalanceSalaryCredit,
            SalaryDebit = BalanceBaseDto.BalanceSalaryDebit,
            BalanceDate = BalanceBaseDto.BalanceBalanceDate,
            Account = AccountRequestDto,
        };
    }
}