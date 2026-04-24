using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TotalizerRole.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestTotalizerRole
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ITotalizerRoleService>();

            serviceMock.Setup(m => m.Put(It.IsAny<TotalizerRoleModel>())).ReturnsAsync(TotalizerRoleModel);

            Controller = new TotalizerRoleController(serviceMock.Object, Mapper);
            //Aplicar validação de campos obrigatórios

            var result = await Controller.Put(TotalizerRoleRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}