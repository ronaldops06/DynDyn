using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class AccountMapper : BaseTestService
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

            var parentModel = new AccountModel
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            var model = new AccountModel
            {
                Id = 2,
                Name = "Cash",
                Status = StatusType.Ativo,
                ParentAccountId = parentModel.Id,
                ParentAccount = parentModel,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            //Model -> Entity
            var entity = Mapper.Map<AccountEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Status, model.Status);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Category.Id, model.Category.Id);
            Assert.Equal(entity.ParentAccountId, model.ParentAccountId);
            Assert.Equal(entity.ParentAccount.Id, model.ParentAccount.Id);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var accountModel = Mapper.Map<AccountModel>(entity);
            Assert.Equal(accountModel.Id, entity.Id);
            Assert.Equal(accountModel.Name, entity.Name);
            Assert.Equal(accountModel.Status, entity.Status);
            Assert.Equal(accountModel.CategoryId, entity.CategoryId);
            Assert.Equal(accountModel.Category.Id, entity.Category.Id);
            Assert.Equal(accountModel.ParentAccountId, entity.ParentAccountId);
            Assert.Equal(accountModel.ParentAccount.Id, entity.ParentAccount.Id);
            Assert.Equal(accountModel.DataCriacao, entity.DataCriacao);
            Assert.Equal(accountModel.DataAlteracao, entity.DataAlteracao);
            Assert.Equal(accountModel.UserId, entity.UserId);
            Assert.Equal(accountModel.User.Id, entity.User.Id);
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

            var parentEntity = new AccountEntity
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = categoryEntity.Id,
                Category = categoryEntity,
                User = userEntity,
                UserId = userEntity.Id
            };

            var listEntity = new List<AccountEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new AccountEntity
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Status = StatusType.Ativo,
                    ParentAccountId = parentEntity.Id,
                    ParentAccount = parentEntity,
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
            var listModel = Mapper.Map<List<AccountModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].Status, listEntity[i].Status);
                Assert.Equal(listModel[i].CategoryId, listEntity[i].CategoryId);
                Assert.Equal(listModel[i].Category.Id, listEntity[i].Category.Id);
                Assert.Equal(listModel[i].ParentAccountId, listEntity[i].ParentAccountId);
                Assert.Equal(listModel[i].ParentAccount.Id, listEntity[i].ParentAccount.Id);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }
    }
}