using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Transaction.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestTransaction
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ITransactionService>();

            serviceMock.Setup(m => m.Post(It.IsAny<TransactionModel>())).ReturnsAsync(TransactionModel);

            Controller = new TransactionController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Valor", "É um campo obrigatório");
            Controller.ModelState.AddModelError("Consolidated", "É um campo obrigatório");
            Controller.ModelState.AddModelError("Account", "É um campo obrigatório");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(TransactionRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}