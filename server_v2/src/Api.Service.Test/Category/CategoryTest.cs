using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using static Api.Service.Test.Helpers.CategoryHelpers;
using static Api.Service.Test.Helpers.BaseHelper;
using Moq;
using Api.Domain.Repository;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.Category
{
    public class CategoryTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;

        protected Mock<ICategoryRepository> RepositoryMock = new Mock<ICategoryRepository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<CategoryModel> listCategoryModel = new List<CategoryModel>();
        protected List<CategoryModel> listCategoryModelResult = new List<CategoryModel>();
        protected CategoryModel categoryModel;
        protected CategoryModel categoryModelResult;
        protected CategoryModel categoryModelUpdate;
        protected CategoryModel categoryModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

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
                    Name = Faker.Name.FullName(),
                    Type = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listCategoryModel.Add(dto);
            }

            listCategoryModelResult = listCategoryModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                       .Take(pageParams.PageSize)
                                                       .ToList();

            categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            categoryModelResult = new CategoryModel
            {
                Id = categoryModel.Id,
                Name = categoryModel.Name,
                Type = categoryModel.Type,
                Status = categoryModel.Status,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            categoryModelUpdate = new CategoryModel
            {
                Id = categoryModel.Id,
                Name = "Lazer",
                Type = CategoryType.Conta,
                Status = StatusType.Inativo,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            categoryModelUpdateResult = new CategoryModel
            {
                Id = categoryModelUpdate.Id,
                Name = categoryModelUpdate.Name,
                Type = categoryModelUpdate.Type,
                Status = categoryModelUpdate.Status,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "category",
                ReferenceId = categoryModel.Id
            };
        }

        protected void ApplyTest(CategoryModel categoryModelSource, CategoryModel categoryModelDest)
        {
            Assert.NotNull(categoryModelDest);
            Assert.NotNull(categoryModelDest.User);
            Assert.True(categoryModelDest.UserId > 0);
            Assert.Equal(categoryModelSource.Id, categoryModelDest.Id);
            Assert.Equal(categoryModelSource.Name, categoryModelDest.Name);
            Assert.Equal(categoryModelSource.Type, categoryModelDest.Type);
            Assert.Equal(categoryModelSource.Status, categoryModelDest.Status);
            Assert.Equal(categoryModelSource.UserId, categoryModelDest.UserId);
            Assert.Equal(categoryModelSource.User.Id, categoryModelDest.User.Id);
        }
    }
}