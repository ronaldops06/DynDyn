using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Api.Application.Test.Helpers.CategoryHelpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Category.WhenRequestGetAll
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<ICategoryService>();

            var pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };

            var listCategoryModel = new List<CategoryModel>
            {
                new CategoryModel{
                    Id = 1,
                    Nome = Faker.Name.FullName(),
                    Tipo = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                },
                 new CategoryModel{
                    Id = 2,
                    Nome = Faker.Name.FullName(),
                    Tipo = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                 }
            };

            var pageListCategoryModel = PageList<CategoryModel>.Create(pageParams, listCategoryModel, listCategoryModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListCategoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await _controller.Get(pageParams);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
