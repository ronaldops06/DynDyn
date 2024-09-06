using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ITransactionService>();

            serviceMock.Setup(m => m.Put(It.IsAny<TransactionModel>())).ReturnsAsync(TransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Value", "É um campo obrigatório");

            var result = await Controller.Put(TransactionRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}