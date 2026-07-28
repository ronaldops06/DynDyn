using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IAttributeService>();

            var pageListAttributeModel = PageList<AttributeModel>.Create(PageParams, ListAttributeModel, ListAttributeModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListAttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<AttributeResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}