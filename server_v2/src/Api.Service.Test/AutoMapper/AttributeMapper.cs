using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class AttributeMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            
            var model = new AttributeModel
            {
                Id = 2,
                Name = "Cash",
                //
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            //Model -> Entity
            var entity = Mapper.Map<AttributeEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            //
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var attributeModel = Mapper.Map<AttributeEntity>(entity);
            Assert.Equal(attributeModel.Id, entity.Id);
            //
            Assert.Equal(attributeModel.UserId, entity.UserId);
            Assert.Equal(attributeModel.User.Id, entity.User.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            

            var listEntity = new List<AttributeEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new AttributeEntity
                {
                    Id = i,
                    //
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<AttributeModel>>(listEntity);

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