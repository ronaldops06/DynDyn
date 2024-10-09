using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Category;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Api.Application.Test.Helpers.CategoryHelpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Category.WhenRequestGetAll
{
    public class ReturnGet : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            int categoryId = 1;

            var categoryModel = new CategoryModel
            {
                Id = categoryId,
                Name = Faker.Name.FullName(),
                Type = GetCategoryTypeRandom(),
                Status = GetStatusTypeRandom(),
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            var serviceMock = new Mock<ICategoryService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(categoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await _controller.Get(categoryId);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as CategoryResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(resultValue.Id, categoryModel.Id);
            Assert.Equal(resultValue.Name, categoryModel.Name);
            Assert.Equal(resultValue.Type, (int)categoryModel.Type);
            Assert.Equal(resultValue.Status, (int)categoryModel.Status);
        }
    }
}
