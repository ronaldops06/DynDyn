using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;
using static Api.Service.Test.Helpers.BaseHelper;

namespace Api.Service.Test.Portfolio
{
    public class PortfolioTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;
        
        protected Mock<IPortfolioRepository> RepositoryMock = new Mock<IPortfolioRepository>();
        protected Mock<ITrashService> TrashServiceMock = new Mock<ITrashService>();
        protected List<PortfolioModel> listPortfolioModel = new List<PortfolioModel>();
        protected List<PortfolioModel> listPortfolioModelResult = new List<PortfolioModel>();
        protected PortfolioModel PortfolioModel;
        protected PortfolioModel PortfolioModelResult;
        protected PortfolioModel PortfolioModelUpdate;
        protected PortfolioModel PortfolioModelUpdateResult;
        protected PageParams pageParams;
        protected TrashModel trashModel;

        protected PortfolioTest()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            var parentModel = new PortfolioModel
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                var model = new PortfolioModel()
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Status = GetStatusTypeRandom(),
                    ParentPortfolioId = parentModel.Id,
                    ParentPortfolio = parentModel,
                    CategoryId = categoryModel.Id,
                    Category = categoryModel,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listPortfolioModel.Add(model);
            }

            listPortfolioModelResult = listPortfolioModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                     .Take(pageParams.PageSize)
                                                     .ToList();

            PortfolioModel = new PortfolioModel
            {
                Id = 2,
                Name = "Cash",
                Status = StatusType.Ativo,
                ParentPortfolioId = parentModel.Id,
                ParentPortfolio = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            PortfolioModelResult = new PortfolioModel
            {
                Id = PortfolioModel.Id,
                Name = PortfolioModel.Name,
                Status = PortfolioModel.Status,
                ParentPortfolioId = PortfolioModel.ParentPortfolioId,
                ParentPortfolio = PortfolioModel.ParentPortfolio,
                CategoryId = PortfolioModel.CategoryId,
                Category = PortfolioModel.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            PortfolioModelUpdate = new PortfolioModel
            {
                Id = PortfolioModel.Id,
                Name = "Carteira",
                Status = StatusType.Inativo,
                ParentPortfolioId = parentModel.Id,
                ParentPortfolio = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            PortfolioModelUpdateResult = new PortfolioModel
            {
                Id = PortfolioModelUpdate.Id,
                Name = PortfolioModelUpdate.Name,
                Status = PortfolioModelUpdate.Status,
                ParentPortfolioId = PortfolioModelUpdate.ParentPortfolioId,
                ParentPortfolio = PortfolioModelUpdate.ParentPortfolio,
                CategoryId = PortfolioModelUpdate.CategoryId,
                Category = PortfolioModelUpdate.Category,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
            
            trashModel = new TrashModel()
            {
                Reference = "portfolio",
                ReferenceId = PortfolioModel.Id
            };
        }

        protected void ApplyTest(PortfolioModel PortfolioModelSource, PortfolioModel PortfolioModelDest)
        {
            Assert.NotNull(PortfolioModelDest);
            Assert.NotNull(PortfolioModelSource.User);
            Assert.True(PortfolioModelSource.UserId > 0);
            Assert.Equal(PortfolioModelSource.Id, PortfolioModelDest.Id);
            Assert.Equal(PortfolioModelSource.Name, PortfolioModelDest.Name);
            Assert.Equal(PortfolioModelSource.Status, PortfolioModelDest.Status);
            Assert.Equal(PortfolioModelSource.CategoryId, PortfolioModelDest.CategoryId);
            Assert.Equal(PortfolioModelSource.Category.Id, PortfolioModelDest.Category.Id);
            Assert.Equal(PortfolioModelSource.ParentPortfolioId, PortfolioModelDest.ParentPortfolioId);
            Assert.Equal(PortfolioModelSource.ParentPortfolio.Id, PortfolioModelDest.ParentPortfolio.Id);
            Assert.Equal(PortfolioModelSource.UserId, PortfolioModelDest.UserId);
            Assert.Equal(PortfolioModelSource.User.Id, PortfolioModelDest.User.Id);
        }
    }
}