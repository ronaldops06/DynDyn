using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.Balance
{
    public class WhenExecuteUpdate : BalanceTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var balanceEntityUpdateResult = Mapper.Map<BalanceEntity>(balanceModelUpdateResult);
            var balanceEntityUpdate = Mapper.Map<BalanceEntity>(balanceModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(balanceEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(balanceEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<BalanceEntity>())).ReturnsAsync(balanceEntityUpdateResult);
            BalanceService service = new BalanceService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(balanceModelUpdate);
            ApplyTest(balanceModelUpdate, resultUpdate);
        }
    }
}