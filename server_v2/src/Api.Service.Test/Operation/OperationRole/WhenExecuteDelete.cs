using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation.OperationRole
{
    public class WhenExecuteDelete : OperationRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var operationRoleEntity = Mapper.Map<OperationRoleEntity>(operationRoleModel);

            TrashServiceMock.Setup(m => m.Post(trashModel));
            
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(operationRoleEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            OperationRoleService service = new OperationRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Delete(operationRoleModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new OperationRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}