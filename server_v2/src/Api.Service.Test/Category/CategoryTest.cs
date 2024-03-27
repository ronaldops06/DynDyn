using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Service.Test.Helpers.CategoryHelpers;
using static Api.Service.Test.Helpers.BaseHelper;
using Moq;
using Api.Domain.Repository;
using Xunit;

namespace Api.Service.Test.Category
{
    public class CategoryTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<ICategoryRepository> RepositoryMock = new Mock<ICategoryRepository>();
        protected List<CategoryModel> listCategoryModel = new List<CategoryModel>();
        protected List<CategoryModel> listCategoryModelResult = new List<CategoryModel>();
        protected CategoryModel categoryModel;
        protected CategoryModel categoryModelResult;
        protected CategoryModel categoryModelUpdate;
        protected CategoryModel categoryModelUpdateResult;
        protected PageParams pageParams;

        protected CategoryTest()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var dto = new CategoryModel()
                {
                    Id = i,
                    Nome = Faker.Name.FullName(),
                    Tipo = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listCategoryModel.Add(dto);
            }

            listCategoryModelResult = listCategoryModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                       .Take(pageParams.PageSize)
                                                       .ToList();

            categoryModel = new CategoryModel
            {
                Id = 1,
                Nome = "Corrente",
                Tipo = CategoryType.Conta,
                Status = StatusType.Ativo
            };

            categoryModelResult = new CategoryModel
            {
                Id = categoryModel.Id,
                Nome = categoryModel.Nome,
                Tipo = categoryModel.Tipo,
                Status = categoryModel.Status,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            categoryModelUpdate = new CategoryModel
            {
                Id = categoryModel.Id,
                Nome = "Lazer",
                Tipo = CategoryType.Conta,
                Status = StatusType.Inativo
            };

            categoryModelUpdateResult = new CategoryModel
            {
                Id = categoryModelUpdate.Id,
                Nome = categoryModelUpdate.Nome,
                Tipo = categoryModelUpdate.Tipo,
                Status = categoryModelUpdate.Status,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };
        }

        protected void ApplyTest(CategoryModel categoryModelSource, CategoryModel categoryModelDest)
        {
            Assert.NotNull(categoryModelDest);
            Assert.Equal(categoryModelSource.Id, categoryModelDest.Id);
            Assert.Equal(categoryModelSource.Nome, categoryModelDest.Nome);
            Assert.Equal(categoryModelSource.Tipo, categoryModelDest.Tipo);
            Assert.Equal(categoryModelSource.Status, categoryModelDest.Status);
        }
    }
}