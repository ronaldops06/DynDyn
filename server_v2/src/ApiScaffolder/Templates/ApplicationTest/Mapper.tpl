using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.{{model}};
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class {{model}}Mapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            //Referências
            
            var {{alias}}RequestDto = new {{model}}RequestDto()
            {
                Id = 1
            };

            //Dto -> Model
            var model = Mapper.Map<{{model}}Model>({{alias}}RequestDto);
            Assert.Equal(model.Id, {{alias}}RequestDto.Id);
            //

            //Model -> DtoResult
            var {{alias}}ResponseDto = Mapper.Map<{{model}}ResponseDto>(model);
            Assert.Equal({{alias}}ResponseDto.Id, model.Id);
            //
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            //Referências

            var listModel = new List<{{model}}Model>();

            for (int i = 1; i <= 5; i++)
            {
                var {{alias}}Model = new {{model}}Model
                {
                    Id = i,
                    //
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add({{alias}}Model);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<{{model}}ResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
               //
            }

            var pageList = new PageList<{{model}}Model>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<{{model}}ResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                //
            }
        }
    }
}