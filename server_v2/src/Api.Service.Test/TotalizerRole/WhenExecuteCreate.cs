using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.TotalizerRole
{
    public class WhenExecuteCreate : TotalizerRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var totalizerRoleEntityResult = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModelResult);
            var totalizerRoleEntity = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TotalizerType>())).ReturnsAsync(It.IsAny<TotalizerRoleEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TotalizerRoleEntity>())).ReturnsAsync(totalizerRoleEntityResult);
            TotalizerRoleService service = new TotalizerRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post(totalizerRoleModel);
            ApplyTest(totalizerRoleModel, result);
        }
    }
}