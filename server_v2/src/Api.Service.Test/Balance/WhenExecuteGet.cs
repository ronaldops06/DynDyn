using Api.Domain.Entities;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.Balance
{
    public class WhenExecuteGet : BalanceTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var balanceEntityResult = Mapper.Map<BalanceEntity>(balanceModelResult);
            var listBalanceEntity = Mapper.Map<List<BalanceEntity>>(listBalanceModelResult);

            var data = new Data<BalanceEntity>(listBalanceEntity.Count, listBalanceEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync(balanceEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            BalanceService service = new BalanceService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById(balanceModelResult.Id);
            ApplyTest(balanceModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);
        }
    }
}