using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TotalizerRole.WhenRequestCreate
{
    public class ReturnCreated : BaseTestTotalizerRole
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<ITotalizerRoleService>();

            serviceMock.Setup(m => m.Post(It.IsAny<TotalizerRoleModel>())).ReturnsAsync(TotalizerRoleModel);

            Controller = new TotalizerRoleController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(TotalizerRoleRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as TotalizerRoleResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            //
        }
    }
}