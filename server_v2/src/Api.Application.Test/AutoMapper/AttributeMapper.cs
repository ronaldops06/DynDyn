using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;
using static Api.Application.Test.Helpers.BaseHelper;
using static Api.Application.Test.Helpers.AttributeHelper;

namespace Api.Application.Test.AutoMapper
{
    public class AttributeMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            //Referências
            
            var attributeRequestDto = new AttributeRequestDto()
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Description = Faker.Lorem.Sentence(),
                Status = (int)GetStatusTypeRandom(),
                DataType = (int)GetAttributeDataTypeRandom(),
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            //Dto -> Model
            var model = Mapper.Map<AttributeModel>(attributeRequestDto);
            Assert.Equal(model.Id, attributeRequestDto.Id);
            Assert.Equal(model.Name, attributeRequestDto.Name);
            Assert.Equal(model.Description, attributeRequestDto.Description);
            Assert.Equal((int)model.DataType, attributeRequestDto.DataType);
            Assert.Equal((int)model.Status, attributeRequestDto.Status);

            //Model -> DtoResult
            var attributeResponseDto = Mapper.Map<AttributeResponseDto>(model);
            Assert.Equal(attributeResponseDto.Id, model.Id);
            Assert.Equal(attributeResponseDto.Name, model.Name);
            Assert.Equal(attributeResponseDto.Description, model.Description);
            Assert.Equal(attributeResponseDto.DataType, (int)model.DataType);
            Assert.Equal(attributeResponseDto.Status, (int)model.Status);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            //Referências

            var listModel = new List<AttributeModel>();

            for (int i = 1; i <= 5; i++)
            {
                var attributeModel = new AttributeModel
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Description = Faker.Lorem.Sentence(),
                    Status = GetStatusTypeRandom(),
                    DataType = GetAttributeDataTypeRandom(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add(attributeModel);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<AttributeResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
                Assert.Equal(listDto[i].Description, listModel[i].Description);
                Assert.Equal(listDto[i].DataType, (int)listModel[i].DataType);
                Assert.Equal(listDto[i].Status, (int)listModel[i].Status);
            }

            var pageList = new PageList<AttributeModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<AttributeResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Name, listModel[i].Name);
                Assert.Equal(listResponseDto[i].Description, listModel[i].Description);
                Assert.Equal(listResponseDto[i].DataType, (int)listModel[i].DataType);
                Assert.Equal(listResponseDto[i].Status, (int)listModel[i].Status);
            }
        }
    }
}