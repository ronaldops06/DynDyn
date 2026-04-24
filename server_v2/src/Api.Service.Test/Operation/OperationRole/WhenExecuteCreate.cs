using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Api.Service.Test.Operation.OperationRole;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation.OperationRole
{
    public class WhenExecuteCreate : OperationRoleTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var operationRoleEntityResult = Mapper.Map<OperationRoleEntity>(operationRoleModelResult);
            var operationRoleEntity = Mapper.Map<OperationRoleEntity>(operationRoleModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<OperationRoleEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<OperationRoleEntity>())).ReturnsAsync(operationRoleEntityResult);
            OperationRoleService service = new OperationRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post(operationRoleModel);
            ApplyTest(operationRoleModel, result);
        }
    }
}