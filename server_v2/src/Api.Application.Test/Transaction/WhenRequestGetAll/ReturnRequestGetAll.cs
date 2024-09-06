using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ITransactionService>();

            var pageListTransactionModel = PageList<TransactionModel>.Create(PageParams, ListTransactionModel, ListTransactionModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListTransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<TransactionResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}