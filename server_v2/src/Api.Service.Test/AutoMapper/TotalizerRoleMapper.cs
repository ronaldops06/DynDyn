using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class TotalizerRoleMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            
            var model = new TotalizerRoleModel
            {
                Id = 2,
                Code = "Cash",
                Type = TotalizerType.Discriminated,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            //Model -> Entity
            var entity = Mapper.Map<TotalizerRoleEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Code, model.Code);
            Assert.Equal(entity.Type, model.Type);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var totalizerRoleModel = Mapper.Map<TotalizerRoleEntity>(entity);
            Assert.Equal(totalizerRoleModel.Id, entity.Id);
            Assert.Equal(totalizerRoleModel.Code, entity.Code);
            Assert.Equal(totalizerRoleModel.Type, entity.Type);
            Assert.Equal(totalizerRoleModel.UserId, entity.UserId);
            Assert.Equal(totalizerRoleModel.User.Id, entity.User.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            

            var listEntity = new List<TotalizerRoleEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new TotalizerRoleEntity
                {
                    Id = i,
                    Code = Faker.Name.FullName(),
                    //
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<TotalizerRoleModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                //
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }
    }
}