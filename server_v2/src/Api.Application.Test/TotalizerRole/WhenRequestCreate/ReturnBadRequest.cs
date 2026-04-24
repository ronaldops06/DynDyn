using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Application.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TotalizerRole.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestTotalizerRole
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ITotalizerRoleService>();

            serviceMock.Setup(m => m.Post(It.IsAny<TotalizerRoleModel>())).ReturnsAsync(TotalizerRoleModel);

            Controller = new TotalizerRoleController(serviceMock.Object, Mapper);
            //Aplicar testes de campos obrigatórios

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(TotalizerRoleRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}