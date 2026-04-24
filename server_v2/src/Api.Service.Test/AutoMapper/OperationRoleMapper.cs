using Api.Domain.Entities;
using Api.Domain.Models;
using Domain.Entities;
using Faker;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class OperationRoleMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            
            var model = new OperationRoleModel
            {
                Id = 2,
                Name = "FGTS",
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            //Model -> Entity
            var entity = Mapper.Map<OperationRoleEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var operationRoleModel = Mapper.Map<OperationRoleEntity>(entity);
            Assert.Equal(operationRoleModel.Id, entity.Id);
            Assert.Equal(operationRoleModel.Name, entity.Name);
            Assert.Equal(operationRoleModel.UserId, entity.UserId);
            Assert.Equal(operationRoleModel.User.Id, entity.User.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            

            var listEntity = new List<OperationRoleEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new OperationRoleEntity
                {
                    Id = i,
                    Name = Name.FullName(),
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<OperationRoleModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }
    }
}