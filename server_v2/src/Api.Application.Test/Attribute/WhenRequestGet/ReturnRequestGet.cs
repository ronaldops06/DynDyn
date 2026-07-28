using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestGet
{
    public class ReturnRequestGet : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IAttributeService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(AttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(AttributeModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as AttributeResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(AttributeRequestDto.Name, resultValue.Name);
            Assert.Equal(AttributeRequestDto.Description, resultValue.Description);
            Assert.Equal(AttributeRequestDto.DataType, resultValue.DataType);
            Assert.Equal(AttributeRequestDto.Status, resultValue.Status);
        }
    }
}