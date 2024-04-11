using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Operation;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class OperationMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var categoryDto = new CategoryRequestDto()
            {
                Nome = "Corrente",
                Tipo = (int)CategoryType.Conta,
                Status = (int)StatusType.Ativo,
            };

            var operationRequestDto = new OperationRequestDto()
            {
                Name = "Compra Monitor",
                Recurrent = false,
                Type = (int)OperationType.Debito,
                Status = (int)StatusType.Ativo,
                Category = categoryDto,
            };

            //Dto -> Model
            var model = Mapper.Map<OperationModel>(operationRequestDto);
            Assert.Equal(model.Id, operationRequestDto.Id);
            Assert.Equal(model.Name, operationRequestDto.Name);
            Assert.Equal(model.Recurrent, operationRequestDto.Recurrent);
            Assert.Equal((int)model.Type, operationRequestDto.Type);
            Assert.Equal((int)model.Status, operationRequestDto.Status);
            Assert.Equal(model.Category.Id, operationRequestDto.Category.Id);
            Assert.Equal(model.CategoryId, operationRequestDto.Category.Id);

            //Model -> DtoResult
            var operationResponseDto = Mapper.Map<OperationResponseDto>(model);
            Assert.Equal(operationResponseDto.Id, model.Id);
            Assert.Equal(operationResponseDto.Name, model.Name);
            Assert.Equal(operationResponseDto.Recurrent, model.Recurrent);
            Assert.Equal(operationResponseDto.Type, (int)model.Type);
            Assert.Equal(operationResponseDto.Status, (int)model.Status);
            Assert.Equal(operationResponseDto.Category.Id, model.Category.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var categoryModel = new CategoryModel
            {
                Id = 1,
                Nome = "Corrente",
                Tipo = CategoryType.Conta,
                Status = StatusType.Ativo
            };


            var listModel = new List<OperationModel>();

            for (int i = 1; i <= 5; i++)
            {
                var operationModel = new OperationModel
                {
                    Id = i,
                    Name = Faker.Name.FullName(),
                    Type = OperationType.Credito,
                    Recurrent = false,
                    Status = StatusType.Ativo,
                    CategoryId = categoryModel.Id,
                    Category = categoryModel,
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add(operationModel);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<OperationResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
                Assert.Equal(listDto[i].Recurrent, listModel[i].Recurrent);
                Assert.Equal(listDto[i].Type, (int)listModel[i].Type);
                Assert.Equal(listDto[i].Status, (int)listModel[i].Status);
                Assert.Equal(listDto[i].Category.Id, listModel[i].Category.Id);
            }

            var pageList = new PageList<OperationModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<OperationResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Name, listModel[i].Name);
                Assert.Equal(listResponseDto[i].Recurrent, listModel[i].Recurrent);
                Assert.Equal(listResponseDto[i].Type, (int)listModel[i].Type);
                Assert.Equal(listResponseDto[i].Status, (int)listModel[i].Status);
                Assert.Equal(listResponseDto[i].Category.Id, listModel[i].Category.Id);
            }
        }
    }
}