using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.TotalizerRole
{
    public class WhenExecuteUpdate : TotalizerRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var totalizerRoleEntityUpdateResult = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModelUpdateResult);
            var totalizerRoleEntityUpdate = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TotalizerType>())).ReturnsAsync(totalizerRoleEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(totalizerRoleEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<TotalizerRoleEntity>())).ReturnsAsync(totalizerRoleEntityUpdateResult);
            TotalizerRoleService service = new TotalizerRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(totalizerRoleModelUpdate);
            ApplyTest(totalizerRoleModelUpdate, resultUpdate);
        }
    }
}