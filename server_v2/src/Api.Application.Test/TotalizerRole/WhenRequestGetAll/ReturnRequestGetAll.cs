using Api.Application.V1.Controllers;
using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TotalizerRole.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestTotalizerRole
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ITotalizerRoleService>();

            var pageListTotalizerRoleModel = PageList<TotalizerRoleModel>.Create(PageParams, ListTotalizerRoleModel, ListTotalizerRoleModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListTotalizerRoleModel);

            Controller = new TotalizerRoleController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<TotalizerRoleResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}