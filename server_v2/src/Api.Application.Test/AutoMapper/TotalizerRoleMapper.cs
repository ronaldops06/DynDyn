using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.TotalizerRole;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;
using Xunit;

namespace Api.Application.Test.AutoMapper
{
    public class TotalizerRoleMapper : BaseTestApplication
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            //Referências
            
            var totalizerRoleRequestDto = new TotalizerRoleRequestDto()
            {
                Id = 1
            };

            //Dto -> Model
            var model = Mapper.Map<TotalizerRoleModel>(totalizerRoleRequestDto);
            Assert.Equal(model.Id, totalizerRoleRequestDto.Id);
            //

            //Model -> DtoResult
            var totalizerRoleResponseDto = Mapper.Map<TotalizerRoleResponseDto>(model);
            Assert.Equal(totalizerRoleResponseDto.Id, model.Id);
            //
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            //Referências

            var listModel = new List<TotalizerRoleModel>();

            for (int i = 1; i <= 5; i++)
            {
                var totalizerRoleModel = new TotalizerRoleModel
                {
                    Id = i,
                    //
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow

                };
                listModel.Add(totalizerRoleModel);
            }

            //List<Model> -> List<Dto>
            var listDto = Mapper.Map<List<TotalizerRoleResponseDto>>(listModel);

            Assert.True(listDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listDto[i].Id, listModel[i].Id);
               //
            }

            var pageList = new PageList<TotalizerRoleModel>(listModel, listModel.Count, 1, listModel.Count);

            //PageList -> DtoResult
            var listResponseDto = Mapper.Map<List<TotalizerRoleResponseDto>>(pageList);

            Assert.True(listResponseDto.Count() == listModel.Count());

            for (int i = 0; i < listDto.Count(); i++)
            {
                Assert.Equal(listResponseDto[i].Id, listModel[i].Id);
                //
            }
        }
    }
}