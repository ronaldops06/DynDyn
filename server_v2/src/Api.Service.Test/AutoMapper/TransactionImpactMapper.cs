using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class TransactionImpactMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            
            var model = new TransactionImpactModel
            {
                Id = 2,
                //
            };

            //Model -> Entity
            var entity = Mapper.Map<TransactionImpactEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            //


            //Entity -> Model
            var transactionImpactModel = Mapper.Map<TransactionImpactEntity>(entity);
            Assert.Equal(transactionImpactModel.Id, entity.Id);
            //

        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            

            var listEntity = new List<TransactionImpactEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new TransactionImpactEntity
                {
                    Id = i,
                    //
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<TransactionImpactModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                //

            }
        }
    }
}