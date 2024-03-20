using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Category;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Api.Application.Test.Helpers.CategoryHelpers;

namespace Api.Application.Test.Category.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ICategoryService>();
            var nome = Faker.Name.FullName();
            var tipo = GetCategoryTypeRandom();
            var status = GetStatusTypeRandom();

            var categoryModel = new CategoryModel
            {
                Id = 1,
                Nome = nome,
                Tipo = tipo,
                Status = status,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Post(It.IsAny<CategoryModel>())).ReturnsAsync(categoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Nome", "É um campo obrigatório");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            _controller.Url = url.Object;

            var categoryRequestDto = new CategoryRequestDto
            {
                Nome = nome,
                Tipo = (int)tipo,
                Status = (int)status
            };

            var result = await _controller.Post(categoryRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
