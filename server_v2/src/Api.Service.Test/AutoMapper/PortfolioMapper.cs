using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class PortfolioMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
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

            var model = new PortfolioModel
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

            //Model -> Entity
            var entity = Mapper.Map<PortfolioEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Status, model.Status);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Category.Id, model.Category.Id);
            Assert.Equal(entity.ParentPortfolioId, model.ParentPortfolioId);
            Assert.Equal(entity.ParentPortfolio.Id, model.ParentPortfolio.Id);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var portfolioModel = Mapper.Map<PortfolioEntity>(entity);
            Assert.Equal(portfolioModel.Id, entity.Id);
            Assert.Equal(portfolioModel.Name, entity.Name);
            Assert.Equal(portfolioModel.Status, entity.Status);
            Assert.Equal(portfolioModel.CategoryId, entity.CategoryId);
            Assert.Equal(portfolioModel.Category.Id, entity.Category.Id);
            Assert.Equal(portfolioModel.ParentPortfolioId, entity.ParentPortfolioId);
            Assert.Equal(portfolioModel.ParentPortfolio.Id, entity.ParentPortfolio.Id);
            Assert.Equal(portfolioModel.DataCriacao, entity.DataCriacao);
            Assert.Equal(portfolioModel.DataAlteracao, entity.DataAlteracao);
            Assert.Equal(portfolioModel.UserId, entity.UserId);
            Assert.Equal(portfolioModel.User.Id, entity.User.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            
            var categoryEntity = new CategoryEntity
            {
                Id = 1,
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
                User = userEntity,
                UserId = userEntity.Id
            };

            var parentEntity = new PortfolioEntity
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryEntity.Id,
                Category = categoryEntity,
                User = userEntity,
                UserId = userEntity.Id
            };

            var listEntity = new List<PortfolioEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new PortfolioEntity
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Status = StatusType.Ativo,
                    ParentPortfolioId = parentEntity.Id,
                    ParentPortfolio = parentEntity,
                    CategoryId = categoryEntity.Id,
                    Category = categoryEntity,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<PortfolioModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].Status, listEntity[i].Status);
                Assert.Equal(listModel[i].CategoryId, listEntity[i].CategoryId);
                Assert.Equal(listModel[i].Category.Id, listEntity[i].Category.Id);
                Assert.Equal(listModel[i].ParentPortfolioId, listEntity[i].ParentPortfolioId);
                Assert.Equal(listModel[i].ParentPortfolio.Id, listEntity[i].ParentPortfolio.Id);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }
    }
}