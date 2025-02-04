using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.Balance
{
    public class WhenExecuteDelete : BalanceTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var balanceEntity = Mapper.Map<BalanceEntity>(balanceModel);
   
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(balanceEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            BalanceService service = new BalanceService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var result = await service.Delete(balanceModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new BalanceService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}