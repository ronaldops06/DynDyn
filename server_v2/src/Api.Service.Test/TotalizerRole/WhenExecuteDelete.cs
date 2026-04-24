using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.TotalizerRole
{
    public class WhenExecuteDelete : TotalizerRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var totalizerRoleEntity = Mapper.Map<TotalizerRoleEntity>(totalizerRoleModel);

            TrashServiceMock.Setup(m => m.Post(trashModel));
            
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(totalizerRoleEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            TotalizerRoleService service = new TotalizerRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Delete(totalizerRoleModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new TotalizerRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}