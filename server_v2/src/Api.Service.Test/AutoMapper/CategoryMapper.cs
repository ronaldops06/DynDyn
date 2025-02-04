using Api.Domain.Dtos.Category;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
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
                Name = Faker.Lorem.GetFirstWord(),
                Type = (int)CategoryType.Conta,
                Status = (int)StatusType.Ativo,
            };

            //Dto -> Model
            var model = Mapper.Map<CategoryModel>(dto);
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Name, dto.Name);
            Assert.Equal((int)model.Type, dto.Type);
            Assert.Equal((int)model.Status, dto.Status);

            model.User = UserModelFake;
            model.UserId = UserModelFake.Id;
            
            //Model -> Entity
            var entity = Mapper.Map<CategoryEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Name, model.Name);
            Assert.Equal(entity.Type, model.Type);
            Assert.Equal(entity.Status, model.Status);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var categoryModel = Mapper.Map<CategoryModel>(entity);
            Assert.Equal(categoryModel.Id, entity.Id);
            Assert.Equal(categoryModel.Name, entity.Name);
            Assert.Equal(categoryModel.Type, entity.Type);
            Assert.Equal(categoryModel.Status, entity.Status);
            Assert.Equal(categoryModel.DataCriacao, entity.DataCriacao);
            Assert.Equal(categoryModel.DataAlteracao, entity.DataAlteracao);
            Assert.Equal(categoryModel.UserId, entity.UserId);
            Assert.Equal(categoryModel.User.Id, entity.User.Id);

            //Model -> DtoResult
            var categoryResponseDto = Mapper.Map<CategoryResponseDto>(categoryModel);
            Assert.Equal(categoryResponseDto.Id, categoryModel.Id);
            Assert.Equal(categoryResponseDto.Name, categoryModel.Name);
            Assert.Equal(categoryResponseDto.Type, (int)categoryModel.Type);
            Assert.Equal(categoryResponseDto.Status, (int)categoryModel.Status);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            
            var listEntity = new List<CategoryEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new CategoryEntity
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Type = GetCategoryTypeRandom(),
                    Status = GetStatusTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow,
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<CategoryModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Name, listEntity[i].Name);
                Assert.Equal(listModel[i].Status, listEntity[i].Status);
                Assert.Equal(listModel[i].Type, listEntity[i].Type);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<CategoryResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
                Assert.Equal(listDto[i].Type, (int)listModel[i].Type);
                Assert.Equal(listDto[i].Status, (int)listModel[i].Status);
            }

            var pageList = new PageList<CategoryModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<CategoryResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Name, listModel[i].Name);
                Assert.Equal(listResponseDto[i].Type, (int)listModel[i].Type);
                Assert.Equal(listResponseDto[i].Status, (int)listModel[i].Status);
            }
        }
    }
}