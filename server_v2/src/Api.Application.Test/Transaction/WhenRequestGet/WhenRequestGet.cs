using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestGet
{
    public class WhenRequestGet : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ITransactionService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(TransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(TransactionModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as TransactionResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(TransactionRequestDto.Value, resultValue.Value);
            Assert.Equal(TransactionRequestDto.Observation, resultValue.Observation);
            Assert.Equal(TransactionRequestDto.Installment, resultValue.Installment);
            Assert.Equal(TransactionRequestDto.Consolidated, resultValue.Consolidated);
            Assert.Equal(TransactionRequestDto.TotalInstallments, resultValue.TotalInstallments);
            Assert.Equal(TransactionRequestDto.Account.Id, resultValue.Account.Id);
            Assert.Equal(TransactionRequestDto.Operation.Id, resultValue.Operation.Id);
            Assert.Equal(TransactionRequestDto.ParentTransaction?.Id, resultValue.ParentTransaction?.Id);
        }
    }
}