using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation.OperationRole
{
    public class WhenExecuteUpdate : OperationRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var operationRoleEntityUpdateResult = Mapper.Map<OperationRoleEntity>(operationRoleModelUpdateResult);
            var operationRoleEntityUpdate = Mapper.Map<OperationRoleEntity>(operationRoleModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(operationRoleEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(operationRoleEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<OperationRoleEntity>())).ReturnsAsync(operationRoleEntityUpdateResult);
            OperationRoleService service = new OperationRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(operationRoleModelUpdate);
            ApplyTest(operationRoleModelUpdate, resultUpdate);
        }
    }
}