using Api.Application.V1.Controllers;
using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TotalizerRole.WhenRequestGet
{
    public class ReturnRequestGet : BaseTestTotalizerRole
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ITotalizerRoleService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(TotalizerRoleModel);

            Controller = new TotalizerRoleController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(TotalizerRoleModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as TotalizerRoleResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
           //
        }
    }
}