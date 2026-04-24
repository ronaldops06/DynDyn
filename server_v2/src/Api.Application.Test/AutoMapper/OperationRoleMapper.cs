using Api.Domain.Dtos.Operation;
using Api.Domain.Models;
using Domain.Helpers;
using Faker;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class OperationRoleMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var transactionRuleRequestDto = new OperationRoleRequestDto()
            {
                Id = 1,
                Name = "INSS"
            };

            //Dto -> Model
            var model = Mapper.Map<OperationRoleModel>(transactionRuleRequestDto);
            Assert.Equal(model.Id, transactionRuleRequestDto.Id);
            Assert.Equal(model.Name, transactionRuleRequestDto.Name);

            //Model -> DtoResult
            var transactionRuleResponseDto = Mapper.Map<OperationRoleResponseDto>(model);
            Assert.Equal(transactionRuleResponseDto.Id, model.Id);
            Assert.Equal(transactionRuleResponseDto.Name, model.Name);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var listModel = new List<OperationRoleModel>();

            for (int i = 1; i <= 5; i++)
            {
                var transactionRuleModel = new OperationRoleModel
                {
                    Id = i,
                    Name = Name.FullName(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add(transactionRuleModel);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<OperationRoleResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
                Assert.Equal(listDto[i].Name, listModel[i].Name);
            }

            var pageList = new PageList<OperationRoleModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<OperationRoleResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                Assert.Equal(listResponseDto[i].Name, listModel[i].Name);
            }
        }
    }
}