using Api.Domain.Dtos.Category;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;
using static Api.Service.Test.Helpers.CategoryHelpers;
using static Api.Service.Test.Helpers.BaseHelper;

namespace Api.Service.Test.AutoMapper
{
    public class CategoryMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var dto = new CategoryRequestDto
            {
                Id = 1,
                Nome = Faker.Lorem.GetFirstWord(),
                Tipo = (int)CategoryType.Conta,
                Status = (int)StatusType.Ativo,
            };

            //Dto -> Model
            var model = Mapper.Map<CategoryModel>(dto);
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Nome, dto.Nome);
            Assert.Equal((int)model.Tipo, dto.Tipo);
            Assert.Equal((int)model.Status, dto.Status);

            //Model -> Entity
            var entity = Mapper.Map<CategoryEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Nome, model.Nome);
            Assert.Equal(entity.Tipo, model.Tipo);
            Assert.Equal(entity.Status, model.Status);

            //Entity -> Model
            var categoryModel = Mapper.Map<CategoryModel>(entity);
            Assert.Equal(categoryModel.Id, entity.Id);
            Assert.Equal(categoryModel.Nome, entity.Nome);
            Assert.Equal(categoryModel.Tipo, entity.Tipo);
            Assert.Equal(categoryModel.Status, entity.Status);
            Assert.Equal(categoryModel.DataCriacao, entity.DataCriacao);
            Assert.Equal(categoryModel.DataAlteracao, entity.DataAlteracao);

            //Model -> DtoResult
            var categoryResponseDto = Mapper.Map<CategoryResponseDto>(categoryModel);
            Assert.Equal(categoryResponseDto.Id, categoryModel.Id);
            Assert.Equal(categoryResponseDto.Nome, categoryModel.Nome);
            Assert.Equal(categoryResponseDto.Tipo, (int)categoryModel.Tipo);
            Assert.Equal(categoryResponseDto.Status, (int)categoryModel.Status);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var listEntity = new List<CategoryEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new CategoryEntity
                {
                    Id = i,
                    Nome = Faker.Name.FullName(),
                    Tipo = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<CategoryModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Nome, listEntity[i].Nome);
                Assert.Equal(listModel[i].Status, listEntity[i].Status);
                Assert.Equal(listModel[i].Tipo, listEntity[i].Tipo);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<CategoryResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Nome, listModel[i].Nome);
                Assert.Equal(listDto[i].Tipo, (int)listModel[i].Tipo);
                Assert.Equal(listDto[i].Status, (int)listModel[i].Status);
            }

            var pageList = new PageList<CategoryModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<CategoryResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Nome, listModel[i].Nome);
                Assert.Equal(listResponseDto[i].Tipo, (int)listModel[i].Tipo);
                Assert.Equal(listResponseDto[i].Status, (int)listModel[i].Status);
            }
        }
    }
}