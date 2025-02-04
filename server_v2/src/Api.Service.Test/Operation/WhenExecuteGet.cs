using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation
{
    public class WhenExecuteGet : OperationTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var operationEntityResult = Mapper.Map<OperationEntity>(operationModelResult);
            var listOperationEntity = Mapper.Map<List<OperationEntity>>(listOperationModelResult);

            var data = new Data<OperationEntity>(listOperationEntity.Count, listOperationEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(operationEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OperationType>())).ReturnsAsync(operationEntityResult);
            OperationService service = new OperationService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var resultById = await service.GetById(operationModelResult.Id);
            ApplyTest(operationModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            var resultByNameAndType = await service.GetByNameAndType(operationModelResult.Name, operationModelResult.Type);
            ApplyTest(operationModelResult, resultByNameAndType);
            Assert.NotEqual(1, resultByNameAndType.Id);
        }
    }
}