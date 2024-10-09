using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class OperationMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var categoryModel = new CategoryModel()
            {
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
            };

            var model = new OperationModel()
            {
                Name = "Compra Monitor",
                Recurrent = false,
                Type = OperationType.Debito,
                Status = StatusType.Ativo,
                CategoryId = categoryModel.Id,
                Category = categoryModel,
            };

            //Model -> Entity
            var entity = Mapper.Map<OperationEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Recurrent, model.Recurrent);
            Assert.Equal(entity.Type, model.Type);
            Assert.Equal(entity.Status, model.Status);
            Assert.Equal(entity.CategoryId, model.CategoryId);
            Assert.Equal(entity.Category.Id, model.Category.Id);

            //Entity -> Model
            var operationModel = Mapper.Map<OperationModel>(entity);
            Assert.Equal(operationModel.Id, entity.Id);
            Assert.Equal(operationModel.Name, entity.Name);
            Assert.Equal(operationModel.Recurrent, entity.Recurrent);
            Assert.Equal(operationModel.Type, entity.Type);
            Assert.Equal(operationModel.Status, entity.Status);
            Assert.Equal(operationModel.CategoryId, entity.CategoryId);
            Assert.Equal(operationModel.Category.Id, entity.Category.Id);
            Assert.Equal(operationModel.DataCriacao, entity.DataCriacao);
            Assert.Equal(operationModel.DataAlteracao, entity.DataAlteracao);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var categoryEntity = new CategoryEntity()
            {
                Name = "Corrente",
                Type = CategoryType.Conta,
                Status = StatusType.Ativo,
            };

            var listEntity = new List<OperationEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new OperationEntity
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Recurrent = true,
                    Type = OperationType.Credito,
                    Status = StatusType.Ativo,
                    CategoryId = categoryEntity.Id,
                    Category = categoryEntity,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<OperationModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].Recurrent, listEntity[i].Recurrent);
                Assert.Equal(listModel[i].Type, listEntity[i].Type);
                Assert.Equal(listModel[i].Status, listEntity[i].Status);
                Assert.Equal(listModel[i].CategoryId, listEntity[i].CategoryId);
                Assert.Equal(listModel[i].Category.Id, listEntity[i].Category.Id);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
            }
        }
    }
}