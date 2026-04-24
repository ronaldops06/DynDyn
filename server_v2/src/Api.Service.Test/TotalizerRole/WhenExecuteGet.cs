using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.TotalizerRole
{
    public class WhenExecuteGet : TotalizerRoleTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var totalizerRoleEntityResult = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModelResult);
            var listTotalizerRoleEntity = Mapper.Map<List<TotalizerRoleEntity>>(listTotalizerRoleModelResult);

            var data = new Data<TotalizerRoleEntity>(listTotalizerRoleEntity.Count, listTotalizerRoleEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(totalizerRoleEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TotalizerType>())).ReturnsAsync(totalizerRoleEntityResult);
            TotalizerRoleService service = new TotalizerRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById(totalizerRoleModelResult.Id);
            ApplyTest(totalizerRoleModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            //Métodos específicos
        }
    }
}