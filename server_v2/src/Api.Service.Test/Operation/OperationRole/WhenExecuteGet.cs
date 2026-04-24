using Api.Domain.Entities;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation.OperationRole
{
    public class WhenExecuteGet : OperationRoleTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var operationRoleEntityResult = Mapper.Map<OperationRoleEntity>(operationRoleModelResult);
            var listOperationRoleEntity = Mapper.Map<List<OperationRoleEntity>>(listOperationRoleModelResult);

            var data = new Data<OperationRoleEntity>(listOperationRoleEntity.Count, listOperationRoleEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(operationRoleEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(operationRoleEntityResult);
            OperationRoleService service = new OperationRoleService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById(operationRoleModelResult.Id);
            ApplyTest(operationRoleModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            //Métodos específicos
        }
    }
}