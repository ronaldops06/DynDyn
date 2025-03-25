using Api.Domain.Dtos.Balance;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Domain.Models;
using Xunit;

namespace Api.Application.Test.AutoMapper;

public class BalanceMapper : BaseTestApplication
{
    [Fact(DisplayName = "É possível mapear os modelos")]
            public void Eh_Possivel_Mapear_Os_Modelos()
            {
                var categoryRequestDto = new CategoryRequestDto
                {
                    Id = 1,
                    Name = "Corrente",
                    Type = (int)CategoryType.Conta,
                    Status = (int)StatusType.Ativo
                };
    
                var accountRequestDto = new PortfolioRequestDto
                {
                    Id = 1,
                    Name = "Geral",
                    Status = (int)StatusType.Ativo,
                    Category = categoryRequestDto
                };
                
                var balanceRequestDto = new BalanceRequestDto
                {
                    Id = 1,
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
                    Portfolio = accountRequestDto
                };
                
                //Dto -> Model
                var model = Mapper.Map<BalanceModel>(balanceRequestDto);
                Assert.Equal(model.Id, balanceRequestDto.Id);
                Assert.Equal(model.Value, balanceRequestDto.Value);
                Assert.Equal(model.Valuation, balanceRequestDto.Valuation);
                Assert.Equal(model.Dividends, balanceRequestDto.Dividends);
                Assert.Equal(model.Income, balanceRequestDto.Income);
                Assert.Equal(model.PercentageValuation, balanceRequestDto.PercentageValuation);
                Assert.Equal(model.PercentageIncome, balanceRequestDto.PercentageIncome);
                Assert.Equal(model.Credit, balanceRequestDto.Credit);
                Assert.Equal(model.Debit, balanceRequestDto.Debit);
                Assert.Equal(model.SalaryCredit, balanceRequestDto.SalaryCredit);
                Assert.Equal(model.SalaryDebit, balanceRequestDto.SalaryDebit); 
                Assert.Equal(model.Inflow, balanceRequestDto.Inflow); 
                Assert.Equal(model.Outflow, balanceRequestDto.Outflow); 
                Assert.Equal(model.Month, balanceRequestDto.Month);
                Assert.Equal(model.Year, balanceRequestDto.Year);
                Assert.Equal(model.PortfolioId, balanceRequestDto.Portfolio.Id);
                Assert.Equal(model.Portfolio.Id, balanceRequestDto.Portfolio.Id);
    
                //Model -> DtoResult
                var balanceResponseDto = Mapper.Map<BalanceResponseDto>(model);
                Assert.Equal(balanceResponseDto.Id, model.Id);
                Assert.Equal(balanceResponseDto.Value, model.Value);
                Assert.Equal(balanceResponseDto.Valuation, model.Valuation);
                Assert.Equal(balanceResponseDto.Dividends, model.Dividends);
                Assert.Equal(balanceResponseDto.Income, model.Income);
                Assert.Equal(balanceResponseDto.PercentageValuation, model.PercentageValuation);
                Assert.Equal(balanceResponseDto.PercentageIncome, model.PercentageIncome);
                Assert.Equal(balanceResponseDto.Credit, model.Credit);
                Assert.Equal(balanceResponseDto.Debit, model.Debit);
                Assert.Equal(balanceResponseDto.SalaryCredit, model.SalaryCredit);
                Assert.Equal(balanceResponseDto.SalaryDebit, model.SalaryDebit); 
                Assert.Equal(balanceResponseDto.Inflow, model.Inflow); 
                Assert.Equal(balanceResponseDto.Outflow, model.Outflow); 
                Assert.Equal(balanceResponseDto.Month, model.Month);
                Assert.Equal(balanceResponseDto.Year, model.Year);
                Assert.Equal(balanceResponseDto.Portfolio.Id, model.PortfolioId);
                Assert.Equal(balanceResponseDto.Portfolio.Id, model.Portfolio.Id);
            }
            
            [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo
            };
            
            Random random = new Random();
            var listModel = new List<BalanceModel>();

            for (int i = 1; i <= 5; i++)
            {
                var accountModel = new PortfolioModel
                {
                    Id = i,
                    Name = $"Geral {i}",
                    Status = StatusType.Ativo,
                    Category = categoryModel
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
                    Outflow = random.Next(5000),
                    Month = 1,
                    Year = 2025,
                    Portfolio = accountModel,
                    PortfolioId = accountModel.Id
                };
                
                listModel.Add(model);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<BalanceResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Value, listModel[i].Value);
                Assert.Equal(listDto[i].Valuation, listModel[i].Valuation);
                Assert.Equal(listDto[i].Dividends, listModel[i].Dividends);
                Assert.Equal(listDto[i].Income, listModel[i].Income);
                Assert.Equal(listDto[i].PercentageValuation, listModel[i].PercentageValuation);
                Assert.Equal(listDto[i].PercentageIncome, listModel[i].PercentageIncome);
                Assert.Equal(listDto[i].Credit, listModel[i].Credit);
                Assert.Equal(listDto[i].Debit, listModel[i].Debit);
                Assert.Equal(listDto[i].SalaryCredit, listModel[i].SalaryCredit);
                Assert.Equal(listDto[i].SalaryDebit, listModel[i].SalaryDebit); 
                Assert.Equal(listDto[i].Inflow, listModel[i].Inflow); 
                Assert.Equal(listDto[i].Outflow, listModel[i].Outflow); 
                Assert.Equal(listDto[i].Month, listModel[i].Month);
                Assert.Equal(listDto[i].Year, listModel[i].Year);
                Assert.Equal(listDto[i].Portfolio.Id, listModel[i].PortfolioId);
                Assert.Equal(listDto[i].Portfolio.Id, listModel[i].Portfolio.Id);
            }

            var pageList = new PageList<BalanceModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<BalanceResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Value, listModel[i].Value);
                Assert.Equal(listResponseDto[i].Valuation, listModel[i].Valuation);
                Assert.Equal(listResponseDto[i].Dividends, listModel[i].Dividends);
                Assert.Equal(listResponseDto[i].Income, listModel[i].Income);
                Assert.Equal(listResponseDto[i].PercentageValuation, listModel[i].PercentageValuation);
                Assert.Equal(listResponseDto[i].PercentageIncome, listModel[i].PercentageIncome);
                Assert.Equal(listResponseDto[i].Credit, listModel[i].Credit);
                Assert.Equal(listResponseDto[i].Debit, listModel[i].Debit);
                Assert.Equal(listResponseDto[i].SalaryCredit, listModel[i].SalaryCredit);
                Assert.Equal(listResponseDto[i].SalaryDebit, listModel[i].SalaryDebit); 
                Assert.Equal(listResponseDto[i].Inflow, listModel[i].Inflow); 
                Assert.Equal(listResponseDto[i].Outflow, listModel[i].Outflow); 
                Assert.Equal(listResponseDto[i].Month, listModel[i].Month);
                Assert.Equal(listResponseDto[i].Year, listModel[i].Year);
                Assert.Equal(listResponseDto[i].Portfolio.Id, listModel[i].PortfolioId);
                Assert.Equal(listResponseDto[i].Portfolio.Id, listModel[i].Portfolio.Id);
            }
        }
}