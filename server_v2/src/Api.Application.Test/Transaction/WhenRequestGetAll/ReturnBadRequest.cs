using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestGetAll
{
    public class ReturnBadRequest : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ITransactionService>();

            var pageListTransactionModel = PageList<TransactionModel>.Create(PageParams, ListTransactionModel, ListTransactionModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListTransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(PageParams);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}